using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DBI.Core.Security;
using DBI.Core.Web;
using DBI.Data;
using Ext.Net;
using System.Data.Entity;

namespace DBI.Web.EMS.Views.Modules.CustomerSurveys
{
    public partial class umManageOrgs : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!validateComponentSecurity("SYS.CustomerSurveys.ManageOrgs"))
            {
                X.Redirect("~/Views/uxDefault.aspx");

            }
        }

        protected void deReadDollars(object sender, StoreReadDataEventArgs e)
        {
            string _selectedRecordID = uxCompanySelectionModel.SelectedRecordID;
            if (_selectedRecordID != "0" && _selectedRecordID.Contains(":"))
            {
                long OrgID = GetOrgFromTree(_selectedRecordID);
                using (Entities _context = new Entities())
                {
                    IQueryable<CUSTOMER_SURVEY_THRESH_AMT> Amounts = CUSTOMER_SURVEYS.GetOrganizationThresholdAmounts(OrgID, _context);
                    foreach (var item in Amounts)
                    {
                        item.ORG_HIER = _context.ORG_HIER_V.Where(x => x.ORG_ID == item.ORG_ID).Select(x => x.ORG_HIER).Distinct().Single();
                    }
                    int count;
                    var data = GenericData.ListFilterHeader(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], Amounts, out count);
                    
                    uxDollarStore.DataSource = data;
                    e.Total = count;
                }
            }
        }

        protected void deReadFormTypes(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                uxFormTypeStore.DataSource = _context.SURVEY_TYPES.ToList();
            }
        }

        protected long GetOrgFromTree(string _selectedRecordID)
        {
            if (_selectedRecordID.Contains(":"))
            {
                char[] _delimiterChars = { ':' };
                string[] _selectedID = _selectedRecordID.Split(_delimiterChars);
                return long.Parse(_selectedID[1].ToString());
            }
            else
            {
                return long.Parse(_selectedRecordID);
            }
        }

        protected void deLoadOrgTree(object sender, NodeLoadEventArgs e)
        {
            // User clicked on legal entity or hierarchy
            if (e.NodeID.Contains(":") == false)
            {
                if (e.NodeID == "0")
                {
                    var data = HR.ActiveOverheadBudgetLegalEntities();
                    foreach (var view in data)
                    {
                        Node node = new Node();
                        node.Text = view.ORGANIZATION_NAME;
                        node.NodeID = view.ORGANIZATION_ID.ToString();
                        e.Nodes.Add(node);
                    }
                }

                else
                {
                    long nodeID = long.Parse(e.NodeID);
                    var data = HR.LegalEntityHierarchies().Where(a => a.ORGANIZATION_ID == nodeID);
                    foreach (var view in data)
                    {
                        Node node = new Node();
                        node.Text = view.HIERARCHY_NAME;
                        node.NodeID = string.Format("{0}:{1}", view.ORGANIZATION_STRUCTURE_ID.ToString(), view.ORGANIZATION_ID.ToString());
                        e.Nodes.Add(node);
                    }
                }
            }

            // User clicked on org
            else
            {
                char[] delimChars = { ':' };
                string[] selID = e.NodeID.Split(delimChars);
                long hierarchyID = long.Parse(selID[0].ToString());
                long orgID = long.Parse(selID[1].ToString());
                var data = HR.ActiveOrganizationsByHierarchy(hierarchyID, orgID);
                var OrgsList = SYS_USER_ORGS.GetUserOrgs(SYS_USER_INFORMATION.UserID(User.Identity.Name)).Select(x => x.ORG_ID);
                bool addNode;
                bool leafNode;
                bool colorNode;

                foreach (var view in data)
                {
                    if (view.HIER_LEVEL == 1)
                    {
                        addNode = false;
                        leafNode = true;
                        colorNode = false;
                        var nextData = HR.ActiveOrganizationsByHierarchy(hierarchyID, view.ORGANIZATION_ID);

                        // In this org?
                        if (SYS_USER_ORGS.IsInOrg(SYS_USER_INFORMATION.UserID(User.Identity.Name), view.ORGANIZATION_ID) == true)
                        {
                            addNode = true;
                            colorNode = true;
                        }

                        // In next org?
                        foreach (long allowedOrgs in OrgsList)
                        {
                            if (nextData.Select(x => x.ORGANIZATION_ID).Contains(allowedOrgs))
                            {
                                addNode = true;
                                leafNode = false;
                                break;
                            }
                        }

                        if (addNode == true)
                        {
                            Node node = new Node();
                            node.NodeID = string.Format("{0}:{1}", hierarchyID.ToString(), view.ORGANIZATION_ID.ToString());
                            node.Text = view.ORGANIZATION_NAME;
                            node.Leaf = leafNode;
                            //if (colorNode == true)
                            //{
                            //    node.Icon = Icon.Tick;
                            //}
                            //else
                            //{
                            //    node.Icon = Icon.FolderGo;
                            //}
                            e.Nodes.Add(node);
                        }
                    }
                }
            }
        }

        protected void deReadThresholds(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                if (e.Parameters["AmountId"] != null)
                {
                    decimal AmountId = decimal.Parse(e.Parameters["AmountId"]);
                    List<CUSTOMER_SURVEY_THRESHOLDS> Threshold = CUSTOMER_SURVEYS.GetThresholdPercentages(AmountId, _context).Include(x => x.CUSTOMER_SURVEY_THRESH_AMT).ToList();
                    uxThresholdStore.DataSource = Threshold;
                }
            }
        }

        protected void deSaveThreshold(object sender, DirectEventArgs e)
        {
            ChangeRecords<CUSTOMER_SURVEY_THRESHOLDS> data = new StoreDataHandler(e.ExtraParams["data"]).BatchObjectData<CUSTOMER_SURVEY_THRESHOLDS>();

            CUSTOMER_SURVEY_THRESHOLDS Threshold;

            foreach (CUSTOMER_SURVEY_THRESHOLDS item in data.Created)
            {
                Threshold = new CUSTOMER_SURVEY_THRESHOLDS();
                Threshold.THRESHOLD = item.THRESHOLD;
                Threshold.AMOUNT_ID = decimal.Parse(e.ExtraParams["amountId"]);
                Threshold.CREATE_DATE = DateTime.Now;
                Threshold.MODIFY_DATE = DateTime.Now;
                Threshold.CREATED_BY = User.Identity.Name;
                Threshold.MODIFIED_BY = User.Identity.Name;

                GenericData.Insert<CUSTOMER_SURVEY_THRESHOLDS>(Threshold);
            }

            foreach (CUSTOMER_SURVEY_THRESHOLDS item in data.Updated)
            {
                using (Entities _context = new Entities())
                {
                    Threshold = CUSTOMER_SURVEYS.GetThreshold(item.THRESHOLD_ID, _context);
                    Threshold.THRESHOLD = item.THRESHOLD;
                    Threshold.MODIFIED_BY = User.Identity.Name;
                    Threshold.MODIFY_DATE = DateTime.Now;
                }
                GenericData.Update<CUSTOMER_SURVEY_THRESHOLDS>(Threshold);
            }

            uxCompanySelectionModel.SetLocked(false);
            uxThresholdStore.Reload();
            uxThresholdForm.Reset();
            X.Js.Call("checkEditing");
            //dmSubtractFromDirty();
        }

        protected void deSaveDollar(object sender, DirectEventArgs e)
        {
            ChangeRecords<CUSTOMER_SURVEY_THRESH_AMT> data = new StoreDataHandler(e.ExtraParams["data"]).BatchObjectData<CUSTOMER_SURVEY_THRESH_AMT>();
            string _selectedRecordID = uxCompanySelectionModel.SelectedRecordID;
            long OrgId = GetOrgFromTree(_selectedRecordID);
            CUSTOMER_SURVEY_THRESH_AMT Dollars;
            foreach (CUSTOMER_SURVEY_THRESH_AMT item in data.Created)
            {
                Dollars = new CUSTOMER_SURVEY_THRESH_AMT();
                Dollars.TYPE_ID = item.TYPE_ID;
                Dollars.LOW_DOLLAR_AMT = item.LOW_DOLLAR_AMT;
                Dollars.HIGH_DOLLAR_AMT = item.HIGH_DOLLAR_AMT;
                Dollars.ORG_ID = OrgId;
                Dollars.CREATED_BY = User.Identity.Name;
                Dollars.MODIFIED_BY = User.Identity.Name;
                Dollars.CREATE_DATE = DateTime.Now;
                Dollars.MODIFY_DATE = DateTime.Now;

                GenericData.Insert<CUSTOMER_SURVEY_THRESH_AMT>(Dollars);

                string ORG_HIER;
                string TYPE_NAME;
                using (Entities _context = new Entities())
                {
                    ORG_HIER = _context.ORG_HIER_V.Where(x => x.ORG_ID == Dollars.ORG_ID).Select(x => x.ORG_HIER).Distinct().Single();
                    TYPE_NAME = _context.SURVEY_TYPES.Where(x => x.TYPE_ID == Dollars.TYPE_ID).Select(x => x.TYPE_NAME).Single();
                }
                ModelProxy Record = uxDollarStore.GetByInternalId(item.PhantomId);
                Record.CreateVariable = true;
                Record.SetId(Dollars.AMOUNT_ID);
                Record.Set("ORG_HIER", ORG_HIER);
                Record.Set("TYPE_NAME", TYPE_NAME);
                Record.Commit();
            }

            foreach (CUSTOMER_SURVEY_THRESH_AMT item in data.Updated)
            {
                using (Entities _context = new Entities())
                {
                    Dollars = CUSTOMER_SURVEYS.GetOrganizationThresholdAmount(item.AMOUNT_ID, _context);
                    Dollars.LOW_DOLLAR_AMT = item.LOW_DOLLAR_AMT;
                    Dollars.HIGH_DOLLAR_AMT = item.HIGH_DOLLAR_AMT;
                    Dollars.MODIFY_DATE = DateTime.Now;
                    Dollars.MODIFIED_BY = User.Identity.Name;
                    Dollars.TYPE_ID = item.TYPE_ID;
                }
                GenericData.Update<CUSTOMER_SURVEY_THRESH_AMT>(Dollars);
            }
            //dmSubtractFromDirty();
            uxDollarGrid.GetView().Refresh();
            uxCompanySelectionModel.SetLocked(false);
            uxDollarSelection.SetLocked(false);
            X.Js.Call("checkEditing");
            uxThresholdStore.Reload();
        }

        protected void deDeleteDollar(object sender, DirectEventArgs e)
        {
            decimal AmountId = decimal.Parse(e.ExtraParams["AmountId"]);
            List<CUSTOMER_SURVEY_THRESHOLDS> ThresholdCheck;
            CUSTOMER_SURVEY_THRESH_AMT ToBeDeleted;
            using (Entities _context = new Entities())
            {
                ThresholdCheck = CUSTOMER_SURVEYS.GetThresholdPercentages(AmountId, _context).ToList();
                ToBeDeleted = CUSTOMER_SURVEYS.GetOrganizationThresholdAmount(AmountId, _context);
            }

            if (ThresholdCheck.Count > 0)
            {
                X.Msg.Alert("Contains Percentages", "Please delete all percentage thresholds before deleting the dollar range").Show();
            }
            else
            {
                GenericData.Delete<CUSTOMER_SURVEY_THRESH_AMT>(ToBeDeleted);
                uxDollarStore.Reload();
                uxDeleteDollarButton.Disable();
            }


        }

        protected void deDeleteThreshold(object sender, DirectEventArgs e)
        {
            decimal ThresholdId = decimal.Parse(e.ExtraParams["ThresholdId"]);
            CUSTOMER_SURVEY_THRESHOLDS ToBeDeleted;
            using (Entities _context = new Entities())
            {
                ToBeDeleted = CUSTOMER_SURVEYS.GetThreshold(ThresholdId, _context);
            }

            GenericData.Delete<CUSTOMER_SURVEY_THRESHOLDS>(ToBeDeleted);
            uxThresholdStore.Reload();
            uxDeleteThresholdButton.Disable();
        }

        protected void deLoadThresholdForm(object sender, DirectEventArgs e)
        {
            uxThresholdFormType.Value = "Edit";
            List<CUSTOMER_SURVEY_THRESHOLDS> Threshold = JSON.Deserialize<List<CUSTOMER_SURVEY_THRESHOLDS>>(e.ExtraParams["RowValues"]);
            foreach (CUSTOMER_SURVEY_THRESHOLDS ThresholdToEdit in Threshold)
            {
                uxThreshold.Value = ThresholdToEdit.THRESHOLD;
                uxThresholdId.Value = ThresholdToEdit.THRESHOLD_ID;
                uxAddEditThresholdWindow.Show();
            }
        }

        protected void deLoadDollarStore(object sender, DirectEventArgs e)
        {
            if (long.Parse(Session["isDirty"].ToString()) == 0)
            {
                uxDollarStore.Reload();
                uxAddDollarButton.Enable();
                uxDeleteDollarButton.Disable();
                uxDeleteThresholdButton.Disable();
                uxThresholdStore.RemoveAll();
            }
        }

        //[DirectMethod]
        //public void dmAddToDirty()
        //{
        //    long isDirty = (long)Session["isDirty"];
        //    isDirty++;
        //    Session["isDirty"] = isDirty;
        //}

        //[DirectMethod]
        //public void dmSubtractFromDirty()
        //{
        //    long isDirty = (long)Session["isDirty"];
        //    isDirty--;
        //    Session["isDirty"] = isDirty;
        //}
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using DBI.Data;
using DBI.Core.Web;

namespace DBI.Web.EMS.Views.Modules.BudgetBidding
{
    public partial class umBudgetBiddingMain : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (!validateComponentSecurity("SYS.BudgetBidding.View"))
                {
                    X.Redirect("~/Views/uxDefault.aspx");
                }

                BB.CleanOldTempRecords(2);
            }
        }

        protected void deLoadOrgTree(object sender, NodeLoadEventArgs e)
        {
            // Add LE
            if (e.NodeID == "0")
            {
                var allowedOrgs = BB.UserAllowedOrgs(SYS_USER_INFORMATION.UserID(User.Identity.Name));
                using (Entities context = new Entities())
                {
                    SYS_PROFILE_OPTIONS profileNumber = SYS_PROFILE_OPTIONS.ProfileOption("OverheadBudgetHierarchy");
                    var profileLEs = context.SYS_ORG_PROFILE_OPTIONS.Where(x => x.PROFILE_OPTION_ID == profileNumber.PROFILE_OPTION_ID);
                    foreach (var le in profileLEs)
                    {
                        string leName = HR.LegalEntityHierarchies().Where(x => x.ORGANIZATION_ID == le.ORGANIZATION_ID).Select(x => x.ORGANIZATION_NAME).FirstOrDefault();
                        var nextData = HR.ActiveOrganizationsByHierarchy(Convert.ToInt64(le.PROFILE_VALUE), le.ORGANIZATION_ID);
                        foreach (long allowedOrg in allowedOrgs)
                        {
                            if (nextData.Select(x => x.ORGANIZATION_ID).Contains(allowedOrg))
                            {
                                Node node = new Node();
                                node.NodeID = string.Format("{0}:{1}:{2}", le.PROFILE_VALUE, le.ORGANIZATION_ID.ToString(), le.ORGANIZATION_ID.ToString());
                                node.Text = leName;
                                if (BB.IsRollup(le.ORGANIZATION_ID) == true)
                                {
                                    node.Icon = Icon.Building;
                                }
                                e.Nodes.Add(node);
                                break;
                            }
                        }
                    }
                }
            }

            // Add orgs
            else
            {
                char[] delimChars = { ':' };
                string[] selID = e.NodeID.Split(delimChars);
                long hierarchyID = long.Parse(selID[0].ToString());
                long leOrgID = long.Parse(selID[1].ToString());
                long orgID = long.Parse(selID[2].ToString());
                var data = HR.ActiveOrganizationsByHierarchy(hierarchyID, orgID);
                var orgsList = BB.UserAllowedOrgs(SYS_USER_INFORMATION.UserID(User.Identity.Name));
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
                        if (BB.IsUserOrgAndAllowed(SYS_USER_INFORMATION.UserID(User.Identity.Name), view.ORGANIZATION_ID) == true)
                        {
                            addNode = true;
                            colorNode = true;
                        }

                        // In next org?
                        foreach (long allowedOrgs in orgsList)
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
                            node.NodeID = string.Format("{0}:{1}:{2}", hierarchyID.ToString(), leOrgID, view.ORGANIZATION_ID.ToString());
                            node.Text = view.ORGANIZATION_NAME;
                            node.Leaf = leafNode;
                            if (colorNode == true)
                            {
                                if (BB.IsRollup(view.ORGANIZATION_ID) == true)
                                {
                                    node.Icon = Icon.PackageGreen;
                                }
                                else
                                {
                                    node.Icon = Icon.Accept;
                                }
                            }
                            else
                            {
                                if (BB.IsRollup(view.ORGANIZATION_ID) == true)
                                {
                                    node.Icon = Icon.PackageGreen;
                                }
                                else
                                {
                                    node.Icon = Icon.TagsGrey;
                                }
                            }
                            e.Nodes.Add(node);
                        }
                    }
                }
            }
        }

        protected void deLoadFiscalYears(object sender, StoreReadDataEventArgs e)
        {
            uxFiscalYearStore.DataSource = BB.FiscalYears();
        }

        protected void deLoadBudgetVersions(object sender, StoreReadDataEventArgs e)
        {
            uxVersionStore.DataSource = BB.BudgetVersions();
        }

        protected void deSelectOrg(object sender, DirectEventArgs e)
        {
            bool prevBlank = PreviouslyBlankBudget();
            string nodeID = uxOrgPanel.SelectedNodes[0].NodeID;
            char[] delimChars = { ':' };
            string[] selID = nodeID.Split(delimChars);

            if (nodeID == "0")
            {
                uxHidOrgOK.Text = "";
                uxYearVersionTitle.Text = "";
                DisableToolbar();
                LoadBudget(prevBlank);
                return;
            }

            long orgID = long.Parse(selID[2].ToString());

            if (BB.IsRollup(orgID) == true || BB.IsUserOrgAndAllowed(SYS_USER_INFORMATION.UserID(User.Identity.Name), orgID) == true)
            {
                uxHidOrgOK.Text = "Y";
                uxYearVersionTitle.Text = uxOrgPanel.SelectedNodes[0].Text;
                EnableToolbar();
                LoadBudget(prevBlank);
            }
            else
            {
                uxHidOrgOK.Text = "";
                uxYearVersionTitle.Text = "";
                DisableToolbar();
                LoadBudget(prevBlank);
            }
        }

        protected void deSelectYear(object sender, DirectEventArgs e)
        {
            bool prevBlank = PreviouslyBlankBudget();

            uxHidYearOK.Text = "Y";
            LoadBudget(prevBlank);
        }

        protected void deSelectVersion(object sender, DirectEventArgs e)
        {
            bool prevBlank = PreviouslyBlankBudget();

            uxHidVerOK.Text = "Y";
            LoadBudget(prevBlank);
        }

        protected bool PreviouslyBlankBudget()
        {
            string orgOK = uxHidOrgOK.Text;
            string yearOK = uxHidYearOK.Text;
            string verOK = uxHidVerOK.Text;
            bool prevBlank = (orgOK == "" || yearOK == "" || verOK == "") ? true : false;

            return prevBlank;
        }

        protected void LoadBudget(bool prevBlank)
        {
            string orgOK = uxHidOrgOK.Text;
            string yearOK = uxHidYearOK.Text;
            string verOK = uxHidVerOK.Text;
            string url = "umBlankBudget.aspx";

            if (orgOK == "Y" && yearOK == "Y" && verOK == "Y")
            {
                string nodeID = uxOrgPanel.SelectedNodes[0].NodeID;
                char[] delimChars = { ':' };
                string[] selID = nodeID.Split(delimChars);
                string hierarchyID = selID[0].ToString();
                long leOrgID = Convert.ToInt64(selID[1].ToString());
                long orgID = Convert.ToInt64(selID[2].ToString());

                string orgName = HttpUtility.UrlEncode(uxOrgPanel.SelectedNodes[0].Text);
                long fiscalYear = Convert.ToInt64(uxFiscalYear.SelectedItem.Value);
                long verID = Convert.ToInt64(uxVersion.SelectedItem.Value);
                string verName = HttpUtility.UrlEncode(uxVersion.SelectedItem.Text);


                if (BB.IsRollup(orgID) == false)
                {
                    //FIX   
                    if (orgID == 1210)
                    {
                        url = "umMonthBudget.aspx?hierID=" + hierarchyID + "&leOrgID=" + leOrgID + "&orgID=" + orgID + "&orgName=" + orgName + "&fiscalYear=" + fiscalYear + "&verID=" + verID + "&verName=" + verName;
                    }
                    else
                    {
                        url = "umYearBudget.aspx?hierID=" + hierarchyID + "&leOrgID=" + leOrgID + "&orgID=" + orgID + "&orgName=" + orgName + "&fiscalYear=" + fiscalYear + "&verID=" + verID + "&verName=" + verName;

                    }
                    //FIX   
                }
                else
                {
                    url = "umYearRollupSummary.aspx?hierID=" + hierarchyID + "&leOrgID=" + leOrgID + "&orgID=" + orgID + "&orgName=" + orgName + "&fiscalYear=" + fiscalYear + "&verID=" + verID + "&verName=" + verName;
                }
            }

            else if (prevBlank == true)
            {
                return;
            }

            uxBudgetPanel.Loader.SuspendScripting();
            uxBudgetPanel.Loader.Url = url;
            uxBudgetPanel.Loader.Mode = LoadMode.Frame;
            uxBudgetPanel.Loader.DisableCaching = true;
            uxBudgetPanel.LoadContent();
        }

        protected void EnableToolbar()
        {
            uxFiscalYear.Enable();
            uxVersion.Enable();
            uxOrgSettings.Disable();
        }

        protected void DisableToolbar()
        {
            uxFiscalYear.Disable();
            uxVersion.Disable();
            uxOrgSettings.Disable();
        }
    }
}
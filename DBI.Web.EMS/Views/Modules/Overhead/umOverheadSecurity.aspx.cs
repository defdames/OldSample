using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DBI.Core.Web;
using Ext.Net;
using DBI.Data;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public partial class umOverheadSecurity : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (!validateComponentSecurity("SYS.OverheadBudget.Security"))
                {
                    X.Redirect("~/Views/uxDefault.aspx");
                }
            }
        }

        /// <summary>
        /// Displayes all the legal entities that are in oracle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deLoadLegalEntities(object sender, NodeLoadEventArgs e)
        {
            //Load LEs
            if (e.NodeID == "0")
            {

                var data = HR.LegalEntityHierarchies().Select(a => new { a.ORGANIZATION_ID, a.ORGANIZATION_NAME }).Distinct().ToList();

            
                //Build the treepanel
                foreach (var view in data)
                {

                    //Only show for the profile option OverheadBudgetHierarchy

                    string _profileValue = SYS_ORG_PROFILE_OPTIONS.OrganizationProfileOption("OverheadBudgetHierarchy", view.ORGANIZATION_ID);

                    if (_profileValue.Length > 0)
                    {
                        long _profileLong = long.Parse(_profileValue);
                        var _hierData = HR.LegalEntityHierarchies().Where(a => a.ORGANIZATION_ID == view.ORGANIZATION_ID & a.ORGANIZATION_STRUCTURE_ID == _profileLong).ToList();

                        //Check for incomplete setup 
                        int _cnt = OVERHEAD_BUDGET_TYPE.BudgetTypes(view.ORGANIZATION_ID).Count();


                        //Create the Hierarchy Levels
                        Node node = new Node();
                        node.Text = view.ORGANIZATION_NAME;
                        node.NodeID = string.Format("{0}:{1}", view.ORGANIZATION_ID.ToString(), _profileValue.ToString());
                        if (_cnt > 0)
                        {
                            node.Icon = Icon.BulletGreen;
                        }
                        else
                        {
                            node.Icon = Icon.BulletRed;                  
                        }
                        node.Leaf = true;
                        e.Nodes.Add(node);
                    }     
                }
            }
            //else
            //{
            //    long nodeID = long.Parse(e.NodeID);

            //    //Load Hierarchies for LE
            //    var data = HR.LegalEntityHierarchies().Where(a => a.ORGANIZATION_ID == nodeID).ToList();

            //    //Build the treepanel
            //    foreach (var view in data)
            //    {
            //        //Create the Hierarchy Levels
            //        Node node = new Node();
            //        node.Icon = Icon.BulletMagnify;
            //        node.Text = view.HIERARCHY_NAME;
            //        node.NodeID = string.Format("{0}:{1}", view.ORGANIZATION_ID.ToString(), view.ORGANIZATION_STRUCTURE_ID.ToString());
            //        node.Leaf = true;
            //        e.Nodes.Add(node);
            //    }
            //}

        }

        protected void deSelectNode(object sender, DirectEventArgs e)
        {
            TreeSelectionModel sm = uxOrganizationTreePanel.GetSelectionModel() as TreeSelectionModel;
            string _nodeText = e.ExtraParams["ORGANIZATION_NAME"];

                uxCenterTabPanel.RemoveAll();
                AddTab(sm.SelectedRecordID + "OF", _nodeText + " - Budget Versions", "umOverheadMaintainBudgets.aspx?orgid=" + sm.SelectedRecordID, false, true);
                AddTab(sm.SelectedRecordID + "OS", _nodeText + " - Organization Security", "umOverheadOrganizationSecurity.aspx?orgid=" + sm.SelectedRecordID, false, false);
                AddTab(sm.SelectedRecordID + "BT", _nodeText + " - Budget Types", "umOverheadBudgetTypes.aspx?leid=" + sm.SelectedRecordID, false, false);
                uxOrganizationTreePanel.Collapse();

        }

        /// <summary>
        /// Adds a tab to the tabpanel
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="url"></param>
        /// <param name="loadContent"></param>
        /// <param name="setActive"></param>
        protected void AddTab(string id, string title, string url, Boolean loadContent = false, Boolean setActive = false)
        {

            Ext.Net.Panel pan = new Ext.Net.Panel();

            pan.ID = "Tab" + id.Replace(":","_");
            pan.Title = title;
            pan.CloseAction = CloseAction.Destroy;
            pan.Loader = new ComponentLoader();
            pan.Loader.ID = "loader" + id.Replace(":", "_");
            pan.Loader.Url = url;
            pan.Loader.Mode = LoadMode.Frame;
            pan.Loader.LoadMask.ShowMask = true;
            pan.Loader.DisableCaching = true;
            pan.AddTo(uxCenterTabPanel);

            if(setActive)
                uxCenterTabPanel.SetActiveTab("Tab" + id.Replace(":", "_"));

            if (loadContent)
                pan.LoadContent();
        }

        public class ACTUAL_BALANCES
        {
            public decimal PERIOD_NET_DR { get; set; }
            public decimal PERIOD_NET_CR { get; set; }
            public short PERIOD_YEAR { get; set; }
            public long CODE_COMBINATION_ID { get; set; }
            public long PERIOD_NUM { get; set; }
        }


        [DirectMethod]
        public void AddTabPanel(string id, string title, string url)
        {

            Ext.Net.Panel pan = new Ext.Net.Panel();

            pan.ID = "Tab" + id.Replace(":", "_");
            pan.Title = title;
            pan.CloseAction = CloseAction.Destroy;
            pan.Closable = true;
            pan.Loader = new ComponentLoader();
            pan.Loader.ID = "loader" + id.Replace(":", "_");
            pan.Loader.Url = url;
            pan.Loader.Mode = LoadMode.Frame;
            pan.Loader.LoadMask.ShowMask = true;
            pan.Loader.DisableCaching = true;
            pan.AddTo(uxCenterTabPanel);
            
            uxCenterTabPanel.SetActiveTab("Tab" + id.Replace(":", "_"));
        }

        protected void deImportActuals(object sender, DirectEventArgs e)
        {

        }

        protected void uxBussinessUnitStore_ReadData(object sender, StoreReadDataEventArgs e)
        {
            var data = HR.LegalEntityHierarchies().Select(a => new DBI.Data.Generic.DoubleComboLongID { ID = a.ORGANIZATION_ID, ID_NAME = a.ORGANIZATION_NAME }).Distinct().ToList();
            uxBussinessUnitStore.DataSource = data;
        }

        protected void uxFiscalYearsStore_ReadData(object sender, StoreReadDataEventArgs e)
        {
            uxFiscalYearsStore.DataSource = PA.FiscalYearsGL().OrderByDescending(x => x.ID_NAME);
        }

        public class GL_PERIODS
        {
            public string ENTERED_PERIOD_NAME { get; set; }
            public short PERIOD_YEAR { get; set; }
            public long PERIOD_NUM { get; set; }
            public string PERIOD_TYPE { get; set; }
            public DateTime START_DATE { get; set; }
            public DateTime? END_DATE { get; set; }
            public string ACTUALS_IMPORTED_FLAG { get; set; }
            public string ADMIN { get; set; }
        }

        protected void uxDetailStore_ReadData(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                short _fiscal_year = short.Parse(uxFiscalYear.SelectedItem.Value);

                string sql2 = "select entered_period_name,period_year,period_num,period_type,start_date,end_date,'N' as ACTUALS_IMPORTED_FLAG from gl.gl_periods where period_set_name = 'DBI Calendar' order by period_num";
                List<GL_PERIODS> _periodMonthList = _context.Database.SqlQuery<GL_PERIODS>(sql2).Where(x => x.PERIOD_YEAR == _fiscal_year & x.PERIOD_TYPE == "Month").ToList();

                int count;
                uxDetailStore.DataSource = GenericData.EnumerableFilterHeader<GL_PERIODS>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _periodMonthList, out count);
                e.Total = count;
            }
        }

        protected void deOverheadImportActuals(object sender, DirectEventArgs e)
        {
            short _fiscal_year = short.Parse(uxFiscalYear.SelectedItem.Value);
            long _organizationID = long.Parse(uxBussinessUnit.SelectedItem.Value);

            using (Entities _context = new Entities())
            {

                CheckboxSelectionModel _csm = uxPeriodSelectionModel;

                List<string> _periodsToImport = _csm.SelectedRows.Select(x => x.RecordID).ToList();

                //First get the periods for the fiscal year
                string sql2 = "select entered_period_name,period_year,period_num,period_type,start_date,end_date from gl.gl_periods where period_set_name = 'DBI Calendar' order by period_num";
                List<GL_PERIODS> _periodMonthList = _context.Database.SqlQuery<GL_PERIODS>(sql2).Where(x => x.PERIOD_YEAR == _fiscal_year & x.PERIOD_TYPE == "Month" & _periodsToImport.Contains(x.PERIOD_NUM.ToString())).ToList();

                // Get the range of accounts
                List<GL_ACCOUNTS_V> _rangeOfAccounts = new List<GL_ACCOUNTS_V>();


                //Import for all active organizations

                //Get Hierarchy ID for the selected Business Unit
                long _profileValue = long.Parse(SYS_ORG_PROFILE_OPTIONS.OrganizationProfileOption("OverheadBudgetHierarchy", _organizationID));
                var _activeOrganizations = HR.OverheadOrganizationStatusByHierarchy(_profileValue, _organizationID).Where(x => x.ORGANIZATION_STATUS == "Active").ToList(); // Active Organizations

                foreach (HR.ORGANIZATION_V1 _org in _activeOrganizations)
                {
                    var _data = _context.OVERHEAD_GL_RANGE.Where(x => x.ORGANIZATION_ID == _org.ORGANIZATION_ID);

                    foreach (OVERHEAD_GL_RANGE _range in _data)
                    {
                        var _adata = _context.GL_ACCOUNTS_V.Where(x => String.Compare(x.SEGMENT1, _range.SRSEGMENT1) >= 0 && String.Compare(x.SEGMENT1, _range.ERSEGMENT1) <= 0);
                        _adata = _adata.Where(x => String.Compare(x.SEGMENT2, _range.SRSEGMENT2) >= 0 && String.Compare(x.SEGMENT2, _range.ERSEGMENT2) <= 0);
                        _adata = _adata.Where(x => String.Compare(x.SEGMENT3, _range.SRSEGMENT3) >= 0 && String.Compare(x.SEGMENT3, _range.ERSEGMENT3) <= 0);
                        _adata = _adata.Where(x => String.Compare(x.SEGMENT4, _range.SRSEGMENT4) >= 0 && String.Compare(x.SEGMENT4, _range.ERSEGMENT4) <= 0);
                        _adata = _adata.Where(x => String.Compare(x.SEGMENT5, _range.SRSEGMENT5) >= 0 && String.Compare(x.SEGMENT5, _range.ERSEGMENT5) <= 0);
                        _adata = _adata.Where(x => String.Compare(x.SEGMENT6, _range.SRSEGMENT6) >= 0 && String.Compare(x.SEGMENT6, _range.ERSEGMENT6) <= 0);
                        _adata = _adata.Where(x => String.Compare(x.SEGMENT7, _range.SRSEGMENT7) >= 0 && String.Compare(x.SEGMENT7, _range.ERSEGMENT7) <= 0);
                        List<GL_ACCOUNTS_V> _accountRange = _adata.ToList();
                        _rangeOfAccounts.AddRange(_accountRange);

                    }

                    //Exclude any accounts added to the list of excluded accounts
                    List<OVERHEAD_GL_ACCOUNT> _excludedAccounts = _context.OVERHEAD_GL_ACCOUNT.Where(x => x.ORGANIZATION_ID == _org.ORGANIZATION_ID ).ToList();

                    //Create a list of accounts matching up with GL_ACCOUNTS_V
                    List<GL_ACCOUNTS_V> _eAccountList = new List<GL_ACCOUNTS_V>();

                    foreach (OVERHEAD_GL_ACCOUNT _eaccount in _excludedAccounts)
                    {
                        var _adata = _context.GL_ACCOUNTS_V.Where(x => x.CODE_COMBINATION_ID == _eaccount.CODE_COMBINATION_ID).Single();
                        _rangeOfAccounts.Remove(_adata);
                    }


                    //We need to get the list of overhead budget headers
                    List<OVERHEAD_ORG_BUDGETS> _budgetHeaderList = _context.OVERHEAD_ORG_BUDGETS.Where(x => x.FISCAL_YEAR == _fiscal_year && x.ORGANIZATION_ID == _org.ORGANIZATION_ID).ToList();


                    //Add total detail
                    foreach (GL_ACCOUNTS_V _validAccount in _rangeOfAccounts)
                    {

                        string sql = string.Format("select period_net_dr, period_net_cr,period_year,code_combination_id,period_num from gl.gl_balances where actual_flag = 'A' and period_year = {0} and code_combination_id = {1} and set_of_books_id in (select distinct set_of_books_id from apps.hr_operating_units)", _fiscal_year, _validAccount.CODE_COMBINATION_ID);
                        List<ACTUAL_BALANCES> _balance = _context.Database.SqlQuery<ACTUAL_BALANCES>(sql).ToList();

                        foreach (GL_PERIODS _period in _periodMonthList)
                        {

                            foreach (OVERHEAD_ORG_BUDGETS _budgetHeader in _budgetHeaderList)
                            {

                                List<OVERHEAD_BUDGET_DETAIL> _budgetLineList = _context.OVERHEAD_BUDGET_DETAIL.Where(x => x.ORG_BUDGET_ID == _budgetHeader.ORG_BUDGET_ID).ToList();
                                OVERHEAD_BUDGET_DETAIL _line = _budgetLineList.Where(x => x.ORG_BUDGET_ID == _budgetHeader.ORG_BUDGET_ID & x.CODE_COMBINATION_ID == _validAccount.CODE_COMBINATION_ID & x.PERIOD_NUM == _period.PERIOD_NUM).SingleOrDefault();
                                ACTUAL_BALANCES _actualTotalLine = _balance.Where(x => x.PERIOD_NUM == _period.PERIOD_NUM).SingleOrDefault();
                                decimal _aTotal = 0;

                                if (_actualTotalLine != null)
                                {
                                    _aTotal = _actualTotalLine.PERIOD_NET_DR + Decimal.Negate(_actualTotalLine.PERIOD_NET_CR);
                                }
                                else
                                {
                                    _aTotal = 0;
                                }

                                if (_line == null)
                                {
                                    //No data, create it
                                    OVERHEAD_BUDGET_DETAIL _record = new OVERHEAD_BUDGET_DETAIL();
                                    _record.ORG_BUDGET_ID = _budgetHeader.ORG_BUDGET_ID;
                                    _record.PERIOD_NAME = _period.ENTERED_PERIOD_NAME;
                                    _record.PERIOD_NUM = _period.PERIOD_NUM;
                                    _record.CODE_COMBINATION_ID = _validAccount.CODE_COMBINATION_ID;
                                    _record.AMOUNT = _aTotal;
                                    _record.CREATE_DATE = DateTime.Now;
                                    _record.MODIFY_DATE = DateTime.Now;
                                    _record.CREATED_BY = User.Identity.Name;
                                    _record.MODIFIED_BY = User.Identity.Name;
                                    _record.ACTUALS_IMPORTED_FLAG = "Y";
                                    GenericData.Insert<OVERHEAD_BUDGET_DETAIL>(_record);
                                }
                                else
                                {
                                    //Data update it
                                    _line.AMOUNT = _aTotal;
                                    _line.MODIFY_DATE = DateTime.Now;
                                    _line.MODIFIED_BY = User.Identity.Name;
                                    _line.ACTUALS_IMPORTED_FLAG = "Y";
                                    GenericData.Update<OVERHEAD_BUDGET_DETAIL>(_line);
                                }
                            }
                        }
                    }


                }


                


            }
        }


    }    
}
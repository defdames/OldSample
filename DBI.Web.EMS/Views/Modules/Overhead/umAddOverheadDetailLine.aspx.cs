using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DBI.Data;
using Ext.Net;
using DBI.Core.Web;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public partial class umAddOverheadDetailLine : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (!validateComponentSecurity("SYS.OverheadBudget.Maintenance"))
                {
                    X.Redirect("~/Views/uxDefault.aspx");
                }

            }
        }

        protected void deLoadDetailLinesStore(object sender, StoreReadDataEventArgs e)
        {

            try
            {
                long _budget_id = long.Parse(Request.QueryString["budgetID"]);
                long _account_id = long.Parse(Request.QueryString["accountID"]);
                short _fiscal_year = short.Parse(Request.QueryString["fiscalyear"]);

                using (Entities _context = new Entities())
                {

                    List<OVERHEAD_BUDGET_DETAIL> _detail = _context.OVERHEAD_BUDGET_DETAIL.Where(x => x.ORG_BUDGET_ID == _budget_id & x.CODE_COMBINATION_ID == _account_id).OrderBy(x => x.PERIOD_NUM).ToList();

                    int _periodListCount = 12;

                    if (_detail.Count() < 12)
                    {

                        //get a distinct list of periods already created 
                        List<long?> _periodsCreated = _detail.Select(x => x.PERIOD_NUM).ToList();
                        List<string> _periodString = _periodsCreated.ConvertAll<string>(x => x.ToString());

                        //We need to create the budget lines in the system for each period.
                        string sql = "select entered_period_name,period_year,period_num,period_type,start_date,end_date from gl.gl_periods where period_set_name = 'DBI Calendar' order by period_num";
                        List<GL_PERIODS> _periodList = _context.Database.SqlQuery<GL_PERIODS>(sql).Where(x => x.PERIOD_YEAR == _fiscal_year & x.PERIOD_TYPE == "Month" & (!_periodString.Contains(x.PERIOD_NUM.ToString()))).ToList();

                        foreach (GL_PERIODS _period in _periodList)
                        {
                            OVERHEAD_BUDGET_DETAIL _record = new OVERHEAD_BUDGET_DETAIL();
                            _record.ORG_BUDGET_ID = _budget_id;
                            _record.PERIOD_NAME = _period.ENTERED_PERIOD_NAME;
                            _record.PERIOD_NUM = _period.PERIOD_NUM;
                            _record.CODE_COMBINATION_ID = _account_id;
                            _record.AMOUNT = 0;
                            _record.CREATE_DATE = DateTime.Now;
                            _record.MODIFY_DATE = DateTime.Now;
                            _record.CREATED_BY = User.Identity.Name;
                            _record.MODIFIED_BY = User.Identity.Name;
                            _record.ACTUALS_IMPORTED_FLAG = "N";
                            GenericData.Insert<OVERHEAD_BUDGET_DETAIL>(_record);
                        }

                        _detail = _context.OVERHEAD_BUDGET_DETAIL.Where(x => x.ORG_BUDGET_ID == _budget_id & x.CODE_COMBINATION_ID == _account_id).OrderBy(x => x.PERIOD_NUM).ToList();
                    }

                    if (!(e.Parameters["DISPERSE_AMOUNT"] == null))
                    {

                    //Count unlocked months
                        _periodListCount = _detail.Where(x => x.ACTUALS_IMPORTED_FLAG == "N").Count();

                    foreach (OVERHEAD_BUDGET_DETAIL _item in _detail)
                    {
                            if (_item.ACTUALS_IMPORTED_FLAG == "N")
                            {
                                string _type = e.Parameters["DISPERSE_TYPE"];
                                string sql = "select entered_period_name,period_year,period_num,period_type,start_date,end_date from gl.gl_periods where period_set_name = 'DBI Calendar' order by period_num";
                                List<GL_PERIODS> _periodList = _context.Database.SqlQuery<GL_PERIODS>(sql).Where(x => x.PERIOD_YEAR == _fiscal_year & x.PERIOD_TYPE == "Week").ToList();

                                //Anually Disperse
                                if (_type == "A")
                                {
                                    decimal _total = decimal.Parse(e.Parameters["DISPERSE_AMOUNT"]);
                                    decimal _distAmount = (_total / _periodListCount);
                                    _item.AMOUNT = _distAmount;
                                }

                                //Monthly Disperse
                                if (_type == "M")
                                {
                                    decimal _total = decimal.Parse(e.Parameters["DISPERSE_AMOUNT"]);
                                    _item.AMOUNT = _total;
                                }

                                //Weekly Disperse
                                if (_type == "W")
                                {
                                    _periodList = _periodList.Where(x => x.ENTERED_PERIOD_NAME.Contains(_item.PERIOD_NAME)).ToList();
                                    int periodCount = _periodList.Count();

                                    if (periodCount == 0)
                                        throw new DBICustomException("There are no periods setup for this year by week, please contact finance to setup these periods. This function is disabled");

                                    decimal _total = decimal.Parse(e.Parameters["DISPERSE_AMOUNT"]);
                                    decimal _distAmount = ((_total) * periodCount);
                                    _item.AMOUNT = _distAmount;
                                }

                                //Anually by 445 Disperse
                                if (_type == "AW")
                                {
                                    int _periodsPerPeriod = _periodList.Count();

                                    _periodList = _periodList.Where(x => x.ENTERED_PERIOD_NAME.Contains(_item.PERIOD_NAME)).ToList();
                                    int periodCount = _periodList.Count();

                                    if (periodCount == 0)
                                        throw new DBICustomException("There are no periods setup for this year by week, please contact finance to setup these periods. This function is disabled");

                                    decimal _total = decimal.Parse(e.Parameters["DISPERSE_AMOUNT"]);
                                    decimal _distAmount = (_total / _periodsPerPeriod);
                                    _distAmount = ((_distAmount) * periodCount);
                                    _item.AMOUNT = _distAmount;
                                }
                            }

                        }

                    }


                    OVERHEAD_ACCOUNT_COMMENT _commentR = _context.OVERHEAD_ACCOUNT_COMMENT.Where(x => x.ORG_BUDGET_ID == _budget_id & x.CODE_COMBINATION_ID == _account_id).SingleOrDefault();
                    if (_commentR != null)
                        uxAccountComments.Text = _commentR.COMMENTS;

                    int count;
                    uxDetailStore.DataSource = GenericData.EnumerableFilterHeader<OVERHEAD_BUDGET_DETAIL>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _detail, out count);
                    e.Total = count;
                }

            }
            catch (Exception error)
            {
                X.Mask.Hide();

                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.INFO,
                    Title = "Error",
                    Message = error.Message
                });

                long _budget_id = long.Parse(Request.QueryString["budgetID"]);
                long _account_id = long.Parse(Request.QueryString["accountID"]);

                using (Entities _context = new Entities())
                {

                    List<OVERHEAD_BUDGET_DETAIL> _detail = _context.OVERHEAD_BUDGET_DETAIL.Where(x => x.ORG_BUDGET_ID == _budget_id & x.CODE_COMBINATION_ID == _account_id).OrderBy(x => x.PERIOD_NUM).ToList();

                    OVERHEAD_ACCOUNT_COMMENT _commentR = _context.OVERHEAD_ACCOUNT_COMMENT.Where(x => x.ORG_BUDGET_ID == _budget_id & x.CODE_COMBINATION_ID == _account_id).SingleOrDefault();
                    if (_commentR != null)
                        uxAccountComments.Text = _commentR.COMMENTS;

                    int count;
                    uxDetailStore.DataSource = GenericData.EnumerableFilterHeader<OVERHEAD_BUDGET_DETAIL>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _detail, out count);
                    e.Total = count;
                }
            }

        }

        protected void deCalcuateAmount(object sender, DirectEventArgs e)
        {

            if (uxAmountCalculator.Text == "")
            {
                X.Msg.Alert("Error", "Allocation amount is required!").Show();
            }
            else
            {

                Ext.Net.ParameterCollection ps = new Ext.Net.ParameterCollection();

                Ext.Net.StoreParameter _p = new Ext.Net.StoreParameter();
                _p.Mode = ParameterMode.Value;
                _p.Name = "DISPERSE_AMOUNT";
                _p.Value = uxAmountCalculator.Text;
                ps.Add(_p);

                Ext.Net.StoreParameter _p2 = new Ext.Net.StoreParameter();
                _p2.Mode = ParameterMode.Value;
                _p2.Name = "DISPERSE_TYPE";
                _p2.Value = uxDispersementType.SelectedItem.Value;
                ps.Add(_p2);

                uxDetailStore.Reload(ps);

                uxDisbursementDetailsWindow.Close();
            }
        }

        public class GL_PERIODS
        {
            public string ENTERED_PERIOD_NAME { get; set; }
            public short PERIOD_YEAR { get; set; }
            public long PERIOD_NUM { get; set; }
            public string PERIOD_TYPE { get; set; }
            public DateTime START_DATE { get; set; }
            public DateTime? END_DATE { get; set; }
        }

        protected void deSaveDetailLine(object sender, DirectEventArgs e)
        {
            string json = e.ExtraParams["Values"];

            List<OVERHEAD_BUDGET_DETAIL> _gridValues = JSON.Deserialize<List<OVERHEAD_BUDGET_DETAIL>>(json);

            foreach (OVERHEAD_BUDGET_DETAIL _detail in _gridValues)
            {
                _detail.MODIFIED_BY = User.Identity.Name;
                _detail.MODIFY_DATE = DateTime.Now;
            }

            GenericData.Update<OVERHEAD_BUDGET_DETAIL>(_gridValues);

            long _budget_id = long.Parse(Request.QueryString["budgetID"]);
            long _account_id = long.Parse(Request.QueryString["accountID"]);

            using (Entities _context = new Entities())
            {
                //Add comments if they were displayed

                OVERHEAD_ACCOUNT_COMMENT _record = _context.OVERHEAD_ACCOUNT_COMMENT.Where(x => x.ORG_BUDGET_ID == _budget_id & x.CODE_COMBINATION_ID == _account_id).SingleOrDefault();

                if (_record == null)
                {
                    OVERHEAD_ACCOUNT_COMMENT _newCommentRecord = new OVERHEAD_ACCOUNT_COMMENT();
                    _newCommentRecord.CODE_COMBINATION_ID = _account_id;
                    _newCommentRecord.ORG_BUDGET_ID = _budget_id;
                    _newCommentRecord.CREATE_DATE = DateTime.Now;
                    _newCommentRecord.CREATED_BY = User.Identity.Name;
                    _newCommentRecord.MODIFY_DATE = DateTime.Now;
                    _newCommentRecord.MODIFIED_BY = User.Identity.Name;
                    _newCommentRecord.COMMENTS = uxAccountComments.Text;
                    GenericData.Insert<OVERHEAD_ACCOUNT_COMMENT>(_newCommentRecord);
                }
                else
                {
                    OVERHEAD_ACCOUNT_COMMENT _newCommentRecord = _newCommentRecord = _context.OVERHEAD_ACCOUNT_COMMENT.Where(x => x.ORG_BUDGET_ID == _budget_id & x.CODE_COMBINATION_ID == _account_id).SingleOrDefault();
                    _newCommentRecord.COMMENTS = uxAccountComments.Text;
                    _newCommentRecord.MODIFY_DATE = DateTime.Now;
                    _newCommentRecord.MODIFIED_BY = User.Identity.Name;
                    GenericData.Update<OVERHEAD_ACCOUNT_COMMENT>(_newCommentRecord);
                }

            }

        }

    }
}
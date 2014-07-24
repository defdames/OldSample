using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Ext.Net;
using DBI.Data;
using DBI.Core.Web;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public partial class umOverheadGeneralLedger : BasePage
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

            protected void deAddGLRange(object sender, DirectEventArgs e)
            {

                string organizationID = Request.QueryString["orgID"];

                string url = "/Views/Modules/Overhead/umAddGlAccountRange.aspx?orgID=" + organizationID;
                Window win = new Window
                {
                    ID = "uxShowAccountRangeWindow",
                    Title = "General Ledger Account Range Filter",
                    Height = 700,
                    Width = 800,
                    Modal = true,
                    Resizable = true,
                    CloseAction = CloseAction.Destroy,
                    Loader = new ComponentLoader
                    {
                        Mode = LoadMode.Frame,
                        DisableCaching = true,
                        Url = url,
                        AutoLoad = true,
                        LoadMask =
                        {
                            ShowMask = true
                        }
                    }
                };

                win.Listeners.Close.Handler = "#{uxGLAccountRangeGridPanel}.getStore().load();";

                win.Render(this.Form);

                win.Show();
            }

            protected void deReadGLRange(object sender, StoreReadDataEventArgs e)
            {

                using (Entities _context = new Entities())
                {
                    long _organizationID = long.Parse(Request.QueryString["orgID"]);
                    int count;
                    IQueryable<OVERHEAD_GL_RANGE_V> _data = OVERHEAD.OverheadGLRangeByOrganizationId(_organizationID, _context);
                    uxGLAccountRangeStore.DataSource = GenericData.ListFilterHeader<OVERHEAD_GL_RANGE_V>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _data, out count);
                    e.Total = count;
                }
            }


            protected void deDeleteGLRange(object sender, DirectEventArgs e)
            {

                RowSelectionModel _rsm = uxGLAccountRangeSelectionModel;
                foreach (SelectedRow _row in _rsm.SelectedRows)
                {
                    OVERHEAD_GL_RANGE _record = OVERHEAD_GL_RANGE.OverheadRangeByID(long.Parse(_row.RecordID));
                    GenericData.Delete<OVERHEAD_GL_RANGE>(_record);
                }

                uxGLAccountRangeStore.Reload(true);
                _rsm.DeselectAll(true);
                uxDeleteGLRangeDelete.Disabled = true;

                uxGlAccountSecurityGridSelectionModel.DeselectAll();
                uxGlAccountSecurityStore.RemoveAll();

            }

        


            protected void deSelectRange(object sender, DirectEventArgs e)
            {
                if (uxGLAccountRangeSelectionModel.SelectedRows.Count() > 0)
                {
                    uxDeleteGLRangeDelete.Enable();
                }
                else
                {
                    uxDeleteGLRangeDelete.Disable();
                }

                Ext.Net.ParameterCollection ps = new Ext.Net.ParameterCollection();

                Ext.Net.StoreParameter _p = new Ext.Net.StoreParameter();
                _p.Mode = ParameterMode.Value;
                _p.Name = "INCLUDE_EXCLUDE";
                _p.Value = e.ExtraParams["INCLUDE_EXCLUDE"];
                ps.Add(_p);

                uxGlAccountSecurityStore.Reload(ps);
            }

            protected void deDeSelectRange(object sender, DirectEventArgs e)
            {
                uxDeleteGLRangeDelete.Disable();
                uxGLAccountRangeSelectionModel.DeselectAll(true);
                uxGlAccountSecurityStore.Reload(true);
            }

            public static string IsGLAccountExcluded(long organizationID, string seg1, string seg2, string seg3, string seg4, string seg5, string seg6, string seg7)
            {
                using(Entities _context = new Entities())
                {
                    List<OVERHEAD_GL_ACCOUNT> _acc = _context.OVERHEAD_GL_ACCOUNT.Where(a => a.ORGANIZATION_ID == organizationID && a.SEGMENT1 == seg1 && a.SEGMENT2 == seg2 && a.SEGMENT3 == seg3 && a.SEGMENT4 == seg4
                        && a.SEGMENT5 == seg5 && a.SEGMENT6 == seg6 && a.SEGMENT7 == seg7).ToList();
                  string _returnValue = "Included";
                    
                    if(_acc.Count() > 0)
                        _returnValue = "Excluded";

                    return _returnValue;

                }

            }


            protected void deReadAccountsForRanges(object sender, StoreReadDataEventArgs e)
            {

                RowSelectionModel _rsm = uxGLAccountRangeSelectionModel;
                
                using (Entities _context = new Entities())
                {

                        OVERHEAD_GL_RANGE _range = OVERHEAD_GL_RANGE.OverheadRangeByID(long.Parse(_rsm.SelectedRow.RecordID));
                        long _organizationID = long.Parse(Request.QueryString["orgID"]);
                        //Get a list of all accounts in the range and if it's been excluded show it as such.

                        int count;
                        var _data = _context.GL_ACCOUNTS_V.Where(x => String.Compare(x.SEGMENT1, _range.SRSEGMENT1) >= 0 && String.Compare(x.SEGMENT1, _range.ERSEGMENT1) <= 0);
                        _data = _data.Where(x => String.Compare(x.SEGMENT2, _range.SRSEGMENT2) >= 0 && String.Compare(x.SEGMENT2, _range.ERSEGMENT2) <= 0);
                        _data = _data.Where(x => String.Compare(x.SEGMENT3, _range.SRSEGMENT3) >= 0 && String.Compare(x.SEGMENT3, _range.ERSEGMENT3) <= 0);
                        _data = _data.Where(x => String.Compare(x.SEGMENT4, _range.SRSEGMENT4) >= 0 && String.Compare(x.SEGMENT4, _range.ERSEGMENT4) <= 0);
                        _data = _data.Where(x => String.Compare(x.SEGMENT5, _range.SRSEGMENT5) >= 0 && String.Compare(x.SEGMENT5, _range.ERSEGMENT5) <= 0);
                        _data = _data.Where(x => String.Compare(x.SEGMENT6, _range.SRSEGMENT6) >= 0 && String.Compare(x.SEGMENT6, _range.ERSEGMENT6) <= 0);
                        _data = _data.Where(x => String.Compare(x.SEGMENT7, _range.SRSEGMENT7) >= 0 && String.Compare(x.SEGMENT7, _range.ERSEGMENT7) <= 0);

                        List<GL_ACCOUNTS_V> _temp = _data.ToList();
                        List<GL_ACCOUNTS_V2> _newTemp = new List<GL_ACCOUNTS_V2>();

                        string IncludeExclude = e.Parameters["INCLUDE_EXCLUDE"];

                        foreach (GL_ACCOUNTS_V _acc in _temp)
                        {
                            GL_ACCOUNTS_V2 _new = new GL_ACCOUNTS_V2();
                            _new.CODE_COMBINATION_ID = _acc.CODE_COMBINATION_ID;
                            _new.SEGMENT1 = _acc.SEGMENT1;
                            _new.SEGMENT1_DESC = _acc.SEGMENT1_DESC;
                            _new.SEGMENT2 = _acc.SEGMENT2;
                            _new.SEGMENT2_DESC = _acc.SEGMENT2_DESC;
                            _new.SEGMENT3 = _acc.SEGMENT3;
                            _new.SEGMENT3_DESC = _acc.SEGMENT3_DESC;
                            _new.SEGMENT4 = _acc.SEGMENT4;
                            _new.SEGMENT4_DESC = _acc.SEGMENT4_DESC;
                            _new.SEGMENT5 = _acc.SEGMENT5;
                            _new.SEGMENT5_DESC = _acc.SEGMENT5_DESC;
                            _new.SEGMENT6 = _acc.SEGMENT6;
                            _new.SEGMENT6_DESC = _acc.SEGMENT6_DESC;
                            _new.SEGMENT7 = _acc.SEGMENT7;
                            _new.SEGMENT7_DESC = _acc.SEGMENT7_DESC;
                            _new.INCLUDED_EXCLUDED = (IncludeExclude == "Excluded") ? "Excluded" : IsGLAccountExcluded(_organizationID, _acc.SEGMENT1, _acc.SEGMENT2, _acc.SEGMENT3, _acc.SEGMENT4, _acc.SEGMENT5, _acc.SEGMENT6, _acc.SEGMENT7);
                            _newTemp.Add(_new);
                        }

                        uxGlAccountSecurityStore.DataSource = GenericData.ListFilterHeader<GL_ACCOUNTS_V2>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _newTemp.AsQueryable(), out count);
                        e.Total = count;

                }

            }

            public class GL_ACCOUNTS_V2 : GL_ACCOUNTS_V
            {
                public string INCLUDED_EXCLUDED { get; set; }
            }

            protected void deExcludeAccount(object sender, DirectEventArgs e)
            {
                RowSelectionModel _rsm = uxGlAccountSecurityGridSelectionModel;

                 long _organizationID = long.Parse(Request.QueryString["orgID"]);

                 foreach (SelectedRow _row in _rsm.SelectedRows)
                 {

                     GL_ACCOUNTS_V _account = GL_ACCOUNTS_V.AccountInformation(long.Parse(_row.RecordID));

                     OVERHEAD_GL_ACCOUNT _data = new OVERHEAD_GL_ACCOUNT();
                     _data.INCLUDE_EXCLUDE_FLAG = "E";
                     _data.ORGANIZATION_ID = _organizationID;
                     _data.SEGMENT1 = _account.SEGMENT1;
                     _data.SEGMENT2 = _account.SEGMENT2;
                     _data.SEGMENT3 = _account.SEGMENT3;
                     _data.SEGMENT4 = _account.SEGMENT4;
                     _data.SEGMENT5 = _account.SEGMENT5;
                     _data.SEGMENT6 = _account.SEGMENT6;
                     _data.SEGMENT7 = _account.SEGMENT7;
                     _data.CREATED_BY = User.Identity.Name;
                     _data.MODIFIED_BY = User.Identity.Name;
                     _data.CREATE_DATE = DateTime.Now;
                     _data.MODIFY_DATE = DateTime.Now;
                     GenericData.Insert<OVERHEAD_GL_ACCOUNT>(_data);

                 }

                uxGlAccountSecurityGridSelectionModel.DeselectAll(true);
                uxGlAccountSecurityStore.Reload(true);

            }

            protected void deIncludeAccount(object sender, DirectEventArgs e)
            {
                RowSelectionModel _rsm = uxGlAccountSecurityGridSelectionModel;

                long _organizationID = long.Parse(Request.QueryString["orgID"]);

                using (Entities _context = new Entities())
                {

                    foreach (SelectedRow _row in _rsm.SelectedRows)
                    {
                        GL_ACCOUNTS_V _account = GL_ACCOUNTS_V.AccountInformation(long.Parse(_row.RecordID));

                        List<OVERHEAD_GL_ACCOUNT> _data = _context.OVERHEAD_GL_ACCOUNT.Where(x => x.ORGANIZATION_ID == _organizationID && x.SEGMENT1 == _account.SEGMENT1 &&
                            x.SEGMENT2 == _account.SEGMENT2 && x.SEGMENT3 == _account.SEGMENT3 && x.SEGMENT4 == _account.SEGMENT4 && x.SEGMENT5 == _account.SEGMENT5
                            && x.SEGMENT6 == _account.SEGMENT6 && x.SEGMENT7 == _account.SEGMENT7).ToList();

                        if (_data.Count() > 0)
                            GenericData.Delete<OVERHEAD_GL_ACCOUNT>(_data);
                    }
                }

                uxGlAccountSecurityGridSelectionModel.DeselectAll();
                uxGlAccountSecurityStore.Reload(true);

            }



          protected void deSelectAccount(object sender, DirectEventArgs e)
            {
                if (uxGlAccountSecurityGridSelectionModel.SelectedRows.Count() > 0)
                {

                    SelectedRow _ssr = uxGLAccountRangeSelectionModel.SelectedRow;
                    long _recordID = long.Parse(_ssr.RecordID);

                    Boolean showButtons = true;
                    using (Entities _context = new Entities())
                    {
                        string check = _context.OVERHEAD_GL_RANGE.Where(x => x.GL_RANGE_ID == _recordID).SingleOrDefault().INCLUDE_EXCLUDE_FLAG;
                        showButtons = (check == "E") ? false : true;
                    }

                    if (showButtons)
                    {
                        uxExcludeAccount.Enable();
                        uxIncludeAccount.Enable();
                    }
                }
                else
                {
                    uxExcludeAccount.Disable();
                    uxIncludeAccount.Disable();
                }
            }

            protected void deDeSelectAccount(object sender, DirectEventArgs e)
            {
                uxExcludeAccount.Disable();
                uxIncludeAccount.Disable();
            }

    }
}


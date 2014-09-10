using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Ext.Net;
using DBI.Data;
using DBI.Core;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public partial class umAddGLAccountRange : DBI.Core.Web.BasePage
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

        protected void deLoadSegment(object sender, StoreReadDataEventArgs e)
        {
            try
            {

           Ext.Net.Store _storeDetails = sender as Ext.Net.Store;
           Entities _context = new Entities();          

               if (_storeDetails.ID == "uxSRSegment1Store")
               {
                   IQueryable<OVERHEAD_BUDGET_FORECAST.GL_ACCOUNT_LIST> _data = OVERHEAD_BUDGET_FORECAST.GeneralLedgerAccounts(_context);
                   List<DBI.Data.Generic.DoubleComboStringID> _rdata = _data.GroupBy(x => new { x.SEGMENT1}).Select(x => new DBI.Data.Generic.DoubleComboStringID { ID = x.Key.SEGMENT1 }).OrderBy(x => x.ID).ToList();

                   if (_rdata.Count == 0)
                       throw new DBICustomException("Not a valid account, please try another selection!");

                   foreach (DBI.Data.Generic.DoubleComboStringID _row in _rdata)
                   {
                       _row.ID_NAME = string.Format("{0} - {1}",_row.ID, OVERHEAD_BUDGET_FORECAST.AccountDescriptionBySegment(_context, 1, _row.ID));
                   }

                   uxSRSegment1Store.DataSource = _rdata;

               }

               if (_storeDetails.ID == "uxSRSegment2Store")
               {
                   IQueryable<OVERHEAD_BUDGET_FORECAST.GL_ACCOUNT_LIST> _data = OVERHEAD_BUDGET_FORECAST.GeneralLedgerAccounts(_context);
                   List<DBI.Data.Generic.DoubleComboStringID> _rdata = _data.GroupBy(x => new { x.SEGMENT1, x.SEGMENT2 }).Where(x => x.Key.SEGMENT1 == uxSRSegment1.SelectedItem.Value).Select(x => new DBI.Data.Generic.DoubleComboStringID { ID = x.Key.SEGMENT2 }).OrderBy(x => x.ID).ToList();

                   if (_rdata.Count == 0)
                       throw new DBICustomException("Not a valid account, please try another selection!");

                   foreach (DBI.Data.Generic.DoubleComboStringID _row in _rdata)
                   {
                       _row.ID_NAME = string.Format("{0} - {1}", _row.ID, OVERHEAD_BUDGET_FORECAST.AccountDescriptionBySegment(_context, 2, _row.ID));
                   }

                   uxSRSegment2Store.DataSource = _rdata;

               }

               if (_storeDetails.ID == "uxSRSegment3Store")
               {
                   IQueryable<OVERHEAD_BUDGET_FORECAST.GL_ACCOUNT_LIST> _data = OVERHEAD_BUDGET_FORECAST.GeneralLedgerAccounts(_context);
                   List<DBI.Data.Generic.DoubleComboStringID> _rdata = _data.GroupBy(x => new { x.SEGMENT1, x.SEGMENT2, x.SEGMENT3 }).Where(x => x.Key.SEGMENT1 == uxSRSegment1.SelectedItem.Value & x.Key.SEGMENT2 == uxSRSegment2.SelectedItem.Value).Select(x => new DBI.Data.Generic.DoubleComboStringID { ID = x.Key.SEGMENT3 }).OrderBy(x => x.ID).ToList();

                   if (_rdata.Count == 0)
                       throw new DBICustomException("Not a valid account, please try another selection!");

                   foreach (DBI.Data.Generic.DoubleComboStringID _row in _rdata)
                   {
                       _row.ID_NAME = string.Format("{0} - {1}", _row.ID, OVERHEAD_BUDGET_FORECAST.AccountDescriptionBySegment(_context, 3, _row.ID));
                   }

                   uxSRSegment3Store.DataSource = _rdata;
               }

               if (_storeDetails.ID == "uxSRSegment4Store")
               {
                   IQueryable<OVERHEAD_BUDGET_FORECAST.GL_ACCOUNT_LIST> _data = OVERHEAD_BUDGET_FORECAST.GeneralLedgerAccounts(_context);
                   List<DBI.Data.Generic.DoubleComboStringID> _rdata = _data.GroupBy(x => new { x.SEGMENT1, x.SEGMENT2, x.SEGMENT3, x.SEGMENT4 }).Where(x => x.Key.SEGMENT1 == uxSRSegment1.SelectedItem.Value & x.Key.SEGMENT2 == uxSRSegment2.SelectedItem.Value & x.Key.SEGMENT3 == uxSRSegment3.SelectedItem.Value).Select(x => new DBI.Data.Generic.DoubleComboStringID { ID = x.Key.SEGMENT4 }).OrderBy(x => x.ID).ToList();

                   if (_rdata.Count == 0)
                       throw new DBICustomException("Not a valid account, please try another selection!");
                   
                   foreach (DBI.Data.Generic.DoubleComboStringID _row in _rdata)
                   {
                       _row.ID_NAME = string.Format("{0} - {1}", _row.ID, OVERHEAD_BUDGET_FORECAST.AccountDescriptionBySegment(_context, 4, _row.ID));
                   }

                   uxSRSegment4Store.DataSource = _rdata;
               }

               if (_storeDetails.ID == "uxSRSegment5Store")
               {
                   IQueryable<OVERHEAD_BUDGET_FORECAST.GL_ACCOUNT_LIST> _data = OVERHEAD_BUDGET_FORECAST.GeneralLedgerAccounts(_context);
                   List<DBI.Data.Generic.DoubleComboStringID> _rdata = _data.GroupBy(x => new { x.SEGMENT1, x.SEGMENT2, x.SEGMENT3, x.SEGMENT4, x.SEGMENT5 }).Where(x => x.Key.SEGMENT1 == uxSRSegment1.SelectedItem.Value & x.Key.SEGMENT2 == uxSRSegment2.SelectedItem.Value & x.Key.SEGMENT3 == uxSRSegment3.SelectedItem.Value & x.Key.SEGMENT4 == uxSRSegment4.SelectedItem.Value).Select(x => new DBI.Data.Generic.DoubleComboStringID { ID = x.Key.SEGMENT5 }).OrderBy(x => x.ID).ToList();

                   if (_rdata.Count == 0)
                       throw new DBICustomException("Not a valid account, please try another selection!");
                   
                   foreach (DBI.Data.Generic.DoubleComboStringID _row in _rdata)
                   {
                       _row.ID_NAME = string.Format("{0} - {1}", _row.ID, OVERHEAD_BUDGET_FORECAST.AccountDescriptionBySegment(_context, 5, _row.ID));
                   }

                   uxSRSegment5Store.DataSource = _rdata;
               }

               if (_storeDetails.ID == "uxSRSegment6Store")
               {
                   IQueryable<OVERHEAD_BUDGET_FORECAST.GL_ACCOUNT_LIST> _data = OVERHEAD_BUDGET_FORECAST.GeneralLedgerAccounts(_context);
                   List<DBI.Data.Generic.DoubleComboStringID> _rdata = _data.GroupBy(x => new { x.SEGMENT1, x.SEGMENT2, x.SEGMENT3, x.SEGMENT4, x.SEGMENT5, x.SEGMENT6 }).Where(x => x.Key.SEGMENT1 == uxSRSegment1.SelectedItem.Value & x.Key.SEGMENT2 == uxSRSegment2.SelectedItem.Value & x.Key.SEGMENT3 == uxSRSegment3.SelectedItem.Value & x.Key.SEGMENT4 == uxSRSegment4.SelectedItem.Value & x.Key.SEGMENT5 == uxSRSegment5.SelectedItem.Value ).Select(x => new DBI.Data.Generic.DoubleComboStringID { ID = x.Key.SEGMENT6 }).OrderBy(x => x.ID).ToList();

                   if (_rdata.Count == 0)
                       throw new DBICustomException("Not a valid account, please try another selection!");
                   
                   foreach (DBI.Data.Generic.DoubleComboStringID _row in _rdata)
                   {
                       _row.ID_NAME = string.Format("{0} - {1}", _row.ID, OVERHEAD_BUDGET_FORECAST.AccountDescriptionBySegment(_context, 6, _row.ID));
                   }

                   uxSRSegment6Store.DataSource = _rdata;
               }

               if (_storeDetails.ID == "uxSRSegment7Store")
               {
                   IQueryable<OVERHEAD_BUDGET_FORECAST.GL_ACCOUNT_LIST> _data = OVERHEAD_BUDGET_FORECAST.GeneralLedgerAccounts(_context);
                   List<DBI.Data.Generic.DoubleComboStringID> _rdata = _data.GroupBy(x => new { x.SEGMENT1, x.SEGMENT2, x.SEGMENT3, x.SEGMENT4, x.SEGMENT5, x.SEGMENT6, x.SEGMENT7 }).Where(x => x.Key.SEGMENT1 == uxSRSegment1.SelectedItem.Value & x.Key.SEGMENT2 == uxSRSegment2.SelectedItem.Value & x.Key.SEGMENT3 == uxSRSegment3.SelectedItem.Value & x.Key.SEGMENT4 == uxSRSegment4.SelectedItem.Value & x.Key.SEGMENT6 == uxSRSegment6.SelectedItem.Value).Select(x => new DBI.Data.Generic.DoubleComboStringID { ID = x.Key.SEGMENT7 }).OrderBy(x => x.ID).ToList();

                   if (_rdata.Count == 0)
                       throw new DBICustomException("Not a valid account, please try another selection!");
                   
                   foreach (DBI.Data.Generic.DoubleComboStringID _row in _rdata)
                   {
                       _row.ID_NAME = string.Format("{0} - {1}", _row.ID, OVERHEAD_BUDGET_FORECAST.AccountDescriptionBySegment(_context, 7, _row.ID));
                   }

                   uxSRSegment7Store.DataSource = _rdata.ToList();
               }

               if (_storeDetails.ID == "uxERSegment1Store")
               {
                   IQueryable<OVERHEAD_BUDGET_FORECAST.GL_ACCOUNT_LIST> _data = OVERHEAD_BUDGET_FORECAST.GeneralLedgerAccounts(_context);
                   List<DBI.Data.Generic.DoubleComboStringID> _rdata = _data.GroupBy(x => new { x.SEGMENT1 }).Where(y => y.Key.SEGMENT1 == uxSRSegment1.SelectedItem.Value).Select(x => new DBI.Data.Generic.DoubleComboStringID { ID = x.Key.SEGMENT1 }).OrderBy(x => x.ID).ToList();

                   if (_rdata.Count == 0)
                       throw new DBICustomException("Not a valid account, please try another selection!");
                   
                   foreach (DBI.Data.Generic.DoubleComboStringID _row in _rdata)
                   {
                       _row.ID_NAME = string.Format("{0} - {1}", _row.ID, OVERHEAD_BUDGET_FORECAST.AccountDescriptionBySegment(_context, 1, _row.ID));
                   }

                   uxERSegment1Store.DataSource = _rdata;
               }

               if (_storeDetails.ID == "uxERSegment2Store")
               {
                   IQueryable<OVERHEAD_BUDGET_FORECAST.GL_ACCOUNT_LIST> _data = OVERHEAD_BUDGET_FORECAST.GeneralLedgerAccounts(_context);
                   List<DBI.Data.Generic.DoubleComboStringID> _rdata = _data.GroupBy(x => new { x.SEGMENT1, x.SEGMENT2 }).Where(x => x.Key.SEGMENT1 == uxERSegment1.SelectedItem.Value & String.Compare(x.Key.SEGMENT2, uxSRSegment2.SelectedItem.Value) >= 0).Select(x => new DBI.Data.Generic.DoubleComboStringID { ID = x.Key.SEGMENT2 }).OrderBy(x => x.ID).ToList();

                   if (_rdata.Count == 0)
                       throw new DBICustomException("Not a valid account, please try another selection!");
                   
                   foreach (DBI.Data.Generic.DoubleComboStringID _row in _rdata)
                   {
                       _row.ID_NAME = string.Format("{0} - {1}", _row.ID, OVERHEAD_BUDGET_FORECAST.AccountDescriptionBySegment(_context, 2, _row.ID));
                   }

                   uxERSegment2Store.DataSource = _rdata;

               }

               if (_storeDetails.ID == "uxERSegment3Store")
               {
                   IQueryable<OVERHEAD_BUDGET_FORECAST.GL_ACCOUNT_LIST> _data = OVERHEAD_BUDGET_FORECAST.GeneralLedgerAccounts(_context);
                   List<DBI.Data.Generic.DoubleComboStringID> _rdata = _data.GroupBy(x => new { x.SEGMENT1, x.SEGMENT2, x.SEGMENT3 }).Where(x => x.Key.SEGMENT1 == uxERSegment1.SelectedItem.Value & x.Key.SEGMENT2 == uxERSegment2.SelectedItem.Value & String.Compare(x.Key.SEGMENT3, uxSRSegment3.SelectedItem.Value) >= 0).Select(x => new DBI.Data.Generic.DoubleComboStringID { ID = x.Key.SEGMENT3 }).OrderBy(x => x.ID).ToList();

                   if (_rdata.Count == 0)
                       throw new DBICustomException("Not a valid account, please try another selection!");
                   
                   foreach (DBI.Data.Generic.DoubleComboStringID _row in _rdata)
                   {
                       _row.ID_NAME = string.Format("{0} - {1}", _row.ID, OVERHEAD_BUDGET_FORECAST.AccountDescriptionBySegment(_context, 3, _row.ID));
                   }

                   uxERSegment3Store.DataSource = _rdata;
               }

               if (_storeDetails.ID == "uxERSegment4Store")
               {
                   IQueryable<OVERHEAD_BUDGET_FORECAST.GL_ACCOUNT_LIST> _data = OVERHEAD_BUDGET_FORECAST.GeneralLedgerAccounts(_context);
                   List<DBI.Data.Generic.DoubleComboStringID> _rdata = _data.GroupBy(x => new { x.SEGMENT1, x.SEGMENT2, x.SEGMENT3, x.SEGMENT4 }).Where(x => x.Key.SEGMENT1 == uxERSegment1.SelectedItem.Value & x.Key.SEGMENT2 == uxERSegment2.SelectedItem.Value & x.Key.SEGMENT3 == uxERSegment3.SelectedItem.Value & String.Compare(x.Key.SEGMENT4, uxSRSegment4.SelectedItem.Value) >= 0).Select(x => new DBI.Data.Generic.DoubleComboStringID { ID = x.Key.SEGMENT4 }).OrderBy(x => x.ID).ToList();

                   if (_rdata.Count == 0)
                       throw new DBICustomException("Not a valid account, please try another selection!");
                   
                   foreach (DBI.Data.Generic.DoubleComboStringID _row in _rdata)
                   {
                       _row.ID_NAME = string.Format("{0} - {1}", _row.ID, OVERHEAD_BUDGET_FORECAST.AccountDescriptionBySegment(_context, 4, _row.ID));
                   }

                   uxERSegment4Store.DataSource = _rdata.ToList();
               }

               if (_storeDetails.ID == "uxERSegment5Store")
               {
                   IQueryable<OVERHEAD_BUDGET_FORECAST.GL_ACCOUNT_LIST> _data = OVERHEAD_BUDGET_FORECAST.GeneralLedgerAccounts(_context);
                   List<DBI.Data.Generic.DoubleComboStringID> _rdata = _data.GroupBy(x => new { x.SEGMENT1, x.SEGMENT2, x.SEGMENT3, x.SEGMENT4, x.SEGMENT5 }).Where(x => x.Key.SEGMENT1 == uxERSegment1.SelectedItem.Value & x.Key.SEGMENT2 == uxERSegment2.SelectedItem.Value & x.Key.SEGMENT3 == uxERSegment3.SelectedItem.Value & x.Key.SEGMENT4 == uxERSegment4.SelectedItem.Value & String.Compare(x.Key.SEGMENT5, uxSRSegment5.SelectedItem.Value) >= 0).Select(x => new DBI.Data.Generic.DoubleComboStringID { ID = x.Key.SEGMENT5 }).OrderBy(x => x.ID).ToList();

                   if (_rdata.Count == 0)
                       throw new DBICustomException("Not a valid account, please try another selection!");
                   
                   foreach (DBI.Data.Generic.DoubleComboStringID _row in _rdata)
                   {
                       _row.ID_NAME = string.Format("{0} - {1}", _row.ID, OVERHEAD_BUDGET_FORECAST.AccountDescriptionBySegment(_context, 5, _row.ID));
                   }

                   uxERSegment5Store.DataSource = _rdata;
               }

               if (_storeDetails.ID == "uxERSegment6Store")
               {
                   IQueryable<OVERHEAD_BUDGET_FORECAST.GL_ACCOUNT_LIST> _data = OVERHEAD_BUDGET_FORECAST.GeneralLedgerAccounts(_context);
                   List<DBI.Data.Generic.DoubleComboStringID> _rdata = _data.GroupBy(x => new { x.SEGMENT1, x.SEGMENT2, x.SEGMENT3, x.SEGMENT4, x.SEGMENT5, x.SEGMENT6 }).Where(x => x.Key.SEGMENT1 == uxERSegment1.SelectedItem.Value & x.Key.SEGMENT2 == uxERSegment2.SelectedItem.Value & x.Key.SEGMENT3 == uxERSegment3.SelectedItem.Value & x.Key.SEGMENT4 == uxERSegment4.SelectedItem.Value & x.Key.SEGMENT5 == uxERSegment5.SelectedItem.Value & String.Compare(x.Key.SEGMENT6, uxSRSegment6.SelectedItem.Value) >= 0).Select(x => new DBI.Data.Generic.DoubleComboStringID { ID = x.Key.SEGMENT6 }).OrderBy(x => x.ID).ToList();

                   if (_rdata.Count == 0)
                       throw new DBICustomException("Not a valid account, please try another selection!");
                   
                   foreach (DBI.Data.Generic.DoubleComboStringID _row in _rdata)
                   {
                       _row.ID_NAME = string.Format("{0} - {1}", _row.ID, OVERHEAD_BUDGET_FORECAST.AccountDescriptionBySegment(_context, 6, _row.ID));
                   }

                   uxERSegment6Store.DataSource = _rdata;
               }

               if (_storeDetails.ID == "uxERSegment7Store")
               {
                   IQueryable<OVERHEAD_BUDGET_FORECAST.GL_ACCOUNT_LIST> _data = OVERHEAD_BUDGET_FORECAST.GeneralLedgerAccounts(_context);
                   List<DBI.Data.Generic.DoubleComboStringID> _rdata = _data.GroupBy(x => new { x.SEGMENT1, x.SEGMENT2, x.SEGMENT3, x.SEGMENT4, x.SEGMENT5, x.SEGMENT6, x.SEGMENT7 }).Where(x => x.Key.SEGMENT1 == uxERSegment1.SelectedItem.Value & x.Key.SEGMENT2 == uxERSegment2.SelectedItem.Value & x.Key.SEGMENT3 == uxERSegment3.SelectedItem.Value & x.Key.SEGMENT4 == uxERSegment4.SelectedItem.Value & x.Key.SEGMENT6 == uxERSegment6.SelectedItem.Value & String.Compare(x.Key.SEGMENT7, uxSRSegment7.SelectedItem.Value) >= 0).Select(x => new DBI.Data.Generic.DoubleComboStringID { ID = x.Key.SEGMENT7 }).OrderBy(x => x.ID).ToList();

                   if (_rdata.Count == 0)
                       throw new DBICustomException("Not a valid account, please try another selection!");
                   
                   foreach (DBI.Data.Generic.DoubleComboStringID _row in _rdata)
                   {
                       _row.ID_NAME = string.Format("{0} - {1}", _row.ID, OVERHEAD_BUDGET_FORECAST.AccountDescriptionBySegment(_context, 7, _row.ID));
                   }

                   uxERSegment7Store.DataSource = _rdata;
               }

            }
            catch (DBICustomException ex)
            {
                X.Msg.Alert("Invalid GL Account", ex.Message.ToString()).Show();
            }
        }


        protected void deReadGLSecurityCodes(object sender, StoreReadDataEventArgs e)
        {

            using (Entities _context = new Entities())
            {
                int count;
                var _data = _context.GL_ACCOUNTS_V.Where(x => string.Compare(x.SEGMENT1 + x.SEGMENT2 + x.SEGMENT3 + x.SEGMENT4 + x.SEGMENT5 + x.SEGMENT6 + x.SEGMENT7,uxSRSegment1.SelectedItem.Value + uxSRSegment2.SelectedItem.Value + uxSRSegment3.SelectedItem.Value + uxSRSegment4.SelectedItem.Value + uxSRSegment5.SelectedItem.Value + uxSRSegment6.SelectedItem.Value + uxSRSegment7.SelectedItem.Value) >= 0).Where(x => string.Compare(x.SEGMENT1 + x.SEGMENT2 + x.SEGMENT3 + x.SEGMENT4 + x.SEGMENT5 + x.SEGMENT6 + x.SEGMENT7,uxSRSegment1.SelectedItem.Value + uxSRSegment2.SelectedItem.Value + uxSRSegment3.SelectedItem.Value + uxSRSegment4.SelectedItem.Value + uxSRSegment5.SelectedItem.Value + uxSRSegment6.SelectedItem.Value + uxSRSegment7.SelectedItem.Value) <= 0);

                List<GL_ACCOUNTS_V> _temp = _data.ToList();
                List<DBI.Web.EMS.Views.Modules.Overhead.umOverheadGeneralLedger.GL_ACCOUNTS_V2> _newTemp = new List<DBI.Web.EMS.Views.Modules.Overhead.umOverheadGeneralLedger.GL_ACCOUNTS_V2>();

                foreach (GL_ACCOUNTS_V _acc in _temp)
                {
                    DBI.Web.EMS.Views.Modules.Overhead.umOverheadGeneralLedger.GL_ACCOUNTS_V2 _new = new DBI.Web.EMS.Views.Modules.Overhead.umOverheadGeneralLedger.GL_ACCOUNTS_V2();
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
                    _newTemp.Add(_new);
                }

                uxGlAccountSecurityStore.DataSource = GenericData.ListFilterHeader<DBI.Web.EMS.Views.Modules.Overhead.umOverheadGeneralLedger.GL_ACCOUNTS_V2>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _newTemp.AsQueryable(), out count);
                e.Total = count;





            }
        }

        protected void deAddAccountRange(object sender, DirectEventArgs e)
        {

            //validate the range being added and make sure it doesn't overlap
            bool duplicateRange = false;

            using (Entities _context = new Entities())
            {
                long _organizationID = long.Parse(Request.QueryString["orgID"]);
                IQueryable<OVERHEAD_GL_RANGE> _data = _context.OVERHEAD_GL_RANGE.Where(x => x.ORGANIZATION_ID == _organizationID);
                foreach (OVERHEAD_GL_RANGE _range in _data)
                {
                    string startingRange = _range.SRSEGMENT1 + _range.SRSEGMENT2 + _range.SRSEGMENT3 + _range.SRSEGMENT4 + _range.SRSEGMENT5 + _range.SRSEGMENT6 + _range.SRSEGMENT7;
                    string endingRange = _range.ERSEGMENT1 + _range.ERSEGMENT2 + _range.ERSEGMENT3 + _range.ERSEGMENT4 + _range.ERSEGMENT5 + _range.ERSEGMENT6 + _range.ERSEGMENT7;

                    string validateSR = uxSRSegment1.SelectedItem.Value + uxSRSegment2.SelectedItem.Value + uxSRSegment3.SelectedItem.Value + uxSRSegment4.SelectedItem.Value + uxSRSegment5.SelectedItem.Value + uxSRSegment6.SelectedItem.Value + uxSRSegment7.SelectedItem.Value;
                    string validateER = uxERSegment1.SelectedItem.Value + uxERSegment2.SelectedItem.Value + uxERSegment3.SelectedItem.Value + uxERSegment4.SelectedItem.Value + uxERSegment5.SelectedItem.Value + uxERSegment6.SelectedItem.Value + uxERSegment7.SelectedItem.Value;

                    if(string.Compare(validateSR,startingRange) >= 0 && string.Compare(validateER, endingRange) <= 0)
                    {
                        duplicateRange = true;
                        break;
                    }
                }

            }
  
            if (!duplicateRange)
            {
                //Add the range to the database
                OVERHEAD_GL_RANGE _ogr = new OVERHEAD_GL_RANGE();
                _ogr.ORGANIZATION_ID = long.Parse(Request.QueryString["orgID"]);
                _ogr.SRSEGMENT1 = uxSRSegment1.SelectedItem.Value;
                _ogr.SRSEGMENT2 = uxSRSegment2.SelectedItem.Value;
                _ogr.SRSEGMENT3 = uxSRSegment3.SelectedItem.Value;
                _ogr.SRSEGMENT4 = uxSRSegment4.SelectedItem.Value;
                _ogr.SRSEGMENT5 = uxSRSegment5.SelectedItem.Value;
                _ogr.SRSEGMENT6 = uxSRSegment6.SelectedItem.Value;
                _ogr.SRSEGMENT7 = uxSRSegment7.SelectedItem.Value;
                _ogr.ERSEGMENT1 = uxERSegment1.SelectedItem.Value;
                _ogr.ERSEGMENT2 = uxERSegment2.SelectedItem.Value;
                _ogr.ERSEGMENT3 = uxERSegment3.SelectedItem.Value;
                _ogr.ERSEGMENT4 = uxERSegment4.SelectedItem.Value;
                _ogr.ERSEGMENT5 = uxERSegment5.SelectedItem.Value;
                _ogr.ERSEGMENT6 = uxERSegment6.SelectedItem.Value;
                _ogr.ERSEGMENT7 = uxERSegment7.SelectedItem.Value;
                _ogr.CREATE_DATE = DateTime.Now;
                _ogr.MODIFY_DATE = DateTime.Now;
                _ogr.CREATED_BY = User.Identity.Name;
                _ogr.MODIFIED_BY = User.Identity.Name;
                GenericData.Insert<OVERHEAD_GL_RANGE>(_ogr);
            }
            else
            {
                e.ErrorMessage = "Duplicate range detected. This account already exists, please select a different range of accounts";
                e.Success = false;
            }
        }

    }
}  

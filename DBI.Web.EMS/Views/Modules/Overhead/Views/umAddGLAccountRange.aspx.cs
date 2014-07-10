using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Ext.Net;
using DBI.Data;
using DBI.Core;

namespace DBI.Web.EMS.Views.Modules.Overhead.Views
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
           Ext.Net.Store _storeDetails = sender as Ext.Net.Store;
           Entities _context = new Entities();          

               if (_storeDetails.ID == "uxSRSegment1Store")
               {

                   IQueryable<object> _data = _context.GL_ACCOUNTS_V.Select(x => new { ID = x.SEGMENT1 }).Distinct().OrderBy(x => x.ID);
                   uxSRSegment1Store.DataSource = _data.ToList();
                   _context.Dispose();
               }

               if (_storeDetails.ID == "uxSRSegment2Store")
               {
                   IQueryable<object> _data = _context.GL_ACCOUNTS_V.Where(x => x.SEGMENT1 == uxSRSegment1.SelectedItem.Value).Select(x => new { ID = x.SEGMENT2 }).Distinct().OrderBy(x => x.ID);
                   uxSRSegment2Store.DataSource = _data.ToList();
                   _context.Dispose();
               }

               if (_storeDetails.ID == "uxSRSegment3Store")
               {
                   IQueryable<object> _data = _context.GL_ACCOUNTS_V.Where(x => x.SEGMENT1 == uxSRSegment1.SelectedItem.Value && x.SEGMENT2 == uxSRSegment2.SelectedItem.Value).Select(x => new { ID = x.SEGMENT3 }).Distinct().OrderBy(x => x.ID);
                   uxSRSegment3Store.DataSource = _data.ToList();
                   _context.Dispose();
               }

               if (_storeDetails.ID == "uxSRSegment4Store")
               {
                   IQueryable<object> _data = _context.GL_ACCOUNTS_V.Where(x => x.SEGMENT1 == uxSRSegment1.SelectedItem.Value && x.SEGMENT2 == uxSRSegment2.SelectedItem.Value && x.SEGMENT3 == uxSRSegment3.SelectedItem.Value).Select(x => new { ID = x.SEGMENT4 }).Distinct().OrderBy(x => x.ID);
                   uxSRSegment4Store.DataSource = _data.ToList();
                   _context.Dispose();
               }

               if (_storeDetails.ID == "uxSRSegment5Store")
               {
                   IQueryable<object> _data = _context.GL_ACCOUNTS_V.Where(x => x.SEGMENT1 == uxSRSegment1.SelectedItem.Value && x.SEGMENT2 == uxSRSegment2.SelectedItem.Value && x.SEGMENT3 == uxSRSegment3.SelectedItem.Value && x.SEGMENT4 == uxSRSegment4.SelectedItem.Value).Select(x => new { ID = x.SEGMENT5 }).Distinct().OrderBy(x => x.ID);
                   uxSRSegment5Store.DataSource = _data.ToList();
                   _context.Dispose();
               }

               if (_storeDetails.ID == "uxSRSegment6Store")
               {
                   IQueryable<object> _data = _context.GL_ACCOUNTS_V.Where(x => x.SEGMENT1 == uxSRSegment1.SelectedItem.Value && x.SEGMENT2 == uxSRSegment2.SelectedItem.Value && x.SEGMENT3 == uxSRSegment3.SelectedItem.Value && x.SEGMENT4 == uxSRSegment4.SelectedItem.Value && x.SEGMENT5 == uxSRSegment5.SelectedItem.Value).Select(x => new { ID = x.SEGMENT6 }).Distinct().OrderBy(x => x.ID);
                   uxSRSegment6Store.DataSource = _data.ToList();
                   _context.Dispose();
               }

               if (_storeDetails.ID == "uxSRSegment7Store")
               {
                   IQueryable<object> _data = _context.GL_ACCOUNTS_V.Where(x => x.SEGMENT1 == uxSRSegment1.SelectedItem.Value && x.SEGMENT2 == uxSRSegment2.SelectedItem.Value && x.SEGMENT3 == uxSRSegment3.SelectedItem.Value && x.SEGMENT4 == uxSRSegment4.SelectedItem.Value && x.SEGMENT5 == uxSRSegment5.SelectedItem.Value && x.SEGMENT6 == uxSRSegment6.SelectedItem.Value).Select(x => new { ID = x.SEGMENT7 }).Distinct().OrderBy(x => x.ID);
                   uxSRSegment7Store.DataSource = _data.ToList();
                   _context.Dispose();
               }

               if (_storeDetails.ID == "uxERSegment1Store")
               {

                   IQueryable<object> _data = _context.GL_ACCOUNTS_V.Where(x => x.SEGMENT1 == uxSRSegment1.SelectedItem.Value).Select(x => new { ID = x.SEGMENT1 }).Distinct().OrderBy(x => x.ID);
                   uxERSegment1Store.DataSource = _data.ToList();
                   _context.Dispose();
               }

               if (_storeDetails.ID == "uxERSegment2Store")
               {
                   IQueryable<object> _data = _context.GL_ACCOUNTS_V.Where(x => x.SEGMENT1 == uxSRSegment1.SelectedItem.Value && String.Compare(x.SEGMENT2, uxSRSegment2.SelectedItem.Value) >= 0).Select(x => new { ID = x.SEGMENT2 }).Distinct().OrderBy(x => x.ID);
                   uxERSegment2Store.DataSource = _data.ToList();
                   _context.Dispose();
               }

               if (_storeDetails.ID == "uxERSegment3Store")
               {
                   IQueryable<object> _data = _context.GL_ACCOUNTS_V.Where(x => x.SEGMENT1 == uxSRSegment1.SelectedItem.Value && x.SEGMENT2 == uxSRSegment2.SelectedItem.Value && String.Compare(x.SEGMENT3, uxSRSegment3.SelectedItem.Value) >= 0).Select(x => new { ID = x.SEGMENT3 }).Distinct().OrderBy(x => x.ID);
                   uxERSegment3Store.DataSource = _data.ToList();
                   _context.Dispose();
               }

               if (_storeDetails.ID == "uxERSegment4Store")
               {
                   IQueryable<object> _data = _context.GL_ACCOUNTS_V.Where(x => x.SEGMENT1 == uxSRSegment1.SelectedItem.Value && x.SEGMENT2 == uxSRSegment2.SelectedItem.Value && x.SEGMENT3 == uxSRSegment3.SelectedItem.Value && String.Compare(x.SEGMENT4, uxSRSegment4.SelectedItem.Value) >= 0).Select(x => new { ID = x.SEGMENT4 }).Distinct().OrderBy(x => x.ID);
                   uxERSegment4Store.DataSource = _data.ToList();
                   _context.Dispose();
               }

               if (_storeDetails.ID == "uxERSegment5Store")
               {
                   IQueryable<object> _data = _context.GL_ACCOUNTS_V.Where(x => x.SEGMENT1 == uxSRSegment1.SelectedItem.Value && x.SEGMENT2 == uxSRSegment2.SelectedItem.Value && x.SEGMENT3 == uxSRSegment3.SelectedItem.Value && x.SEGMENT4 == uxSRSegment4.SelectedItem.Value && String.Compare(x.SEGMENT5, uxSRSegment5.SelectedItem.Value) >= 0).Select(x => new { ID = x.SEGMENT5 }).Distinct().OrderBy(x => x.ID);
                   uxERSegment5Store.DataSource = _data.ToList();
                   _context.Dispose();
               }

               if (_storeDetails.ID == "uxERSegment6Store")
               {
                   IQueryable<object> _data = _context.GL_ACCOUNTS_V.Where(x => x.SEGMENT1 == uxSRSegment1.SelectedItem.Value && x.SEGMENT2 == uxSRSegment2.SelectedItem.Value && x.SEGMENT3 == uxSRSegment3.SelectedItem.Value && x.SEGMENT4 == uxSRSegment4.SelectedItem.Value && x.SEGMENT5 == uxSRSegment5.SelectedItem.Value && String.Compare(x.SEGMENT6, uxSRSegment6.SelectedItem.Value) >= 0).Select(x => new { ID = x.SEGMENT6 }).Distinct().OrderBy(x => x.ID);
                   uxERSegment6Store.DataSource = _data.ToList();
                   _context.Dispose();
               }

               if (_storeDetails.ID == "uxERSegment7Store")
               {
                   IQueryable<object> _data = _context.GL_ACCOUNTS_V.Where(x => x.SEGMENT1 == uxSRSegment1.SelectedItem.Value && x.SEGMENT2 == uxSRSegment2.SelectedItem.Value && x.SEGMENT3 == uxSRSegment3.SelectedItem.Value && x.SEGMENT4 == uxSRSegment4.SelectedItem.Value && x.SEGMENT5 == uxSRSegment5.SelectedItem.Value && x.SEGMENT6 == uxSRSegment6.SelectedItem.Value && String.Compare(x.SEGMENT7, uxSRSegment7.SelectedItem.Value) >= 0).Select(x => new { ID = x.SEGMENT7 }).Distinct().OrderBy(x => x.ID);
                   uxERSegment7Store.DataSource = _data.ToList();
                   _context.Dispose();
               }
        }


        protected void deReadGLSecurityCodes(object sender, StoreReadDataEventArgs e)
        {

            using (Entities _context = new Entities())
            {
                int count;
                IQueryable<GL_ACCOUNTS_V> _data = _context.GL_ACCOUNTS_V.Where(x => String.Compare(x.SEGMENT1, uxSRSegment1.SelectedItem.Value) >= 0 && String.Compare(x.SEGMENT1, uxERSegment1.SelectedItem.Value) <= 0);
                _data = _data.Where(x => String.Compare(x.SEGMENT2, uxSRSegment2.SelectedItem.Value) >= 0 && String.Compare(x.SEGMENT2, uxERSegment2.SelectedItem.Value) <= 0);
                _data = _data.Where(x => String.Compare(x.SEGMENT3, uxSRSegment3.SelectedItem.Value) >= 0 && String.Compare(x.SEGMENT3, uxERSegment3.SelectedItem.Value) <= 0);
                _data = _data.Where(x => String.Compare(x.SEGMENT4, uxSRSegment4.SelectedItem.Value) >= 0 && String.Compare(x.SEGMENT4, uxERSegment4.SelectedItem.Value) <= 0);
                _data = _data.Where(x => String.Compare(x.SEGMENT5, uxSRSegment5.SelectedItem.Value) >= 0 && String.Compare(x.SEGMENT5, uxERSegment5.SelectedItem.Value) <= 0);
                _data = _data.Where(x => String.Compare(x.SEGMENT6, uxSRSegment6.SelectedItem.Value) >= 0 && String.Compare(x.SEGMENT6, uxERSegment6.SelectedItem.Value) <= 0);
                _data = _data.Where(x => String.Compare(x.SEGMENT7, uxSRSegment7.SelectedItem.Value) >= 0 && String.Compare(x.SEGMENT7, uxERSegment7.SelectedItem.Value) <= 0);
                uxGlAccountSecurityStore.DataSource = GenericData.ListFilterHeader<GL_ACCOUNTS_V>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _data, out count);
                e.Total = count;
            }



        }

        protected void deSelectionMemoryTest(object sender, DirectEventArgs e)
        {
            CheckboxSelectionModel sm = uxGlAccountSecurityGridSelectionModel;
            List<SelectedRow> src = sm.SelectedRows.ToList();


        }

    }
}
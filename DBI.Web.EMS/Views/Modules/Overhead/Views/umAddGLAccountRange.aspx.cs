using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Ext.Net;
using DBI.Data;

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

               if (_storeDetails.ID == "uxSRSegment1Store")
               {
                   Entities _context = new Entities();
                   IQueryable<object> _data = _context.GL_ACCOUNTS_V.Select(x => new { ID = x.SEGMENT1 }).Distinct().OrderBy(x => x.ID);
                   uxSRSegment1Store.DataSource = _data.ToList();
               }

               if (_storeDetails.ID == "uxSRSegment2Store")
               {
                   Entities _context = new Entities();
                   IQueryable<object> _data = _context.GL_ACCOUNTS_V.Where(x => x.SEGMENT1 == uxSRSegment1.SelectedItem.Value).Select(x => new { ID = x.SEGMENT2 }).Distinct().OrderBy(x => x.ID);
                   uxSRSegment2Store.DataSource = _data.ToList();
               }

               if (_storeDetails.ID == "uxSRSegment3Store")
               {
                   Entities _context = new Entities();
                   IQueryable<object> _data = _context.GL_ACCOUNTS_V.Where(x => x.SEGMENT1 == uxSRSegment1.SelectedItem.Value && x.SEGMENT2 == uxSRSegment2.SelectedItem.Value).Select(x => new { ID = x.SEGMENT3 }).Distinct().OrderBy(x => x.ID);
                   uxSRSegment3Store.DataSource = _data.ToList();
               }

               if (_storeDetails.ID == "uxSRSegment4Store")
               {
                   Entities _context = new Entities();
                   IQueryable<object> _data = _context.GL_ACCOUNTS_V.Where(x => x.SEGMENT1 == uxSRSegment1.SelectedItem.Value && x.SEGMENT2 == uxSRSegment2.SelectedItem.Value && x.SEGMENT3 == uxSRSegment3.SelectedItem.Value).Select(x => new { ID = x.SEGMENT4 }).Distinct().OrderBy(x => x.ID);
                   uxSRSegment4Store.DataSource = _data.ToList();
               }

               if (_storeDetails.ID == "uxSRSegment5Store")
               {
                   Entities _context = new Entities();
                   IQueryable<object> _data = _context.GL_ACCOUNTS_V.Where(x => x.SEGMENT1 == uxSRSegment1.SelectedItem.Value && x.SEGMENT2 == uxSRSegment2.SelectedItem.Value && x.SEGMENT3 == uxSRSegment3.SelectedItem.Value && x.SEGMENT4 == uxSRSegment4.SelectedItem.Value).Select(x => new { ID = x.SEGMENT5 }).Distinct().OrderBy(x => x.ID);
                   uxSRSegment5Store.DataSource = _data.ToList();
               }

               if (_storeDetails.ID == "uxSRSegment6Store")
               {
                   Entities _context = new Entities();
                   IQueryable<object> _data = _context.GL_ACCOUNTS_V.Where(x => x.SEGMENT1 == uxSRSegment1.SelectedItem.Value && x.SEGMENT2 == uxSRSegment2.SelectedItem.Value && x.SEGMENT3 == uxSRSegment3.SelectedItem.Value && x.SEGMENT4 == uxSRSegment4.SelectedItem.Value && x.SEGMENT5 == uxSRSegment5.SelectedItem.Value).Select(x => new { ID = x.SEGMENT6 }).Distinct().OrderBy(x => x.ID);
                   uxSRSegment6Store.DataSource = _data.ToList();
               }

               if (_storeDetails.ID == "uxSRSegment7Store")
               {
                   Entities _context = new Entities();
                   IQueryable<object> _data = _context.GL_ACCOUNTS_V.Where(x => x.SEGMENT1 == uxSRSegment1.SelectedItem.Value && x.SEGMENT2 == uxSRSegment2.SelectedItem.Value && x.SEGMENT3 == uxSRSegment3.SelectedItem.Value && x.SEGMENT4 == uxSRSegment4.SelectedItem.Value && x.SEGMENT5 == uxSRSegment5.SelectedItem.Value && x.SEGMENT6 == uxSRSegment6.SelectedItem.Value).Select(x => new { ID = x.SEGMENT7 }).Distinct().OrderBy(x => x.ID);
                   uxSRSegment7Store.DataSource = _data.ToList();
               }

               if (_storeDetails.ID == "uxERSegment1Store")
               {
                   Entities _context = new Entities();
                   IQueryable<object> _data = _context.GL_ACCOUNTS_V.Select(x => new { ID = x.SEGMENT1 }).Distinct().OrderBy(x => x.ID);
                   uxERSegment1Store.DataSource = _data.ToList();
               }
        }
    }
}
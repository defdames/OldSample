using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DBI.Core.Web;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public partial class umAccountCategory : BasePage
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

        public class ACCOUNT_CATEGORY_LIST : OVERHEAD_ACCOUNT_CATEGORY
        {
            public string NAME { get; set; }
            public string ACCOUNT_SEGMENT_DESC { get; set; }
        }


        protected void uxAccountCategoryStore_ReadData(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                var _data = _context.OVERHEAD_CATEGORY.AsQueryable();

                int count;
                uxAccountCategoryStore.DataSource = GenericData.ListFilterHeader<OVERHEAD_CATEGORY>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _data, out count);
                e.Total = count;
                uxAddCategory.Enable();
            }
        }

        protected void uxAccountListStore_ReadData(object sender, StoreReadDataEventArgs e)
        {
            RowSelectionModel sm = uxAccountCategorySelectionModel;
           long _selectedRowID = long.Parse(sm.SelectedRow.RecordID);
           string _categoryName = e.Parameters["NAME"];
            using (Entities _context = new Entities())
            {
                var _data = _context.OVERHEAD_ACCOUNT_CATEGORY.Where(x => x.CATEGORY_ID == _selectedRowID).Select(x => new ACCOUNT_CATEGORY_LIST { ACCOUNT_CATEGORY_ID = x.ACCOUNT_CATEGORY_ID, CATEGORY_ID = x.CATEGORY_ID, ACCOUNT_SEGMENT = x.ACCOUNT_SEGMENT, ACCOUNT_ORDER = x.ACCOUNT_ORDER, NAME = _categoryName }).AsQueryable();

                //Get the name of the category id and account segment description
                foreach (ACCOUNT_CATEGORY_LIST _record in _data)
                {
                    //Return the segment description
                    GL_ACCOUNTS_V _description = _context.GL_ACCOUNTS_V.Where(x => x.SEGMENT5 == _record.ACCOUNT_SEGMENT).Single();
                    _record.ACCOUNT_SEGMENT_DESC = _description.SEGMENT5_DESC + " (" + _description.SEGMENT5 + ")";
                }
                
                int count;
                uxAccountListStore.DataSource = GenericData.ListFilterHeader<ACCOUNT_CATEGORY_LIST>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _data.AsQueryable(), out count);
                e.Total = count;

            }
        }

        protected void deSelectCategory(object sender, DirectEventArgs e)
        {
            RowSelectionModel _sm = uxAccountCategorySelectionModel;
            if (_sm.SelectedRows.Count > 0)
            {
                uxDeleteCategory.Enable();


                Ext.Net.ParameterCollection ps = new Ext.Net.ParameterCollection();

                Ext.Net.StoreParameter _p = new Ext.Net.StoreParameter();
                _p.Mode = ParameterMode.Value;
                _p.Name = "NAME";
                _p.Value = e.ExtraParams["NAME"];

                ps.Add(_p);

                uxAccountListStore.Reload(ps);

                uxAccountMaintenace.Enable();
            }
            else
            {
                uxDeleteCategory.Disable();
                uxAccountMaintenace.Disable();
            }

        }

        protected void deDeSelectCategory(object sender, DirectEventArgs e)
        {
            RowSelectionModel _sm = uxAccountCategorySelectionModel;
            if (_sm.SelectedRows.Count > 0)
            {
                uxDeleteCategory.Enable();
            }
            else
            {
                uxDeleteCategory.Disable();
                uxAccountMaintenace.Disable();
            }

        }



        protected void deSaveAccountCategory(object sender, DirectEventArgs e)
        {

            if (uxCategoryName.Text == null)
            {
                X.Msg.Alert("Fields Missing", "Category name is required to save this record!").Show();
            }
            else
            {
                OVERHEAD_CATEGORY _record = new OVERHEAD_CATEGORY();
                _record.NAME = uxCategoryName.Text;
                _record.DESCRIPTION = uxCategoryDescription.Text;
                _record.CREATE_DATE = DateTime.Now;
                _record.MODIFY_DATE = DateTime.Now;
                _record.CREATED_BY = User.Identity.Name;
                _record.MODIFIED_BY = User.Identity.Name;
                GenericData.Insert<OVERHEAD_CATEGORY>(_record);

                uxCategoryWindow.Close();
                uxAccountCategoryStore.Reload();
            }
        }

        protected void deDeleteCategory(object sender, DirectEventArgs e)
        {
           RowSelectionModel sm = uxAccountCategorySelectionModel;
           long _selectedRowID = long.Parse(sm.SelectedRow.RecordID);

            //Make sure it's not in use
            using (Entities _context = new Entities())
            {
                List<OVERHEAD_ACCOUNT_CATEGORY> _oac = _context.OVERHEAD_ACCOUNT_CATEGORY.Where(x => x.CATEGORY_ID == _selectedRowID).ToList();
                if (_oac.Count > 0)
                {
                    X.Msg.Alert("Delete Error", "You can not delete this category as it's in use!").Show();
                }
                else
                {
                    OVERHEAD_CATEGORY _record = _context.OVERHEAD_CATEGORY.Where(x => x.CATEGORY_ID == _selectedRowID).SingleOrDefault();
                    GenericData.Delete<OVERHEAD_CATEGORY>(_record);
                    uxCategoryWindow.Close();
                    uxAccountCategoryStore.Reload();
                }

            }

        }

        public class GL_ACCOUNTS_V2 
        {
            public string SEGMENT5 { get; set; }
            public string SEGMENT5_DESC { get; set; }

        }

        protected void uxGLAccountListStore_ReadData(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {


                string sql = @" select distinct segment5 as SEGMENT5, SEGMENT5_DESC AS SEGMENT5_DESC from xxems.gl_accounts_v";
                var _data = _context.Database.SqlQuery<GL_ACCOUNTS_V2>(sql).AsQueryable();
                
                _data = (from dups in _data
                         where !_context.OVERHEAD_ACCOUNT_CATEGORY.Any(x => x.ACCOUNT_SEGMENT == dups.SEGMENT5)
                         select dups);

                int count;
                uxGLAccountListStore.DataSource = GenericData.ListFilterHeader<GL_ACCOUNTS_V2>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _data, out count);
                e.Total = count;
            }
        }

    }
}
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

        protected void deLoadAccounts(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                var _data = _context.GL_ACCOUNTS_V.Select(x => new GL_ACCOUNTS_V2 { CATEGORY_NAME = "", CATEGORY_ID = 0, SEGMENT5 = x.SEGMENT5, SEGMENT5_DESC = x.SEGMENT5_DESC }).Distinct().OrderBy(x => x.SEGMENT5_DESC).ToList();

                int count;
                uxAccountCategoryStore.DataSource = GenericData.EnumerableFilterHeader<GL_ACCOUNTS_V2>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _data, out count);
                e.Total = count;

             
            }
        }

        public class GL_ACCOUNTS_V2 : GL_ACCOUNTS_V
        {
            public string CATEGORY_NAME {get; set;}
            public long CATEGORY_ID {get; set;}
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

        }

        protected void deSelectCategory(object sender, DirectEventArgs e)
        {
            RowSelectionModel _sm = uxAccountCategorySelectionModel;
            if (_sm.SelectedRows.Count > 0)
            {
                uxDeleteCategory.Enable();
                uxAccountListStore.Reload();
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

    }
}
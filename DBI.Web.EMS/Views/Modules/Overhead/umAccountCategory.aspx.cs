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
            public string ACCOUNT_SEGMENT_DESC { get; set; }
        }

        protected void deSaveSortOrder(object sender, DirectEventArgs e)
        {
            string json = e.ExtraParams["Values"];

            List<OVERHEAD_CATEGORY> _gridValues = JSON.Deserialize<List<OVERHEAD_CATEGORY>>(json);

            foreach (OVERHEAD_CATEGORY _detail in _gridValues)
            {
                _detail.MODIFIED_BY = User.Identity.Name;
                _detail.MODIFY_DATE = DateTime.Now;
            }

            GenericData.Update<OVERHEAD_CATEGORY>(_gridValues);
            uxAccountCategoryStore.Sync();
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

            using (Entities _context = new Entities())
            {
                var _data = _context.OVERHEAD_ACCOUNT_CATEGORY.Where(x => x.CATEGORY_ID == _selectedRowID).Select(x => new ACCOUNT_CATEGORY_LIST { ACCOUNT_CATEGORY_ID = x.ACCOUNT_CATEGORY_ID, CATEGORY_ID = x.CATEGORY_ID, ACCOUNT_SEGMENT = x.ACCOUNT_SEGMENT, SORT_ORDER = x.SORT_ORDER }).ToList();

                //Get the name of the category id and account segment description
                foreach (ACCOUNT_CATEGORY_LIST _record in _data)
                {
                    //Return the segment description
                    GL_ACCOUNTS_V _description = _context.GL_ACCOUNTS_V.Where(x => x.SEGMENT5 == _record.ACCOUNT_SEGMENT).First();
                    _record.ACCOUNT_SEGMENT_DESC = _description.SEGMENT5_DESC + " (" + _description.SEGMENT5 + ")";
                }
                
                int count;
                uxAccountListStore.DataSource = GenericData.EnumerableFilterHeader<ACCOUNT_CATEGORY_LIST>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _data, out count);
                e.Total = count;

            }

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
                    OVERHEAD_CATEGORY _record = _context.OVERHEAD_CATEGORY.Where(x => x.CATEGORY_ID == _selectedRowID).SingleOrDefault();
                    GenericData.Delete<OVERHEAD_CATEGORY>(_record);
                    GenericData.Delete<OVERHEAD_ACCOUNT_CATEGORY>(_oac);
                    uxCategoryWindow.Close();
                    uxAccountCategoryStore.Reload();
                    uxAccountListStore.Reload();

            }

            uxDeleteCategory.Disable();
            uxAccountMaintenace.Disable();

        }

        protected void deLoadAccountMaintenance(object sender, DirectEventArgs e)
        {

            RowSelectionModel _sm = uxAccountCategorySelectionModel;

            string url = "umAddGLAccountForCategory.aspx?category_id=" + _sm.SelectedRow.RecordID;

            Window win = new Window
            {
                ID = "uxAccountMaintenanceWindow",
                Title = "Account List",
                Height = 550,
                Width = 600,
                Modal = true,
                Resizable = false,
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

            win.Listeners.Close.Handler = "#{uxAccountListGridPanel}.getStore().load();";

            win.Render(this.Form);
            win.Show();
        }

        protected void deDeleteAccounts(object sender, DirectEventArgs e)
        {
            RowSelectionModel sm = uxAccountListSelectionModel;

            foreach (SelectedRow _row in sm.SelectedRows)
            {
                using(Entities _context = new Entities())
                {
                    long _selectedRecord = long.Parse(_row.RecordID);
                    OVERHEAD_ACCOUNT_CATEGORY _account = _context.OVERHEAD_ACCOUNT_CATEGORY.Where(x => x.ACCOUNT_CATEGORY_ID == _selectedRecord).SingleOrDefault();

                    if (_account != null)
                    {
                        GenericData.Delete<OVERHEAD_ACCOUNT_CATEGORY>(_account);
                    }
                }

            }

            uxAccountListStore.Reload();

        }

    }
}


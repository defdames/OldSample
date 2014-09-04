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
        


        protected void deleteCategory(object sender, DirectEventArgs e)
        {
            RowSelectionModel sm = uxAccountCategorySelectionModel;
            long _selectedRowID = long.Parse(sm.SelectedRow.RecordID);

            //Make sure it's not in use
            using (Entities _context = new Entities())
            {

                OVERHEAD_CATEGORY _category = OVERHEAD_BUDGET_FORECAST.CategoryByID(_context, _selectedRowID);
                List<OVERHEAD_ACCOUNT_CATEGORY> _accounts = OVERHEAD_BUDGET_FORECAST.AccountCategoriesByCategoryID(_context, _selectedRowID).ToList();

                GenericData.Delete<OVERHEAD_CATEGORY>(_category);
                GenericData.Delete<OVERHEAD_ACCOUNT_CATEGORY>(_accounts);
                
                uxCategoryWindow.Close();
                uxAccountCategoryStore.Reload();
                uxAccountListStore.Reload();

            }

            uxDeleteCategory.Disable();
            uxAccountMaintenace.Disable();

        }

        protected void saveAccountCategorySortOrder(object sender, DirectEventArgs e)
        {
            string json = e.ExtraParams["Values"];

            List<OVERHEAD_CATEGORY> _gridValues = JSON.Deserialize<List<OVERHEAD_CATEGORY>>(json);
            int sort = 0;

            foreach (OVERHEAD_CATEGORY _detail in _gridValues)
            {
                _detail.MODIFIED_BY = User.Identity.Name;
                _detail.MODIFY_DATE = DateTime.Now;
                _detail.SORT_ORDER = sort + 1;
                sort = (int)_detail.SORT_ORDER;
            }

            GenericData.Update<OVERHEAD_CATEGORY>(_gridValues);
            uxAccountCategoryStore.Sync();
            uxAccountCategoryStore.Reload();
        }

        protected void uxAccountCategoryStore_ReadData(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                var _data = OVERHEAD_BUDGET_FORECAST.CategoryAsQueryable(_context);
                int count;
                uxAccountCategoryStore.DataSource = GenericData.ListFilterHeader<OVERHEAD_CATEGORY>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _data, out count);
                e.Total = count;
                uxAddCategory.Enable();
            }
        }

        protected void selectCategory(object sender, DirectEventArgs e)
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


        protected void addCategoryAccount(object sender, DirectEventArgs e)
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

        protected void deleteCategoryAccount(object sender, DirectEventArgs e)
        {
            RowSelectionModel sm = uxAccountListSelectionModel;

            foreach (SelectedRow _row in sm.SelectedRows)
            {
                using (Entities _context = new Entities())
                {
                    long _selectedRecord = long.Parse(_row.RecordID);
                    OVERHEAD_ACCOUNT_CATEGORY _account = OVERHEAD_BUDGET_FORECAST.AccountCategoryByID(_context, _selectedRecord);

                    if (_account != null)
                    {
                        GenericData.Delete<OVERHEAD_ACCOUNT_CATEGORY>(_account);
                    }
                }

            }

            uxAccountListStore.Reload();
        }

        protected void uxAccountListStore_ReadData(object sender, StoreReadDataEventArgs e)
        {
           RowSelectionModel sm = uxAccountCategorySelectionModel;
           long _selectedRowID = long.Parse(sm.SelectedRow.RecordID);

            using (Entities _context = new Entities())
            {
                var _data = OVERHEAD_BUDGET_FORECAST.AccountCategoriesByCategoryID(_context, _selectedRowID).Select(x => new OVERHEAD_BUDGET_FORECAST.ACCOUNT_CATEGORY_LIST { ACCOUNT_CATEGORY_ID = x.ACCOUNT_CATEGORY_ID, CATEGORY_ID = x.CATEGORY_ID, ACCOUNT_SEGMENT = x.ACCOUNT_SEGMENT, SORT_ORDER = x.SORT_ORDER, CREATE_DATE = x.CREATE_DATE, CREATED_BY = x.CREATED_BY }).ToList();

                //Get the name of the category id and account segment description
                foreach (OVERHEAD_BUDGET_FORECAST.ACCOUNT_CATEGORY_LIST _record in _data)
                {
                    //Return the segment description
                    string _description = OVERHEAD_BUDGET_FORECAST.AccountDescriptionBySegment(_context, 5, _record.ACCOUNT_SEGMENT);
                    _record.ACCOUNT_SEGMENT_DESC = _description + " (" + _record.ACCOUNT_SEGMENT + ")";
                }
                
                int count;
                uxAccountListStore.DataSource = GenericData.EnumerableFilterHeader<OVERHEAD_BUDGET_FORECAST.ACCOUNT_CATEGORY_LIST>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _data, out count);
                e.Total = count;

            }

        }



        protected void saveAccountCategory(object sender, DirectEventArgs e)
        {

            if (uxCategoryName.Text == null || uxCategoryName.Text.Length <= 1)
            {
                X.Msg.Alert("Fields Missing", "Category name is required to save this record!").Show();
            }
            else
            {
                long? _lastSortOrder = 0;
                //Get max sort order
                using (Entities _context = new Entities())
                {
                    long? _temp = _context.OVERHEAD_CATEGORY.Select(x => x.SORT_ORDER).Max();
                    if (_temp != null)
                        _lastSortOrder = _temp;
                }

                OVERHEAD_CATEGORY _record = new OVERHEAD_CATEGORY();
                _record.NAME = uxCategoryName.Text;
                _record.DESCRIPTION = uxCategoryDescription.Text;
                _record.CREATE_DATE = DateTime.Now;
                _record.MODIFY_DATE = DateTime.Now;
                _record.CREATED_BY = User.Identity.Name;
                _record.MODIFIED_BY = User.Identity.Name;
                _record.SORT_ORDER = _lastSortOrder + 1;
                GenericData.Insert<OVERHEAD_CATEGORY>(_record);

                uxCategoryWindow.Close();
                uxAccountCategoryStore.Reload();
            }
        }

  
    }
}


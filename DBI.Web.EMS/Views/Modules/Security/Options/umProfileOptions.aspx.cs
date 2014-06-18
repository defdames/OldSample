using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DBI.Core.Web;
using Ext.Net;
using DBI.Data;

namespace DBI.Web.EMS.Views.Modules.Security.Options
{
    public partial class umProfileOptions : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void deReadProfileOptions(object sender, StoreReadDataEventArgs e)
        {
            uxProfileOptionStore.RemoveAll();
            uxProfileOptionStore.ClearFilter();
            uxProfileOptionSelectionModel.ClearSelection();

            var data = SYS_PROFILE_OPTIONS.ProfileOptions();

            int count;
            uxProfileOptionStore.DataSource = GenericData.EnumerableFilterHeader<SYS_PROFILE_OPTIONS>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
            e.Total = count;

        }

        protected void deReadProfileOptionValues(object sender, StoreReadDataEventArgs e)
        {

            uxProfileOptionValuesStore.RemoveAll();
            uxProfileOptionValuesStore.ClearFilter();

            RowSelectionModel rs = uxProfileOptionSelectionModel;
            decimal _selectedProfileOption;
            Boolean _check = decimal.TryParse(rs.SelectedRecordID.ToString(), out _selectedProfileOption);

            var data = SYS_USER_PROFILE_OPTIONS.ProfileOptionsByType(_selectedProfileOption);

            int count;
            uxProfileOptionValuesStore.DataSource = GenericData.EnumerableFilterHeader<SYS_USER_PROFILE_OPTIONS.SYS_PROFILE_OPTIONS_V2>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
            e.Total = count;


        }

        protected void deProfileOptionSelection(object sender, DirectEventArgs e)
        {
            try
            {
                uxProfileOptionValuesStore.RemoveAll();
                uxProfileOptionValuesFilter.ClearFilter();
            }
            catch (Exception ex)
            {
                e.Success = false;
                e.ErrorMessage = ex.ToString();
            }

        }
        
       
        protected void deShowAddEditWindow(object sender, DirectEventArgs e)
        {
            long recordID;
            RowSelectionModel selection = uxProfileOptionSelectionModel;
            Boolean tryParse = long.TryParse(selection.SelectedRecordID, out recordID);

            string URL = string.Empty;

            if (recordID == 0)
                {
                    URL = "/Views/Modules/Security/Options/AddEdit/umAddEditProfileOptions.aspx";
                }
                else
                {
                    URL = "/Views/Modules/Security/Options/AddEdit/umAddEditProfileOptions.aspx?recordID=" + recordID;
                }

            Window win = new Window
            {
                ID = "uxAddEditProfileOptionWindow",
                Title = "Add / Edit Profile Option",
                Height = 250,
                Width = 500,
                Modal = true,
                CloseAction = CloseAction.Destroy,
                Loader = new ComponentLoader
                {
                    Mode = LoadMode.Frame,
                    DisableCaching = true,
                    Url = URL,
                    AutoLoad = true,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win.Listeners.Close.Handler = "#{uxProfileOptionGridPanel}.getStore().load();";

            win.Render(this.Form);
            win.Show();
        }

        protected void deDeleteProfileOption(object sender, DirectEventArgs e)
        {
            RowSelectionModel _model = uxProfileOptionSelectionModel;
            long _recordID = long.Parse(_model.SelectedRecordID);
            SYS_PROFILE_OPTIONS.DeleteProfileOption(_recordID);

            uxProfileOptionStore.RemoveAll();
            uxProfileOptionStore.ClearFilter();
            uxProfileOptionStore.Reload();
        }



    }
}
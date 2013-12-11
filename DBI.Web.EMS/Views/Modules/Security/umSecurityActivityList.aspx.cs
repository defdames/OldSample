using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using DBI.Data;
using Ext.Net;
using DBI.Core;

namespace DBI.Web.EMS.Views.Modules.Security
{
    /// <summary>
    /// Security Activity List Page
    /// </summary>
    public partial class umSecurityActivityList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!validateComponentSecurity("SYS.Activities.View")){
                X.Redirect("~/Views/uxDefault.aspx");
            }
            if (!X.IsAjaxRequest)
            {

                if (Request.Cookies["UserSettings"] != null)
                {
                    string RTL;
                    HttpCookie myCookie = new HttpCookie("UserSettings");
                    myCookie = Request.Cookies["UserSettings"];
                    RTL = myCookie["RTL"];
                    //Check for RTL support
                    if (RTL == "True")
                    {
                        uxViewPort.RTL = true;
                    }
                }
            }
        }

        /// <summary>
        /// Data bind system Activitys to the gridpanel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void deActivitysDatabind(object sender, StoreReadDataEventArgs e)
        {
                int total;
                IEnumerable<SYS_ACTIVITY> data = SYS_ACTIVITY.Activities(e.Start, e.Limit, e.Sort, e.Parameters["filter"], out total);
                e.Total = total;
                uxSecurityActivityGridPanel.GetStore().DataSource = data;
        }


        /// <summary>
        /// Deletes a System Activity
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void deDeleteActivity(object sender, DirectEventArgs e)
        {
            CheckboxSelectionModel sm = uxSecurityActivityGridPanel.GetSelectionModel() as CheckboxSelectionModel;
            long Activity_ID = long.Parse(sm.SelectedRow.RecordID);

            SYS_ACTIVITY.Delete(Activity_ID);

            //Clear the selection model reset buttons
            sm.ClearSelection();

            //Refresh the view.
            uxSecurityActivityStore.Reload();
           
        }

        /// <summary>
        /// Edits a system Activity
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void deEditActivity(object sender, DirectEventArgs e)
        {
            CheckboxSelectionModel sm = uxSecurityActivityGridPanel.GetSelectionModel() as CheckboxSelectionModel;
            long Activity_ID = long.Parse(sm.SelectedRow.RecordID);

            bool isNew;
            SYS_ACTIVITY Activity = SYS_ACTIVITY.Activity(Activity_ID, out isNew);
            
            //Show the Window
            uxSecurityAddActivityWindow.Show();

            this.uxSecurityActivityDetails.SetValues(new
            {
                Activity.ACTIVITY_ID,
                Activity.NAME,
                Activity.DESCRIPTION,
                Activity.PATH,
                Activity.CONTAINER,
                Activity.ICON,
                Activity.CONTROL_TEXT
            });

        }

        /// <summary>
        /// Save a system Activity after it has been created or modified.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deSaveActivity(object sender, DirectEventArgs e)
        {

            if (uxName.Text == "" || uxDescription.Text == "")
            {
                X.Msg.Alert("Fields Missing", "Name and Description are required fields!").Show();
            }
            else
            {
                bool isNew;
                long pActivityID;
                bool result = long.TryParse(uxActivityID.Text, out pActivityID);

                SYS_ACTIVITY Activity = SYS_ACTIVITY.Activity(pActivityID, out isNew);
                Activity.NAME = uxName.Text;
                Activity.DESCRIPTION = uxDescription.Text;
                Activity.PATH = uxPath.Text;
                Activity.ICON = uxIcon.Text;
                Activity.CONTROL_TEXT = uxControlText.Text;
                Activity.CONTAINER = uxContainer.Text;

                if (isNew)
                {
                    Activity.CREATED_BY = User.Identity.Name;
                    Activity.CREATED_DATE = SystemTime();
                    Activity.LAST_UPDATED = SystemTime();
                    Activity.LAST_UPDATED_BY = User.Identity.Name;
                }
                else
                {
                    Activity.LAST_UPDATED = SystemTime();
                    Activity.LAST_UPDATED_BY = User.Identity.Name;
                }

                SYS_ACTIVITY.Save(Activity);

                //Refresh the view.
                uxSecurityActivityStore.Reload();

                uxSecurityActivityDetails.Reset();

                //Close the autogenerated window
                uxSecurityAddActivityWindow.Close();

            }

        }

    }
}
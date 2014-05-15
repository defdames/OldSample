using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Ext.Net;
using DBI.Data;


namespace DBI.Web.EMS.Views.Modules.Security.Options.AddEdit
{
    public partial class umAddEditProfileOptions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //test
        }

        protected void deSaveProfileOption(object sender, DirectEventArgs e)
        {
            try
            {
                SYS_PROFILE_OPTIONS profile = new SYS_PROFILE_OPTIONS();
                profile.PROFILE_KEY = uxProfileKey.Text;
                profile.DESCRIPTION = uxProfileDescription.Text;
                profile.CREATE_DATE = DateTime.Now;
                profile.MODIFY_DATE = DateTime.Now;
                profile.CREATED_BY = User.Identity.Name;
                profile.MODIFIED_BY = User.Identity.Name;
                GenericData.Insert<SYS_PROFILE_OPTIONS>(profile);
            }
            catch (Exception ex)
            {
                e.ErrorMessage = "There was an error saving your profile option, please try again";
                throw;
            }

        }

        protected void deDeleteProfileOption(object sender, DirectEventArgs e)
        {


        }
    }
}
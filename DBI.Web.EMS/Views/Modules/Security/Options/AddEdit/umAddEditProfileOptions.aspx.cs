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
            if (!X.IsAjaxRequest)
            {
                //Check for editMode
                if (!string.IsNullOrEmpty(Request.QueryString["recordID"]))
                {
                    long _recordID = long.Parse(Request.QueryString["recordID"].ToString());
                    SYS_PROFILE_OPTIONS _profile = SYS_PROFILE_OPTIONS.profileOptionByRecordID(_recordID);

                    uxProfileKey.Text = _profile.PROFILE_KEY;
                    uxProfileDescription.Text = _profile.DESCRIPTION;
                }
            }
        }

        protected void deSaveProfileOption(object sender, DirectEventArgs e)
        {
            try
            {               
                if (!string.IsNullOrEmpty(Request.QueryString["recordID"]))
                {
                    long _recordID = long.Parse(Request.QueryString["recordID"].ToString());
                    SYS_PROFILE_OPTIONS _profile = SYS_PROFILE_OPTIONS.profileOptionByRecordID(_recordID);
                    _profile.PROFILE_KEY = uxProfileKey.Text;
                    _profile.DESCRIPTION = uxProfileDescription.Text;
                    _profile.MODIFY_DATE = DateTime.Now;
                    _profile.MODIFIED_BY = User.Identity.Name;
                    GenericData.Update<SYS_PROFILE_OPTIONS>(_profile);
                }
                else
                {
                     SYS_PROFILE_OPTIONS _profile = new SYS_PROFILE_OPTIONS();
                    _profile.PROFILE_KEY = uxProfileKey.Text;
                    _profile.DESCRIPTION = uxProfileDescription.Text;
                    _profile.CREATE_DATE = DateTime.Now;
                    _profile.MODIFY_DATE = DateTime.Now;
                    _profile.CREATED_BY = User.Identity.Name;
                    _profile.MODIFIED_BY = User.Identity.Name;
                    GenericData.Insert<SYS_PROFILE_OPTIONS>(_profile);
                }

            }
            catch (Exception ex)
            {
                e.ErrorMessage = "There was an error saving your profile option, please try again";
                throw;
            }

        }



    }
}
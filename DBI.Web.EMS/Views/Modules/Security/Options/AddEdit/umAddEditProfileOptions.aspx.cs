using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Ext.Net;
using DBI.Data;
using System.Data.Entity.Validation;
using System.Text;


namespace DBI.Web.EMS.Views.Modules.Security.Options.AddEdit
{
    public partial class umAddEditProfileOptions : DBI.Core.Web.BasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                //Check for editMode
                if (!string.IsNullOrEmpty(Request.QueryString["recordID"]))
                {
                    long _recordID = long.Parse(Request.QueryString["recordID"].ToString());
                    SYS_PROFILE_OPTIONS _profile = SYS_PROFILE_OPTIONS.ProfileOption(_recordID);

                    uxProfileKey.Text = _profile.PROFILE_KEY;
                    uxProfileDescription.Text = _profile.DESCRIPTION;
                }
            }
        }

        protected void deSaveProfileOption(object sender, DirectEventArgs e)
        {
            try
            {   
                //Validate Form Data
                if (uxProfileKey.Text.Contains(" "))
                {
                    throw new DBICustomException("Profile name must not contain any spaces!");
                }


                if (!string.IsNullOrEmpty(Request.QueryString["recordID"]))
                {
                    long _recordID = long.Parse(Request.QueryString["recordID"].ToString());
                    SYS_PROFILE_OPTIONS _profile = SYS_PROFILE_OPTIONS.ProfileOption(_recordID);

                    //Make sure it doesn't exits in user_profile_options
                    int _cnt = DBI.Data.SYS_USER_PROFILE_OPTIONS.GetCount((long)_profile.PROFILE_OPTION_ID);
                    if (_cnt > 0)
                    {
                        throw new DBICustomException("You can not update this profile option,it is currently in use!");
                    }
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

            catch (DbEntityValidationException dbeve)
            {
                var outputLines = new StringBuilder();
                foreach (var eve in dbeve.EntityValidationErrors)
                {
                    outputLines.AppendFormat("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:"
                      , DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State);

                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.AppendFormat("- Property: \"{0}\", Error: \"{1}\""
                         , ve.PropertyName, ve.ErrorMessage);
                    }
                }

                throw new DBICustomException(outputLines.ToString());
            }

            catch (DBICustomException ex)
            {
                e.Success = false;
                e.ErrorMessage = ex.ToString();
            }

            catch (Exception ex)
            {
                e.Success = false;
                e.ErrorMessage = ex.ToString();
            }

        }



    }
}
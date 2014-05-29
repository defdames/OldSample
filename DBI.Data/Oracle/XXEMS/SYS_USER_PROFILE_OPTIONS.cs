using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DBI.Data
{
    public partial class SYS_USER_PROFILE_OPTIONS
    {
        /// <summary>
        /// Returns a count of profile options by that profile option name.
        /// </summary>
        /// <param name="profile_option_name"></param>
        /// <returns></returns>
        public static int count(long profile_option_id)
        {
            using (Entities _context = new Entities())
            {
                var data = _context.SYS_USER_PROFILE_OPTIONS.Where(x => x.PROFILE_OPTION_ID == profile_option_id).Count();
                return data;
            }
        }


        /// <summary>
        /// Returns a list of user profile options
        /// </summary>
        /// <returns></returns>
        public static List<SYS_USER_PROFILE_OPTIONS_V> userProfileOptions()
        {
            using (Entities _context = new Entities())
            {

                var data = from a in _context.SYS_USER_PROFILE_OPTIONS.Include("SYS_PROFILE_OPTIONS")
                           select new SYS_USER_PROFILE_OPTIONS_V { PROFILE_KEY = a.SYS_PROFILE_OPTIONS.PROFILE_KEY, DESCRIPTION = a.SYS_PROFILE_OPTIONS.DESCRIPTION, USER_PROFILE_OPTION_ID = a.USER_PROFILE_OPTION_ID, PROFILE_VALUE = a.PROFILE_VALUE, USER_ID= a.USER_ID };
                return data.ToList();

            }
        }

        /// <summary>
        /// Returns a selected user profile option by name and user_id
        /// </summary>
        /// <param name="profile_option_name"></param>
        /// <returns></returns>
        public static string userProfileOption(string profile_option_name, long user_id)
        {
            try
            {
                SYS_USER_PROFILE_OPTIONS_V _option = userProfileOptions().Where(x => x.PROFILE_KEY == profile_option_name && x.USER_ID == user_id).SingleOrDefault();
                string _value = string.Empty;
                if (_option != null)
                {
                    _value = _option.PROFILE_VALUE;
                }

                return _value;
            }
            catch (Exception)
            {

                throw;
            }       
        }

        /// <summary>
        /// Returns a selected user profile option by name and logged in user
        /// </summary>
        /// <param name="profile_option_name"></param>
        /// <returns></returns>
        public static string userProfileOption(string profile_option_name)
        {
            try
            {
                SYS_USER_INFORMATION _loggedInUser = SYS_USER_INFORMATION.LoggedInUser();
                SYS_USER_PROFILE_OPTIONS_V _option = userProfileOptions().Where(x => x.PROFILE_KEY == profile_option_name && x.USER_ID == _loggedInUser.USER_ID).SingleOrDefault();
                string _value = string.Empty;
                if (_option != null)
                {
                    _value = _option.PROFILE_VALUE;
                }

                return _value;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void setProfileOption(string profile_option_name, string new_key_value)
        {
            try
            {
                SYS_USER_INFORMATION _loggedInUser = SYS_USER_INFORMATION.LoggedInUser();
                SYS_PROFILE_OPTIONS _option = SYS_PROFILE_OPTIONS.profileOptionByKey(profile_option_name);

                if (_option == null)
                {
                    // Option doesn't exist
                    throw new DBICustomException(string.Format("Can't update profile option {0}. Profile option doesn't exist!", profile_option_name));
                }

                SYS_USER_PROFILE_OPTIONS _user_option;

                using (Entities _context = new Entities())
                {
                    _user_option = _context.SYS_USER_PROFILE_OPTIONS.Where(x => x.PROFILE_OPTION_ID == _option.PROFILE_OPTION_ID && x.USER_ID == _loggedInUser.USER_ID).SingleOrDefault();
                }

                if (_user_option != null)
                {
                    //Perform update
                    _user_option.PROFILE_VALUE = new_key_value;
                    _user_option.MODIFY_DATE = DateTime.Now;
                    _user_option.MODIFIED_BY = _loggedInUser.USER_NAME;
                    _user_option.USER_ID = _loggedInUser.USER_ID;
                    DBI.Data.GenericData.Update<SYS_USER_PROFILE_OPTIONS>(_user_option);
                }
                else
                {
                    //Create new and save
                    _user_option = new SYS_USER_PROFILE_OPTIONS();
                    _user_option.PROFILE_OPTION_ID = _option.PROFILE_OPTION_ID;
                    _user_option.CREATE_DATE = DateTime.Now;
                    _user_option.USER_ID = _loggedInUser.USER_ID;
                    _user_option.MODIFY_DATE = DateTime.Now;
                    _user_option.CREATED_BY = _loggedInUser.USER_NAME;
                    _user_option.MODIFIED_BY = _loggedInUser.USER_NAME;
                    _user_option.PROFILE_VALUE = new_key_value;
                    DBI.Data.GenericData.Insert<SYS_USER_PROFILE_OPTIONS>(_user_option);
                }

            }

            catch (DBICustomException)
            {
                throw;
            }

            catch (Exception)
            {
                throw;
            }
        }


        public static void setProfileOption(string profile_option_name, long user_id, string new_key_value)
        {
            try
            {
                string _loggedInUserName = SYS_USER_INFORMATION.UserByID(user_id).USER_NAME;
                SYS_PROFILE_OPTIONS _option = SYS_PROFILE_OPTIONS.profileOptionByKey(profile_option_name);

                if (_option == null)
                {
                    // Option doesn't exist
                    throw new DBICustomException(string.Format("Can't update profile option {0}. Profile option doesn't exist!", profile_option_name));
                }

                SYS_USER_PROFILE_OPTIONS _user_option;

                using (Entities _context = new Entities())
                {
                    _user_option = _context.SYS_USER_PROFILE_OPTIONS.Where(x => x.PROFILE_OPTION_ID == _option.PROFILE_OPTION_ID && x.USER_ID == user_id).SingleOrDefault();
                }

                if (_user_option != null)
                {
                    //Perform update
                    _user_option.PROFILE_VALUE = new_key_value;
                    _user_option.MODIFY_DATE = DateTime.Now;
                    _user_option.MODIFIED_BY = _loggedInUserName;
                    _user_option.USER_ID = user_id;
                    DBI.Data.GenericData.Update<SYS_USER_PROFILE_OPTIONS>(_user_option);
                }
                else
                {
                    //Create new and save
                    _user_option = new SYS_USER_PROFILE_OPTIONS();
                    _user_option.PROFILE_OPTION_ID = _option.PROFILE_OPTION_ID;
                    _user_option.CREATE_DATE = DateTime.Now;
                    _user_option.USER_ID = user_id;
                    _user_option.MODIFY_DATE = DateTime.Now;
                    _user_option.CREATED_BY = _loggedInUserName;
                    _user_option.MODIFIED_BY = _loggedInUserName;
                    _user_option.PROFILE_VALUE = new_key_value;
                    DBI.Data.GenericData.Insert<SYS_USER_PROFILE_OPTIONS>(_user_option);
                }

            }

            catch (DBICustomException)
            {
                throw;
            }

            catch (Exception)
            {
                throw;
            }
        }



        public class SYS_USER_PROFILE_OPTIONS_V : SYS_USER_PROFILE_OPTIONS
        {
            public string PROFILE_KEY { get; set; }
            public string DESCRIPTION { get; set; }
        }

    }
}

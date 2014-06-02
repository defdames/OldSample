using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data
{
    /// <summary>
    /// Custom Methods and functions over SYS_PROFILE_OPTIONS
    /// </summary>
    public partial class SYS_PROFILE_OPTIONS
    {
         /// <summary>
        /// Returns a list of profile options in the system.
        /// </summary>
        /// <returns></returns>
        public static List<SYS_PROFILE_OPTIONS> List()
        {
            try 
	        {	        
		        using (Entities _context = new Entities())
                    {
                     return _context.SYS_PROFILE_OPTIONS.ToList();
                    }
	        }
	        catch (Exception)
	        {
		        throw;
	        }
        }

        /// <summary>
        /// Deletes a profile option by profile option id
        /// </summary>
        /// <param name="profileOptionId"></param>
        public static void DeleteProfileOption(long profileOptionId)
        {
            try
            {
                SYS_PROFILE_OPTIONS option;
                using (Entities _context = new Entities())
                {
                    option = _context.SYS_PROFILE_OPTIONS.Where(a => a.PROFILE_OPTION_ID == profileOptionId).SingleOrDefault();
                }

                //Make sure it doesn't exits in user_profile_options
                int _cnt = DBI.Data.SYS_USER_PROFILE_OPTIONS.Count((long)option.PROFILE_OPTION_ID);
                if (_cnt > 0)
                {
                    throw new DBICustomException("You can not delete this user profile, it is currently in use!");
                }

                GenericData.Delete<SYS_PROFILE_OPTIONS>(option);
            }
            catch (Exception)
            {  
                throw;
            }
           
        }


        /// <summary>
        /// Returns a profile option by the profile option Id
        /// </summary>
        /// <param name="profileOptionId"></param>
        /// <returns></returns>
        public static SYS_PROFILE_OPTIONS ProfileOptionByProfileOptionId(long profileOptionId)
        {
            try 
            {        
		        using (Entities _context = new Entities())
                    {
                     return _context.SYS_PROFILE_OPTIONS.Where(a => a.PROFILE_OPTION_ID == profileOptionId).SingleOrDefault();
                    }
	        }
	        catch (Exception)
	        {
		        throw;
	        }
        }


        /// <summary>
        /// Returns a profile option by the profile key name.
        /// </summary>
        /// <param name="key_name"></param>
        /// <returns></returns>
        public static SYS_PROFILE_OPTIONS ProfileOptionByKey(string key_name)
        {
            try 
	        {	        
		        using (Entities _context = new Entities())
                 {
                     return _context.SYS_PROFILE_OPTIONS.Where(a => a.PROFILE_KEY == key_name).SingleOrDefault();
                    }
	        }
	        catch (Exception)
	        {
		        
		        throw;
	        }
        }
    }

    /// <summary>
    /// Custom methods and functions over SYS_USER_PROFILE_OPTIONS
    /// </summary>
    public partial class SYS_USER_PROFILE_OPTIONS
    {
        /// <summary>
        /// Returns a count of profile options by the profile option id.
        /// </summary>
        /// <param name="profile_option_name"></param>
        /// <returns></returns>
        public static int Count(long profile_option_id)
        {
            try
            {
                using (Entities _context = new Entities())
                {
                    var data = _context.SYS_USER_PROFILE_OPTIONS.Where(x => x.PROFILE_OPTION_ID == profile_option_id).Count();
                    return data;
                }
            }
            catch (Exception)
            {  
                throw;
            }
            
        }


        /// <summary>
        /// Returns a list of user profile options
        /// </summary>
        /// <returns></returns>
        public static List<SYS_USER_PROFILE_OPTIONS_V> UserProfileOptionsList()
        {
            try
            {
                using (Entities _context = new Entities())
                {
                    var data = from a in _context.SYS_USER_PROFILE_OPTIONS.Include("SYS_PROFILE_OPTIONS")
                               select new SYS_USER_PROFILE_OPTIONS_V { PROFILE_KEY = a.SYS_PROFILE_OPTIONS.PROFILE_KEY, DESCRIPTION = a.SYS_PROFILE_OPTIONS.DESCRIPTION, USER_PROFILE_OPTION_ID = a.USER_PROFILE_OPTION_ID, PROFILE_VALUE = a.PROFILE_VALUE, USER_ID = a.USER_ID };
                    return data.ToList();

                }
            }
            catch (Exception)
            {
                
                throw;
            }
           
        }

        /// <summary>
        /// Returns a user profile option value by name and user_id
        /// </summary>
        /// <param name="profile_option_name"></param>
        /// <returns></returns>
        public static string UserProfileOption(string profileOptionName, long userId)
        {
            try
            {
                SYS_USER_PROFILE_OPTIONS_V _option = UserProfileOptionsList().Where(x => x.PROFILE_KEY == profileOptionName && x.USER_ID == userId).SingleOrDefault();
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
        /// Returns a user profile option by name and user_id
        /// </summary>
        /// <param name="profile_option_name"></param>
        /// <returns></returns>
        public static SYS_USER_PROFILE_OPTIONS UserProfileOption(decimal profileOptionId, long userId)
        {
            try
            {
                using (Entities _context = new Entities())
                {
                    SYS_USER_PROFILE_OPTIONS _option = _context.SYS_USER_PROFILE_OPTIONS.Where(x => x.PROFILE_OPTION_ID == profileOptionId && x.USER_ID == userId).SingleOrDefault();
                    return _option;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Sets a profile option for the logged in user (or by the passed in value by userId) by profile option name and value, if there is no profile option set it will set it the first time else it will update it.
        /// </summary>
        /// <param name="profileOptionName"></param>
        /// <param name="keyValue"></param>
        /// <param name="userId"></param>
        public static void SetProfileOption(string profileOptionName, string keyValue, long userId = 0)
        {
            try
            {
             
                SYS_USER_INFORMATION _loggedInUser = SYS_USER_INFORMATION.LoggedInUser();
                SYS_PROFILE_OPTIONS _option = SYS_PROFILE_OPTIONS.ProfileOptionByKey(profileOptionName);

                if (_option == null)
                {
                    // Option doesn't exist
                    throw new DBICustomException(string.Format("Can't update profile option {0}. Profile option doesn't exist!", profileOptionName));
                }

                SYS_USER_PROFILE_OPTIONS _userOption;

                SYS_USER_INFORMATION _userInformation = new SYS_USER_INFORMATION();
                if (userId != 0)
                {
                    //Return user information by passed in user account
                    _userInformation = SYS_USER_INFORMATION.UserByID(userId);
                    _userOption = UserProfileOption(_option.PROFILE_OPTION_ID, _userInformation.USER_ID);
                }
                else
                {
                    //Return user information by logged in user account
                    _userOption = UserProfileOption(_option.PROFILE_OPTION_ID, _loggedInUser.USER_ID);
                }


                if (_userOption != null)
                {
                    //Perform update
                    _userOption.PROFILE_VALUE = keyValue;
                    _userOption.MODIFY_DATE = DateTime.Now;
                    _userOption.MODIFIED_BY = _loggedInUser.USER_NAME;
                    _userOption.MODIFIED_BY = _loggedInUser.USER_NAME;
                    _userOption.USER_ID = (_userInformation != null) ? _loggedInUser.USER_ID : _userInformation.USER_ID;
                    DBI.Data.GenericData.Update<SYS_USER_PROFILE_OPTIONS>(_userOption);
                }
                else
                {
                    //Create new and save
                    _userOption = new SYS_USER_PROFILE_OPTIONS();
                    _userOption.PROFILE_OPTION_ID = _option.PROFILE_OPTION_ID;
                    _userOption.CREATE_DATE = DateTime.Now;
                    _userOption.USER_ID = (_userInformation != null) ? _loggedInUser.USER_ID : _userInformation.USER_ID;
                    _userOption.MODIFY_DATE = DateTime.Now;
                    _userOption.CREATED_BY = _loggedInUser.USER_NAME;
                    _userOption.MODIFIED_BY = _loggedInUser.USER_NAME;
                    _userOption.PROFILE_VALUE = keyValue;
                    DBI.Data.GenericData.Insert<SYS_USER_PROFILE_OPTIONS>(_userOption);
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

    /// <summary>
    /// Custom methods and functions over OVERHEAD_BUDGET_TYPE Entity Object
    /// </summary>
    public partial class OVERHEAD_BUDGET_TYPE
    {
        /// <summary>
        /// Returns a list of overhead budget types by legal entity
        /// </summary>
        /// <param name="orgID"></param>
        /// <returns></returns>
        public static List<OVERHEAD_BUDGET_TYPE> BudgetTypesByLegalEntity(long legalEntity)
        {
            try
            {
                using (Entities _context = new Entities())
                {
                    List<OVERHEAD_BUDGET_TYPE> _returnList = _context.OVERHEAD_BUDGET_TYPE.Where(c => c.LE_ORG_ID == legalEntity).ToList();
                    return _returnList;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Returns a budget type for a record_id
        /// </summary>
        /// <param name="orgID"></param>
        /// <returns></returns>
        public static OVERHEAD_BUDGET_TYPE BudgetTypeByBudgetTypeId(long budgetTypeId)
        {
            try
            {
                using (Entities _context = new Entities())
                {
                    OVERHEAD_BUDGET_TYPE _returnData = _context.OVERHEAD_BUDGET_TYPE.Where(c => c.OVERHEAD_BUDGET_TYPE_ID == budgetTypeId).SingleOrDefault();
                    return _returnData;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

    }

    /// <summary>
    /// Custom methods and functions over OVERHEAD_GL_ACCOUNT Entity Object
    /// </summary>
    public partial class OVERHEAD_GL_ACCOUNT
    {
        /// <summary>
        /// Returns a list of Overhead GL Accounts by legal entity
        /// </summary>
        /// <param name="legalEntity"></param>
        /// <returns></returns>
        public static List<GL_ACCOUNTS_V2> OverHeadGlAccountsByLegalEntity(long legalEntity)
        {
            try
            {
                using (Entities _context = new Entities())
                {
                    var data = (from gl in _context.OVERHEAD_GL_ACCOUNT.Where(c => c.OVERHEAD_ORG_ID == legalEntity)
                                join gla in _context.GL_ACCOUNTS_V on gl.CODE_COMBO_ID equals gla.CODE_COMBINATION_ID
                                select new GL_ACCOUNTS_V2
                                {
                                    OVERHEAD_GL_ID = (long)gl.OVERHEAD_GL_ID,
                                    CODE_COMBINATION_ID = gla.CODE_COMBINATION_ID,
                                    SEGMENT1 = gla.SEGMENT1,
                                    SEGMENT2 = gla.SEGMENT2,
                                    SEGMENT3 = gla.SEGMENT3,
                                    SEGMENT4 = gla.SEGMENT4,
                                    SEGMENT5 = gla.SEGMENT5,
                                    SEGMENT6 = gla.SEGMENT6,
                                    SEGMENT7 = gla.SEGMENT7,
                                    SEGMENT5_DESC = gla.SEGMENT5_DESC,
                                    SEGMENT1_DESC = gla.SEGMENT1_DESC,
                                    SEGMENT2_DESC = gla.SEGMENT2_DESC,
                                    SEGMENT3_DESC = gla.SEGMENT3_DESC,
                                    SEGMENT4_DESC = gla.SEGMENT4_DESC,
                                    SEGMENT6_DESC = gla.SEGMENT5_DESC,
                                    SEGMENT7_DESC = gla.SEGMENT7_DESC
                                }).ToList();

                    return data;
                }
            }
            catch (Exception)
            {
                
                throw;
            }
           
        }


        public static void DeleteOverHeadAccountByOverheadGlId(long overheadGlId)
        {
            try
            {
                using (Entities _context = new Entities())
                {
                    OVERHEAD_GL_ACCOUNT account = _context.OVERHEAD_GL_ACCOUNT.Where(a => a.OVERHEAD_GL_ID == overheadGlId).SingleOrDefault();
                    GenericData.Delete<OVERHEAD_GL_ACCOUNT>(account);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }

        public static int CountOverheadAccountsByLegalEntity(long legalEntity)
        {
            try
            {
                using (Entities _context = new Entities())
                {
                    int cnt = _context.OVERHEAD_GL_ACCOUNT.Where(a => a.OVERHEAD_ORG_ID == legalEntity).Count();
                    return cnt;
                }
            }
            catch (Exception)
            {
                
                throw;
            }
           
        }

        public class GL_ACCOUNTS_V2 : GL_ACCOUNTS_V
        {
            public long OVERHEAD_GL_ID { get; set; }
        }
        

    }
}

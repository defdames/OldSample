using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBI.Data.Generic;


namespace DBI.Data
{

    #region User Profile Options

    /// <summary>
    /// Custom Methods and functions over SYS_PROFILE_OPTIONS
    /// </summary>
    public partial class SYS_PROFILE_OPTIONS
    {
         /// <summary>
        /// Returns a list of profile options in the system.
        /// </summary>
        /// <returns></returns>
        public static List<SYS_PROFILE_OPTIONS> ProfileOptions()
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
                int _cnt = DBI.Data.SYS_USER_PROFILE_OPTIONS.GetCount((long)option.PROFILE_OPTION_ID);
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
        public static SYS_PROFILE_OPTIONS ProfileOption(long profileOptionId)
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
        public static SYS_PROFILE_OPTIONS ProfileOption(string key_name)
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

    public partial class SYS_ORG_PROFILE_OPTIONS
    {
        #region Organization Profile Options

        /// <summary>
        /// Returns a list of organization profile options
        /// </summary>
        /// <returns></returns>
        public static List<SYS_ORG_PROFILE_OPTIONS_V> OrganizationProfileOptions()
        {

                using (Entities _context = new Entities())
                {
                    var data = from a in _context.SYS_ORG_PROFILE_OPTIONS
                               join b in _context.SYS_PROFILE_OPTIONS on a.PROFILE_OPTION_ID equals b.PROFILE_OPTION_ID
                               select new SYS_ORG_PROFILE_OPTIONS_V { PROFILE_OPTION_ID = b.PROFILE_OPTION_ID, PROFILE_KEY = b.PROFILE_KEY, DESCRIPTION = b.DESCRIPTION, ORG_PROFILE_OPTION_ID = a.ORG_PROFILE_OPTION_ID, PROFILE_VALUE = a.PROFILE_VALUE, ORGANIZATION_ID = a.ORGANIZATION_ID };
                    return data.ToList();

                }
           

        }

        /// <summary>
        /// Returns an organization profile option by profile option name and organization id
        /// </summary>
        /// <param name="profileOptionName"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public static string OrganizationProfileOption(string profileOptionName, long organizationId)
        {

                SYS_ORG_PROFILE_OPTIONS_V _option = OrganizationProfileOptions().Where(x => x.PROFILE_KEY == profileOptionName && x.ORGANIZATION_ID == organizationId).SingleOrDefault();
                string _value = string.Empty;
                if (_option != null)
                {
                    _value = _option.PROFILE_VALUE;
                }
                return _value;
         
        }



        /// <summary>
        /// Returns a profile option by profile option ID and organization
        /// </summary>
        /// <param name="profileOptionId"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public static SYS_ORG_PROFILE_OPTIONS OrganizationProfileOption(decimal profileOptionId, long organizationId)
        {

                using (Entities _context = new Entities())
                {
                    SYS_ORG_PROFILE_OPTIONS _option = _context.SYS_ORG_PROFILE_OPTIONS.Where(x => x.PROFILE_OPTION_ID == profileOptionId && x.ORGANIZATION_ID == organizationId).SingleOrDefault();
                    return _option;
                }
         
        }

        /// <summary>
        /// Sets a profile option for an organization level.
        /// </summary>
        /// <param name="profileOptionName"></param>
        /// <param name="keyValue"></param>
        /// <param name="organizationId"></param>
        public static void SetOrganizationProfileOption(string profileOptionName, string keyValue, long organizationId)
        {

                SYS_PROFILE_OPTIONS _option = SYS_PROFILE_OPTIONS.ProfileOption(profileOptionName);
                SYS_USER_INFORMATION _loggedInUser = SYS_USER_INFORMATION.LoggedInUser();

                if (_option == null)
                {
                    // Option doesn't exist
                    throw new DBICustomException(string.Format("Can't update profile option {0}. Profile option doesn't exist!", profileOptionName));
                }

                SYS_ORG_PROFILE_OPTIONS _profileOption = OrganizationProfileOption(_option.PROFILE_OPTION_ID, organizationId);

                if (_profileOption != null)
                {
                    //Perform update
                    _profileOption.PROFILE_VALUE = keyValue;
                    _profileOption.MODIFY_DATE = DateTime.Now;
                    _profileOption.MODIFIED_BY = _loggedInUser.USER_NAME;
                    _profileOption.MODIFIED_BY = _loggedInUser.USER_NAME;
                    _profileOption.ORGANIZATION_ID = organizationId;
                    DBI.Data.GenericData.Update<SYS_ORG_PROFILE_OPTIONS>(_profileOption);
                }
                else
                {
                    //Create new and save
                    _profileOption = new SYS_ORG_PROFILE_OPTIONS();
                    _profileOption.PROFILE_OPTION_ID = _option.PROFILE_OPTION_ID;
                    _profileOption.CREATE_DATE = DateTime.Now;
                    _profileOption.ORGANIZATION_ID = organizationId;
                    _profileOption.MODIFY_DATE = DateTime.Now;
                    _profileOption.CREATED_BY = _loggedInUser.USER_NAME;
                    _profileOption.MODIFIED_BY = _loggedInUser.USER_NAME;
                    _profileOption.PROFILE_VALUE = keyValue;
                    DBI.Data.GenericData.Insert<SYS_ORG_PROFILE_OPTIONS>(_profileOption);
                }
            


        }

        public class SYS_ORG_PROFILE_OPTIONS_V : SYS_ORG_PROFILE_OPTIONS
        {
            public string PROFILE_KEY { get; set; }
            public string DESCRIPTION { get; set; }

        }

        #endregion
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
        public static int GetCount(long profile_option_id)
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
        public static List<SYS_USER_PROFILE_OPTIONS_V> UserProfileOptions()
        {

                using (Entities _context = new Entities())
                {
                    var data = from a in _context.SYS_USER_PROFILE_OPTIONS.Include("SYS_PROFILE_OPTIONS")
                               select new SYS_USER_PROFILE_OPTIONS_V { PROFILE_OPTION_ID = a.SYS_PROFILE_OPTIONS.PROFILE_OPTION_ID, PROFILE_KEY = a.SYS_PROFILE_OPTIONS.PROFILE_KEY, DESCRIPTION = a.SYS_PROFILE_OPTIONS.DESCRIPTION, USER_PROFILE_OPTION_ID = a.USER_PROFILE_OPTION_ID, PROFILE_VALUE = a.PROFILE_VALUE, USER_ID = a.USER_ID };
                    return data.ToList();

                }
            
           
        }

        /// <summary>
        /// Returns a user profile option value by name and user_id
        /// </summary>
        /// <param name="profile_option_name"></param>
        /// <returns></returns>
        public static string UserProfileOption(string profileOptionName, long userId)
        {

                SYS_USER_PROFILE_OPTIONS_V _option = UserProfileOptions().Where(x => x.PROFILE_KEY == profileOptionName && x.USER_ID == userId).SingleOrDefault();
                string _value = string.Empty;
                if (_option != null)
                {
                    _value = _option.PROFILE_VALUE;
                }
                return _value;
           
        }

        /// <summary>
        /// Returns a user profile option value by name and logged in user account
        /// </summary>
        /// <param name="profile_option_name"></param>
        /// <returns></returns>
        public static string UserProfileOption(string profileOptionName)
        {

                SYS_USER_INFORMATION _user = SYS_USER_INFORMATION.LoggedInUser();
                SYS_USER_PROFILE_OPTIONS_V _option = UserProfileOptions().Where(x => x.PROFILE_KEY == profileOptionName && x.USER_ID == _user.USER_ID).SingleOrDefault();
                string _value = string.Empty;
                if (_option != null)
                {
                    _value = _option.PROFILE_VALUE;
                }
                return _value;
           
        }

        /// <summary>
        /// Returns a user profile option by name and user_id
        /// </summary>
        /// <param name="profile_option_name"></param>
        /// <returns></returns>
        public static SYS_USER_PROFILE_OPTIONS UserProfileOption(decimal profileOptionId, long userId)
        {

                using (Entities _context = new Entities())
                {
                    SYS_USER_PROFILE_OPTIONS _option = _context.SYS_USER_PROFILE_OPTIONS.Where(x => x.PROFILE_OPTION_ID == profileOptionId && x.USER_ID == userId).SingleOrDefault();
                    return _option;
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

                SYS_USER_INFORMATION _loggedInUser = SYS_USER_INFORMATION.LoggedInUser();
                SYS_PROFILE_OPTIONS _option = SYS_PROFILE_OPTIONS.ProfileOption(profileOptionName);

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

        public class SYS_USER_PROFILE_OPTIONS_V : SYS_USER_PROFILE_OPTIONS
        {
            public string PROFILE_KEY { get; set; }
            public string DESCRIPTION { get; set; }
        }

    #endregion
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
        public static List<OVERHEAD_BUDGET_TYPE> BudgetTypes(long legalEntityOrganizationId)
        {

                using (Entities _context = new Entities())
                {
                    var sql = string.Format(@"select * from xxems.overhead_budget_type 
                                where le_org_id = {0}
                                start with parent_budget_type_id is null
                                connect by prior overhead_budget_type_id = parent_budget_type_id",legalEntityOrganizationId);

                    List<OVERHEAD_BUDGET_TYPE> _returnList = _context.Database.SqlQuery<OVERHEAD_BUDGET_TYPE>(sql).ToList();
                    return _returnList;
                }
            

        }

        /// <summary>
        /// Returns a budget type for a record_id
        /// </summary>
        /// <param name="orgID"></param>
        /// <returns></returns>
        public static OVERHEAD_BUDGET_TYPE BudgetType(long budgetTypeId)
        {

                using (Entities _context = new Entities())
                {
                    OVERHEAD_BUDGET_TYPE _returnData = _context.OVERHEAD_BUDGET_TYPE.Where(c => c.OVERHEAD_BUDGET_TYPE_ID == budgetTypeId).SingleOrDefault();
                    return _returnData;
                }
           

        }

    }

    /// <summary>
    /// Custom methods and functions over OVERHEAD_GL_ACCOUNT Entity Object
    /// </summary>
    public partial class OVERHEAD_GL_ACCOUNT
    {

        public static IQueryable<OVERHEAD_GL_ACCOUNT> OverheadGLAccountsByOrganization(Entities _context, long organizationId)
        {
            IQueryable<OVERHEAD_GL_ACCOUNT> _data = _context.OVERHEAD_GL_ACCOUNT.Where(x => x.ORGANIZATION_ID == organizationId);
            return _data;
        }



        /// <summary>
        /// Returns a list of Overhead GL Accounts by legal entity
        /// </summary>
        /// <param name="legalEntity"></param>
        /// <returns></returns>
        //public static List<GL_ACCOUNTS_V2> AccountsByLegalEntity(long legalEntityOrganizationId)
        //{

        //        using (Entities _context = new Entities())
        //        {
        //            var data = (from gl in _context.OVERHEAD_GL_ACCOUNT.Where(c => c.OVERHEAD_ORG_ID == legalEntityOrganizationId)
        //                        join gla in _context.GL_ACCOUNTS_V on gl.CODE_COMBO_ID equals gla.CODE_COMBINATION_ID
        //                        select new GL_ACCOUNTS_V2
        //                        {
        //                            OVERHEAD_GL_ID = (long)gl.OVERHEAD_GL_ID,
        //                            CODE_COMBINATION_ID = gla.CODE_COMBINATION_ID,
        //                            SEGMENT1 = gla.SEGMENT1,
        //                            SEGMENT2 = gla.SEGMENT2,
        //                            SEGMENT3 = gla.SEGMENT3,
        //                            SEGMENT4 = gla.SEGMENT4,
        //                            SEGMENT5 = gla.SEGMENT5,
        //                            SEGMENT6 = gla.SEGMENT6,
        //                            SEGMENT7 = gla.SEGMENT7,
        //                            SEGMENT5_DESC = gla.SEGMENT5_DESC,
        //                            SEGMENT1_DESC = gla.SEGMENT1_DESC,
        //                            SEGMENT2_DESC = gla.SEGMENT2_DESC,
        //                            SEGMENT3_DESC = gla.SEGMENT3_DESC,
        //                            SEGMENT4_DESC = gla.SEGMENT4_DESC,
        //                            SEGMENT6_DESC = gla.SEGMENT5_DESC,
        //                            SEGMENT7_DESC = gla.SEGMENT7_DESC
        //                        }).ToList();

        //            return data;
        //        }
            
           
        //}

        /// <summary>
        /// Allows the user to delete an overhead Gl account that has been assigned.
        /// </summary>
        /// <param name="overheadGlId"></param>
        //public static void Delete(long overheadGlId)
        //{

        //        using (Entities _context = new Entities())
        //        {
        //            OVERHEAD_GL_ACCOUNT account = _context.OVERHEAD_GL_ACCOUNT.Where(a => a.OVERHEAD_GL_ID == overheadGlId).SingleOrDefault();
        //            GenericData.Delete<OVERHEAD_GL_ACCOUNT>(account);
        //        }
           
            
        //}

        /// <summary>
        /// Returns a count of how many OVERHEAD_GL_ACCOUNTS are assigned by an organization Id
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        //public static int GetCount(long organizationId)
        //{

        //        using (Entities _context = new Entities())
        //        {
        //            int cnt = _context.OVERHEAD_GL_ACCOUNT.Where(a => a.OVERHEAD_ORG_ID == organizationId).Count();
        //            return cnt;
        //        }
           
           
        //}

        public class GL_ACCOUNTS_V2 : GL_ACCOUNTS_V
        {
            public long OVERHEAD_GL_ID { get; set; }
        }
        

    }

    public partial class TIME_CLOCK
    {

        /// <summary>
        /// Returns all employee time.
        /// </summary>
        /// <returns></returns>
        public static List<Employee> EmployeeTime()
        {

                using (Entities _context = new Entities())
                {

                    
                    var _data = (from tc in _context.TIME_CLOCK
                                 join ev in _context.EMPLOYEES_V on tc.PERSON_ID equals ev.PERSON_ID
                                 select new Employee
                                 {
                                     TIME_IN = ((DateTime)tc.MODIFIED_TIME_IN == null) ? tc.TIME_IN : tc.MODIFIED_TIME_IN,
                                     TIME_OUT = ((DateTime)tc.MODIFIED_TIME_OUT == null) ? tc.TIME_OUT : tc.MODIFIED_TIME_OUT,
                                     EMPLOYEE_NAME = ev.EMPLOYEE_NAME,
                                     DAY_OF_WEEK = tc.DAY_OF_WEEK,
                                     TIME_CLOCK_ID = tc.TIME_CLOCK_ID,
                                     ADJUSTED_HOURS = tc.ADJUSTED_HOURS,
                                     ACTUAL_HOURS = tc.ACTUAL_HOURS,
                                     SUBMITTED = tc.SUBMITTED,
                                     APPROVED = tc.APPROVED,
                                     COMPLETED = tc.COMPLETED,
                                     SUPERVISOR_ID = (int)tc.SUPERVISOR_ID,
                                     //MODIFIED_TIME_IN = (DateTime)tc.MODIFIED_TIME_IN,
                                     //MODIFIED_TIME_OUT = (DateTime)tc.MODIFIED_TIME_OUT,
                                     MODIFY_DATE = (DateTime)tc.MODIFY_DATE,
                                     MODIFIED_BY = tc.MODIFIED_BY,
                                     DELETED = tc.DELETED,
                                     DELETED_COMMENTS = tc.DELETED_COMMENTS
                                 }).ToList();
                    return _data;
                }
            

        }


        /// <summary>
        /// Returns all employee time by supervisior Id that has been completed by the user and is NOT approved
        /// </summary>
        /// <param name="supervisorId"></param>
        /// <returns></returns>
        public static List<Employee> EmployeeTimeCompletedUnapproved(decimal supervisorId)
        {

                var _data = EmployeeTime().Where(x => x.SUPERVISOR_ID == supervisorId && x.COMPLETED  == "Y" && x.APPROVED == "N" && x.DELETED =="N").ToList();
                return _data;
           

        }
        /// <summary>
        /// Returns all employee time that has been completed and NOT approved
        /// </summary>
        /// <returns></returns>
        public static List<Employee> EmployeeTimeCompletedUnapprovedPayroll()
        {

                var _data = EmployeeTime().Where(x => x.COMPLETED == "Y" && x.APPROVED == "N" && x.DELETED == "N").ToList();
                return _data;
            
        }
        /// <summary>
        /// Returns all employee time that has been completed by supervisor ID
        /// </summary>
        /// <param name="supervisorId"></param>
        /// <returns></returns>
        public static List<Employee> EmployeeTimeCompleted(decimal supervisorId)
        {

                var _data = EmployeeTime().Where(x => x.SUPERVISOR_ID == supervisorId && x.COMPLETED == "Y" && x.DELETED == "N").ToList();
                return _data;
            

        }
        /// <summary>
        /// Reuturns all employee time that has been completed
        /// </summary>
        /// <returns></returns>
        public static List<Employee> EmployeeTimeCompletedPayroll()
        {
                var _data = EmployeeTime().Where(x => x.COMPLETED == "Y" && x.DELETED == "N").ToList();
                return _data;
          

        }
        /// <summary>
        /// Returns all Employee time that has been completed and approved
        /// </summary>
        /// <returns></returns>
        public static List<Employee> EmployeeTimeCompletedApprovedPayroll()
        {

            var _data = EmployeeTime().Where(x => x.COMPLETED == "Y" && x.APPROVED == "Y" && x.SUBMITTED == "N" && x.DELETED == "N").ToList();
            return _data;
            
        }

        public static List<Employee> EmployeeTimeCompletedApprovedSubmittedPayroll()
        {
            var _data = EmployeeTime().Where(x => x.COMPLETED == "Y" && x.APPROVED == "Y" && x.DELETED == "N").ToList();
            return _data;
        }

        public static DateTime ManagerDateInEditScreen(decimal tcID)
        {
            
                DateTime? _data = EmployeeTime().Where(x => x.TIME_CLOCK_ID == tcID).SingleOrDefault().TIME_IN;
                return (DateTime)_data;
        }

        public static TimeSpan ManagerTimeInEditScreen(decimal tcID)
        {

            DateTime? _data = EmployeeTime().Where(x => x.TIME_CLOCK_ID == tcID).SingleOrDefault().TIME_IN;
            TimeSpan ts = _data.Value.TimeOfDay;
            
            return ts;
        }

        public static DateTime ManagerDateOutEditScreen(decimal tcID)
        {

            DateTime? _data = EmployeeTime().Where(x => x.TIME_CLOCK_ID == tcID).SingleOrDefault().TIME_OUT;
            return (DateTime)_data;
        }

        public static TimeSpan ManagerTimeOutEditScreen(decimal tcID)
        {

            DateTime? _data = EmployeeTime().Where(x => x.TIME_CLOCK_ID == tcID).SingleOrDefault().TIME_OUT;
            TimeSpan ts = _data.Value.TimeOfDay;

            return ts;
        }

        /// <summary>
        /// Updates TIME CLOCK table with new time edited by Manager or Payroll Manager
        /// </summary>
        /// <param name="tcID"></param>
        /// <param name="newTimeIn"></param>
        /// <param name="newTimeOut"></param>
        /// <param name="personName"></param>

        public static void InsertEditedEmployeeTime(decimal tcID, DateTime newTimeIn, DateTime newTimeOut, string personName)
        {

            TIME_CLOCK _data;
            using (Entities _context = new Entities())
            {
                TimeSpan ts = newTimeOut - newTimeIn;
                decimal adjts = ConvertTimeToOraclePayrollFormat(ts);


                _data = _context.TIME_CLOCK.Where(x => x.TIME_CLOCK_ID == tcID).SingleOrDefault();
                _data.ACTUAL_HOURS = (decimal)ts.TotalHours;
                _data.ADJUSTED_HOURS = adjts;
                _data.MODIFIED_TIME_IN = newTimeIn;
                _data.MODIFIED_TIME_OUT = newTimeOut;
                _data.MODIFIED_BY = personName;
                _data.MODIFY_DATE = DateTime.Now;

            }

            DBI.Data.GenericData.Update<TIME_CLOCK>(_data);
        }

        /// <summary>
        /// Marks a flag on the TIMECLOCK table that a time was record was deleted.  Said flg will hide record from all screens
        /// </summary>
        /// <param name="tcId"></param>
        /// <param name="comment"></param>
        /// <param name="personName"></param>
        public static void DeleteEmployeeTime(decimal tcID, string comment, string personName)
        {
            TIME_CLOCK _data;
            using (Entities _context = new Entities())
            {
                _data = _context.TIME_CLOCK.Where(x => x.TIME_CLOCK_ID == tcID).SingleOrDefault();
                _data.DELETED = "Y";
                _data.DELETED_COMMENTS = comment;
                _data.MODIFIED_BY = personName;
                _data.MODIFY_DATE = DateTime.Now;

            }
            DBI.Data.GenericData.Update<TIME_CLOCK>(_data);
        }

        /// <summary>
        /// Approves Employee time so payroll can submit
        /// </summary>
        /// <param name="selection"></param>
        public static void EmployeeTimeSelectionApproved(List<TIME_CLOCK> selection)
        {

                foreach (TIME_CLOCK selected in selection)
                {

                    TIME_CLOCK _data;
                    using (Entities _context = new Entities())
                    {
                        _data = _context.TIME_CLOCK.Where(x => x.COMPLETED == "Y" && x.TIME_CLOCK_ID == selected.TIME_CLOCK_ID).SingleOrDefault();

                        _data.APPROVED = "Y";
                    }
                    DBI.Data.GenericData.Update<TIME_CLOCK>(_data);
                }
           
        }

        public static void EmployeeTimeSelectionSubmitted(List<TIME_CLOCK> selection)
        {
            foreach (TIME_CLOCK selected in selection)
            {
                TIME_CLOCK _data;
                using (Entities _context = new Entities())
                {
                    _data = _context.TIME_CLOCK.Where(x => x.APPROVED == "Y" && x.TIME_CLOCK_ID == selected.TIME_CLOCK_ID).SingleOrDefault();
                    _data.SUBMITTED = "Y";
                }
                DBI.Data.GenericData.Update <TIME_CLOCK>(_data);
            }
        }

        /// <summary>
        /// ADjusts time to be  for oracle payroll
        /// </summary> Adjust time to nearest quarter of hour and store in table
        /// <param name="adjts"></param>
        /// <returns></returns>
        public static decimal ConvertTimeToOraclePayrollFormat(TimeSpan adjts)
        {
            
            double adjtime = (adjts.Minutes > 0 && adjts.Minutes <= 8) ? 0
                         : (adjts.Minutes > 8 && adjts.Minutes <= 23) ? .25
                         : (adjts.Minutes > 23 && adjts.Minutes <= 38) ? .50
                         : (adjts.Minutes > 38 && adjts.Minutes <= 53) ? .75
                         : (adjts.Minutes > 53 && adjts.Minutes <= 60) ? 1
                         : 0;

            decimal fixedtime = adjts.Hours + (decimal)adjtime;
            return fixedtime;
        }

       

      

        

        public class Employee : TIME_CLOCK
        {
            public decimal TIME_CLOCK_ID { get; set; }
            public string EMPLOYEE_NAME { get; set; }
            public DateTime? TIME_IN { get; set; }
            public DateTime? TIME_OUT { get; set; }
            public string ADJUSTED_HOURS_GRID { get; set; }
            public string DAY_OF_WEEK { get; set; }
            public string ACTUAL_HOURS_GRID { get; set; }
            public decimal? ACTUAL_HOURS { get; set; }
            public decimal? ADJUSTED_HOURS { get; set; }
            public string APPROVED { get; set; }
            public string SUBMITTED { get; set; }
            public int SUPERVISOR_ID { get; set; }
            public string COMPLETED { get; set; }
            public string MODIFIED_BY { get; set; }
            public DateTime? MODIFIED_TIME_IN { get; set; }
            public DateTime? MODIFIED_TIME_OUT { get; set; }
            public DateTime? MODIFY_DATE { get; set; }
            public string DELETED { get; set; }
            public string DELETED_COMMENTS { get; set; }
            public string TIME_DIFF { get; set; }

         
            
        }

    }


    public class XXEMS
    {


        /// <summary>
        /// Returns a list of all profile options, user and organization and their values
        /// </summary>
        /// <returns></returns>
        public static List<SYS_PROFILE_OPTIONS_V2> ProfileOptionsByType(decimal profileOptionId)
        {

                List<DBI.Data.SYS_USER_PROFILE_OPTIONS.SYS_USER_PROFILE_OPTIONS_V> _userProfileOptions = SYS_USER_PROFILE_OPTIONS.UserProfileOptions().Where(x => x.PROFILE_OPTION_ID == profileOptionId).ToList();
                List<DBI.Data.SYS_ORG_PROFILE_OPTIONS.SYS_ORG_PROFILE_OPTIONS_V> _orgProfileOptions = SYS_ORG_PROFILE_OPTIONS.OrganizationProfileOptions().Where(x => x.PROFILE_OPTION_ID == profileOptionId).ToList();

                List<SYS_PROFILE_OPTIONS_V2> _options = new List<SYS_PROFILE_OPTIONS_V2>();

                foreach (var userOption in _userProfileOptions)
                {
                    SYS_PROFILE_OPTIONS_V2 _option = new SYS_PROFILE_OPTIONS_V2();
                    _option.PROFILE_OPTION_TYPE = "User";
                    _option.PROFILE_OWNER_NAME = SYS_USER_INFORMATION.UserByID(userOption.USER_ID).USER_NAME;
                    _option.USER_PROFILE_OPTION_ID = userOption.USER_PROFILE_OPTION_ID;
                    _option.PROFILE_VALUE = userOption.PROFILE_VALUE;
                    _options.Add(_option);
                }

                foreach (var orgOption in _orgProfileOptions)
                {
                    SYS_PROFILE_OPTIONS_V2 _option = new SYS_PROFILE_OPTIONS_V2();
                    _option.PROFILE_OPTION_TYPE = "Organization";
                    _option.PROFILE_OWNER_NAME = HR.Organization(orgOption.ORGANIZATION_ID).ORGANIZATION_NAME;
                    _option.USER_PROFILE_OPTION_ID = orgOption.ORG_PROFILE_OPTION_ID;
                    _option.PROFILE_VALUE = orgOption.PROFILE_VALUE;
                    _options.Add(_option);
                }

                return _options;
           

        }

        public static IQueryable<PROJECTS_V> ProjectsByOrgHierarchy(List<long> OrgsList, Entities _context)
        {
            return _context.PROJECTS_V.Where(x => OrgsList.Contains(x.CARRYING_OUT_ORGANIZATION_ID));
        }

        public class SYS_PROFILE_OPTIONS_V2
        {
            public decimal USER_PROFILE_OPTION_ID { get; set; }
            public decimal ORG_PROFILE_OPTION_ID { get; set; }
            public string PROFILE_OPTION_TYPE { get; set; }
            public string PROFILE_VALUE { get; set; }
            public string PROFILE_OWNER_NAME { get; set; }
        }
    }

    public partial class CROSSINGS
    {
        public static void UpdateServiceUnits()
        {
            try
            {
                List<CROSSING> _crossings = new List<CROSSING>();
                using (Entities _context = new Entities())
                {
                    _crossings = _context.CROSSINGS.Where(x => x.MODIFIED_DATE == null).Take(5000).ToList();
                }

                //GMS Data
                List<DBI.Data.GMS.ServiceUnitResponse> _gmsData = DBI.Data.GMS.ServiceUnitData.ServiceUnits();


                foreach (var _crossing in _crossings)
                {
                    DBI.Data.GMS.ServiceUnitResponse _data = _gmsData.Where(x => x.sub_division.ToUpper() == _crossing.SUB_DIVISION).FirstOrDefault();

                    if (_data != null)
                    {

                        System.Diagnostics.Debug.WriteLine(_crossing.CROSSING_ID);
                        _crossing.SERVICE_UNIT = _data.service_unit;
                        _crossing.MODIFIED_DATE = DateTime.Now;
                        DBI.Data.GenericData.Update<CROSSING>(_crossing);
                    }
                    else
                    {
                        _crossing.MODIFIED_DATE = DateTime.Now;
                        DBI.Data.GenericData.Update<CROSSING>(_crossing);
                    }

                }
            }
            catch (Oracle.ManagedDataAccess.Types.OracleTruncateException ex)
            {

                throw (ex);
            }
           
        }
    }

}


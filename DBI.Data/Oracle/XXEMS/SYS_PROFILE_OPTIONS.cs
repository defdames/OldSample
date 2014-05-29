using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data
{
    public partial class SYS_PROFILE_OPTIONS
    {
        /// <summary>
        /// Returns a list of profile options in the system.
        /// </summary>
        /// <returns></returns>
        public static List<SYS_PROFILE_OPTIONS> systemProfileOptions()
        {
            using (Entities _context = new Entities())
            {
                return _context.SYS_PROFILE_OPTIONS.ToList();
            }
        }

        public static void deleteProfileOptionByRecordID(long recordID)
        {
            try
            {
                SYS_PROFILE_OPTIONS option;
                using (Entities _context = new Entities())
                {
                    option = _context.SYS_PROFILE_OPTIONS.Where(a => a.PROFILE_OPTION_ID == recordID).SingleOrDefault();
                }

                //Make sure it doesn't exits in user_profile_options
                int _cnt = DBI.Data.SYS_USER_PROFILE_OPTIONS.count((long)option.PROFILE_OPTION_ID);
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

        public static SYS_PROFILE_OPTIONS profileOptionByRecordID(long recordID)
        {
            using (Entities _context = new Entities())
            {
                return _context.SYS_PROFILE_OPTIONS.Where(a => a.PROFILE_OPTION_ID == recordID).SingleOrDefault();
            }

        }

        /// <summary>
        /// Returns a profile option by the profile key name.
        /// </summary>
        /// <param name="key_name"></param>
        /// <returns></returns>
        public static SYS_PROFILE_OPTIONS profileOptionByKey(string key_name)
        {
            using (Entities _context = new Entities())
            {
                return _context.SYS_PROFILE_OPTIONS.Where(a => a.PROFILE_KEY == key_name).SingleOrDefault();
            }

        }
    }
}

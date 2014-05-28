using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data
{
    public partial class SYS_USER_PROFILE_OPTIONS
    {





        /// <summary>
        /// Returns a list of user profile options
        /// </summary>
        /// <returns></returns>
        public static List<SYS_USER_PROFILE_OPTIONS_V> userProfileOptions()
        {
            using (Entities _context = new Entities())
            {

                var data = from a in _context.SYS_USER_PROFILE_OPTIONS
                           join b in _context.SYS_PROFILE_OPTIONS on a.PROFILE_OPTION_ID equals b.PROFILE_OPTION_ID
                           select new SYS_USER_PROFILE_OPTIONS_V { PROFILE_KEY = b.PROFILE_KEY, DESCRIPTION = b.DESCRIPTION, USER_PROFILE_OPTION_ID = a.USER_PROFILE_OPTION_ID, PROFILE_VALUE = a.PROFILE_VALUE };
                return data.ToList();

            }
        }


        public class SYS_USER_PROFILE_OPTIONS_V : SYS_USER_PROFILE_OPTIONS
        {
            public string PROFILE_KEY { get; set; }
            public string DESCRIPTION { get; set; }
        }

    }
}

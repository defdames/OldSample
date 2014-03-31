using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data
{
    public partial class SYS_USER_ORGS
    {
        public static bool IsInOrg(long UserId, long OrgId)
        {
            using (Entities _context = new Entities())
            {
                SYS_USER_ORGS OrgResult = _context.SYS_USER_ORGS.Where(x => (x.USER_ID == UserId) && (x.ORG_ID == OrgId)).SingleOrDefault();

                if (OrgResult != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<SYS_USER_ORGS> GetUserOrgs(long UserId)
        {
            using (Entities _context = new Entities())
            {
                List<SYS_USER_ORGS> UserOrgs = _context.SYS_USER_ORGS.Where(x => x.USER_ID == UserId).ToList();
                return UserOrgs;
            }
        }
    }
}

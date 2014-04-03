using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace DBI.Data
{
    public partial class SYS_PERMISSIONS
    {
        public static List<SYS_PERMISSIONS> GetGroupPermissions(long JobId)
        {
            using (Entities _context = new Entities())
            {
                List<SYS_PERMISSIONS> Permissions = (from g in _context.SYS_GROUPS
                                                     join gp in _context.SYS_GROUPS_PERMS on g.GROUP_ID equals gp.GROUP_ID
                                                     join p in _context.SYS_PERMISSIONS on gp.PERMISSION_ID equals p.PERMISSION_ID
                                                     where g.JOB_ID == JobId
                                                     select p).ToList();
                return Permissions;
            }
        }

        public static List<SYS_PERMISSIONS> GetPermissionsHierarchy()
        {
            using (Entities _context = new Entities())
            {
                string Query = "select permission_id, permission_name, parent_perm_id, description from xxems.sys_permissions connect by prior permission_id = parent_perm_id start with permission_id=1";
                List<SYS_PERMISSIONS> Permissions = _context.Database.SqlQuery<SYS_PERMISSIONS>(Query).ToList();

                return Permissions;
            }
        }

        public static List<Claim> Claims(string username)
        {
            using (Entities _context = new Entities())
            {
                SYS_USER_INFORMATION userInfo = _context.Set<SYS_USER_INFORMATION>().Where(u => u.USER_NAME.Equals(username.ToUpper())).SingleOrDefault();
                List<Claim> claims = new List<Claim>();

                if (userInfo != null)
                {
                    List<SYS_PERMISSIONS> Permissions = GetPermissions(userInfo.USER_ID);

                    foreach (SYS_PERMISSIONS Permission in Permissions)
                    {
                        if (Permission.PARENT_PERM_ID == 1)
                        {
                            List<SYS_PERMISSIONS> ChildPermissions = (_context.SYS_PERMISSIONS.Where(x => x.PARENT_PERM_ID == Permission.PERMISSION_ID)).ToList();
                            foreach (SYS_PERMISSIONS ChildPermission in ChildPermissions)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, ChildPermission.PERMISSION_NAME));
                            }
                        }
                        else
                        {
                            claims.Add(new Claim(ClaimTypes.Role, Permission.PERMISSION_NAME));
                        }
                    }

                    // Add a claim for the username
                    claims.Add(new Claim(ClaimTypes.Name, username.ToUpper()));

                    // Add full name of user to the claims 
                    claims.Add(new Claim("EmployeeName", userInfo.EMPLOYEE_NAME));
                    claims.Add(new Claim("EmployeeNumber", userInfo.EMPLOYEE_NUMBER.ToString()));
                    //Add current organization ID to claims
                    claims.Add(new Claim("CurrentOrgId", userInfo.CURRENT_ORG_ID.ToString()));

                    claims.Add(new Claim("UserId", userInfo.USER_ID.ToString()));
                }
                return claims;
            }
        }

        public static List<SYS_PERMISSIONS> GetPermissions(long UserId)
        {
            long JobId = SYS_USER_INFORMATION.GetJobId(UserId);

            List<SYS_PERMISSIONS> GroupPermissions = GetGroupPermissions(JobId);
            List<SYS_USER_PERMS> UserPermissions = DBI.Data.SYS_USER_PERMS.GetUserPermissions(UserId);

            foreach (SYS_USER_PERMS UserPermission in UserPermissions)
            {
                if (UserPermission.ALLOW_DENY == "D")
                {
                    GroupPermissions.RemoveAt(GroupPermissions.FindIndex(x => x.PERMISSION_ID == UserPermission.PERMISSION_ID));
                }
                else
                {
                    GroupPermissions.Add(UserPermission.SYS_PERMISSIONS);
                }

            }

            return GroupPermissions;
        }
    }
}

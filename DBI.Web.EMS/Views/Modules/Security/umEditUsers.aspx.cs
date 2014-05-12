using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IdentityModel.Tokens;
using System.IdentityModel.Services;
using System.Security.Claims;
using DBI.Core.Web;
using DBI.Core.Security;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.Security
{
    public partial class umEditUsers : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected List<SYS_PERMISSIONS> GetPermissions()
        {
            using (Entities _context = new Entities())
            {
                return _context.SYS_PERMISSIONS.ToList();
            }
        }

        protected void deReadUsers(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                List<SYS_USER_INFORMATION> UserList = _context.SYS_USER_INFORMATION.ToList();
                int count;
                uxUsersStore.DataSource = GenericData.EnumerableFilterHeader<SYS_USER_INFORMATION>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], UserList, out count);
                e.Total = count;
            }
        }

        protected void deLoadEditUserForm(object sender, DirectEventArgs e)
        {
            long UserId = long.Parse(e.ExtraParams["UserId"]);
            uxEditUserStore.DataSource = GetPermissions();
            uxEditUserStore.DataBind();

            uxEditUserWindow.Show();

            var UserPermissions = SYS_PERMISSIONS.GetPermissions(UserId);
            CheckboxSelectionModel GridModel = uxEditUserGrid.GetSelectionModel() as CheckboxSelectionModel;
            GridModel.DeselectAll();
            GridModel.UpdateSelection();
            foreach (var UserPermission in UserPermissions)
            {
                GridModel.SelectedRows.Add(new SelectedRow(UserPermission.PERMISSION_NAME));
               
            }
            GridModel.UpdateSelection();
        }

        protected void deUpdateUserPermissions(object sender, DirectEventArgs e)
        {
            long UserId = long.Parse(e.ExtraParams["UserId"]);
            long JobId = SYS_USER_INFORMATION.GetJobId(UserId);
            List<SYS_PERMISSIONS> SelectedPermissions = JSON.Deserialize<List<SYS_PERMISSIONS>>(e.ExtraParams["Rows"]);
            List<SYS_PERMISSIONS> GroupPermissions = SYS_PERMISSIONS.GetGroupPermissions(JobId);
            List<SYS_USER_PERMS> UserPermissions = SYS_USER_PERMS.GetUserPermissions(UserId);
            List<SYS_PERMISSIONS> AllPermissions = GetPermissions();
            foreach (SYS_PERMISSIONS SelectedPermission in SelectedPermissions)
            {
                if (UserPermissions.Exists(x => (x.PERMISSION_ID == SelectedPermission.PERMISSION_ID) && (x.ALLOW_DENY == "D")))
                {
                    SYS_USER_PERMS ToDelete;
                    using (Entities _context = new Entities())
                    {
                        ToDelete = _context.SYS_USER_PERMS.Where(x => (x.PERMISSION_ID == SelectedPermission.PERMISSION_ID) && x.USER_ID == UserId).Single();
                    }
                    GenericData.Delete<SYS_USER_PERMS>(ToDelete);
                }
                else if (!GroupPermissions.Exists(x => x.PERMISSION_ID == SelectedPermission.PERMISSION_ID) && !UserPermissions.Exists(x => x.PERMISSION_ID == SelectedPermission.PERMISSION_ID))
                {
                    //add userpermission entry as it's not in the group
                    SYS_USER_PERMS PermissionToAdd = new SYS_USER_PERMS
                    {
                        PERMISSION_ID = SelectedPermission.PERMISSION_ID,
                        USER_ID = UserId,
                        ALLOW_DENY = "A",
                    };
                    GenericData.Insert<SYS_USER_PERMS>(PermissionToAdd);
                }
                AllPermissions.Remove(AllPermissions.Find(x =>x.PERMISSION_ID == SelectedPermission.PERMISSION_ID));
            }
            foreach (SYS_PERMISSIONS NotSelectedPermission in AllPermissions)
            {
                if (GroupPermissions.Exists(x => x.PERMISSION_ID == NotSelectedPermission.PERMISSION_ID)  &&!UserPermissions.Exists(x => x.PERMISSION_ID == NotSelectedPermission.PERMISSION_ID))
                {
                    //add deny
                    SYS_USER_PERMS ToDeny = new SYS_USER_PERMS
                    {
                        PERMISSION_ID = NotSelectedPermission.PERMISSION_ID,
                        USER_ID = UserId,
                        ALLOW_DENY = "D"
                    };
                    GenericData.Insert<SYS_USER_PERMS>(ToDeny);
                }
                else if (UserPermissions.Exists(x => (x.PERMISSION_ID == NotSelectedPermission.PERMISSION_ID)&&(x.ALLOW_DENY == "A")))
                {
                    //remove allow
                    SYS_USER_PERMS ToDelete;
                    using (Entities _context = new Entities())
                    {
                        ToDelete = _context.SYS_USER_PERMS.Where(x => (x.PERMISSION_ID == NotSelectedPermission.PERMISSION_ID) && (x.USER_ID == UserId)).Single();
                    }
                    GenericData.Delete<SYS_USER_PERMS>(ToDelete);
                }
            }
            uxEditUserWindow.Hide();
        }

        public void deImpersonate(object sender, DirectEventArgs e)
        {
            //Get the user information
            //Get the selected user id
            long userID = long.Parse(e.ExtraParams["UserId"]);

            SYS_USER_INFORMATION userDetails = SYS_USER_INFORMATION.UserByID(userID);

            if (userDetails != null)
            {

                List<Claim> claims = SYS_PERMISSIONS.Claims(userDetails.USER_NAME);

                int cnt = claims.Count;
                // Always add the username, this is always required
                claims.Add(new Claim(ClaimTypes.Name, userDetails.USER_NAME));

                // Add a claim to say they were impersonated and by who impersonated them
                claims.Add(new Claim("ImpersonatedUser", userDetails.EMPLOYEE_NAME));

                if (Authentication.GetClaimValue("ImpersonatorUsername", User as ClaimsPrincipal) == string.Empty)
                {
                    claims.Add(new Claim("ImpersonatorUsername", HttpContext.Current.User.Identity.Name));
                }
                if (Authentication.GetClaimValue("ImpersonatorName", User as ClaimsPrincipal) == string.Empty)
                {
                    claims.Add(new Claim("ImpersonatorName", Authentication.GetClaimValue("EmployeeName", User as ClaimsPrincipal)));
                }
               

                var id = new ClaimsIdentity(claims, "Forms");
                var cp = new ClaimsPrincipal(id);

                var token = new SessionSecurityToken(cp);
                var sam = FederatedAuthentication.SessionAuthenticationModule;
                sam.WriteSessionTokenToCookie(token);

                // Break out of frames and redirect user.
                ResourceManager.GetInstance().AddScript("parent.window.location = '{0}';", "../../uxDefault.aspx");
            }
        }

        protected void deLoadUpdateOrgWindow(object sender, DirectEventArgs e)
        {

            using (Entities _context = new Entities())
            {
                var AllOrgs = _context.ORG_HIER_V.Select(x => new { x.ORG_HIER, x.ORG_ID }).Distinct().ToList();
                long UserId = long.Parse(e.ExtraParams["UserId"]);

                var SelectedOrgs = (from s in _context.SYS_USER_ORGS
                                    join o in _context.ORG_HIER_V on s.ORG_ID equals o.ORG_ID
                                    where s.USER_ID == UserId
                                    select new { o.ORG_ID, o.ORG_HIER }).Distinct().ToList();

                foreach (var SelectedOrg in SelectedOrgs)
                {
                    AllOrgs.RemoveAt(AllOrgs.FindIndex(x => x.ORG_ID == SelectedOrg.ORG_ID));
                }


                this.uxAvailableOrgsStore.DataSource = AllOrgs;
                this.uxAvailableOrgsStore.DataBind();
                uxSelectedOrgsStore.DataSource = SelectedOrgs;
                uxSelectedOrgsStore.DataBind();

                uxTwoGridWindow.Show();
            }
        }

        protected void deSaveUserOrgs(object sender, DirectEventArgs e)
        {
            List<ORG_HIER_V> NotSelectedOrgs = JSON.Deserialize<List<ORG_HIER_V>>(e.ExtraParams["NotSelectedOrgs"]);
            List<ORG_HIER_V> SelectedOrgs = JSON.Deserialize<List<ORG_HIER_V>>(e.ExtraParams["SelectedOrgs"]);
            List<SYS_USER_ORGS> UserOrgs;
            long UserId = long.Parse(e.ExtraParams["UserId"]);

            using (Entities _context = new Entities())
            {
                UserOrgs = _context.SYS_USER_ORGS.Where(x => x.USER_ID == UserId).ToList();
            }
            if (SelectedOrgs.Count > 0)
            {
                foreach (ORG_HIER_V SelectedOrg in SelectedOrgs)
                {
                    if (!UserOrgs.Exists(x => x.ORG_ID == SelectedOrg.ORG_ID))
                    {
                        SYS_USER_ORGS NewUserOrg = new SYS_USER_ORGS
                        {
                            ORG_ID = SelectedOrg.ORG_ID,
                            USER_ID = UserId
                        };
                        GenericData.Insert<SYS_USER_ORGS>(NewUserOrg);
                    }
                }
            }
            if (NotSelectedOrgs.Count > 0)
            {
                foreach (ORG_HIER_V NotSelectedOrg in NotSelectedOrgs)
                {
                    if (UserOrgs.Exists(x => x.ORG_ID == NotSelectedOrg.ORG_ID))
                    {
                        SYS_USER_ORGS ToBeDeleted;
                        using (Entities _context = new Entities())
                        {
                            ToBeDeleted = _context.SYS_USER_ORGS.Where(x => (x.ORG_ID == NotSelectedOrg.ORG_ID) && (x.USER_ID == UserId)).Single();
                        }
                        GenericData.Delete<SYS_USER_ORGS>(ToBeDeleted);
                    }
                }
            }

            Notification.Show(new NotificationConfig()
            {
                Title = "Success",
                Html = "User organizations updated successfully.",
                HideDelay = 1000,
                AlignCfg = new NotificationAlignConfig
                {
                    ElementAnchor = AnchorPoint.Center,
                    TargetAnchor = AnchorPoint.Center
                }
            });
            uxTwoGridWindow.Hide();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Claims;
using DBI.Core.Web;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.Security
{
    public partial class umEditOrgs : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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
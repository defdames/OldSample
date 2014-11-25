using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Security;
using DBI.Core.Web;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views
{
    /// <summary>
    /// Default View with buttons and where modules are loaded
    /// </summary>
    public partial class uxDefault : DBI.Core.Web.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {   
            if (!X.IsAjaxRequest && !IsPostBack)
            {
                // Look for cookie information for language support
                if (Request.Cookies["UserSettings"] != null)
                {
                    string RTL;
                    HttpCookie myCookie = new HttpCookie("UserSettings");
                    myCookie = Request.Cookies["UserSettings"];
                    RTL = myCookie["RTL"];
                    //Check for RTL support
                    if (RTL == "True")
                    {
                        uxViewPort.RTL = true;
                    }
                }

               
                // Get Impersonating Info/Details
                string user = Authentication.GetClaimValue("ImpersonatedUser", User as ClaimsPrincipal);
                string byuser = Authentication.GetClaimValue("ImpersonatorName", User as ClaimsPrincipal);

                uxWelcomeTime.Text = string.Format("Today is {0}", DateTime.Now.ToString("D"));

                if (user != string.Empty)
                {
                    //todo Internationalize(currently only in English) because of string.Format
                    uxWelcomeName.Text = string.Format("Impersonating User {0} by ({1})", user, byuser);
                    uxWelcomeName.CtCls = "header-actions-button-red";
                    uxWelcomeName.Disabled = false;
                }
                else
                {
                    //Display Welcome message to user
                    uxWelcomeName.Text = string.Format("Welcome {0}", Authentication.GetClaimValue("EmployeeName", User as ClaimsPrincipal));
                    uxWelcomeName.CtCls = "header-actions-button-orange";
                    uxWelcomeName.Disabled = true;
                    XXEMS.LogUserActivity(User.Identity.Name);
                }
            }
            GenerateMenuItems(User as ClaimsPrincipal);
        }

       

        /// <summary>
        /// Direct Event that loads a panel given 2 ext.net extra parameters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Extra Parameters Page(which panel to load), and Location(where on the page to load it)</param>
        protected void deLoadPage(object sender, DirectEventArgs e)
        {
            //if (long.Parse(Session["isDirty"].ToString()) == 0)
            //{
                uxWest.Collapse();
            //    LoadModule(e.ExtraParams["Page"], e.ExtraParams["Location"]);
            //}
            //else
            //{
            //   CreateDirtyMessage();
            //}
        }

        /// <summary>
        /// Removes the impersonate ability from the user, and resets their security back to their user account.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deRemoveImpersonate(object sender, DirectEventArgs e)
        {
            if (Authentication.GetClaimValue("ImpersonatorUsername", User as ClaimsPrincipal) != null)
            {

                SYS_USER_INFORMATION userDetails = SYS_USER_INFORMATION.UserByUserName(Authentication.GetClaimValue("ImpersonatorUsername", User as ClaimsPrincipal));

                List<Claim> claims = SYS_PERMISSIONS.Claims(userDetails.USER_NAME);

                // Add full name of user to the claims 
                claims.Add(new Claim("EmployeeName", Authentication.GetClaimValue("EmployeeName", User as ClaimsPrincipal)));
                var id = new ClaimsIdentity(claims, "Forms");
                var cp = new ClaimsPrincipal(id);

                var token = new SessionSecurityToken(cp);
                var sam = FederatedAuthentication.SessionAuthenticationModule;
                sam.WriteSessionTokenToCookie(token);


                // Break out of frames and redirect user.
                ResourceManager.GetInstance().AddScript("parent.window.location = '{0}';", "uxDefault.aspx");
            }

        }

        //todo Eventually move to Role contains default(but overrideable) Activities
        /// <summary>
        /// Generates the Menu Items for a logged in user based on all roles assigned to user in claims
        /// </summary>
        public void GenerateMenuItems(ClaimsPrincipal icp)
        {
            long UserId = long.Parse(Authentication.GetClaimValue("UserId", icp));

            List<SYS_PERMISSIONS> Permissions = SYS_PERMISSIONS.GetPermissions(UserId);

            
            List<SYS_PERMISSIONS> PermissionsHierarchy = SYS_PERMISSIONS.GetPermissionsHierarchy();

            if (Permissions.Exists(x => x.PERMISSION_ID == 1))
            {
                //SuperUser, so load all modules and menu items
                List<SYS_MODULES> ModulesList = SYS_MODULES.GetModules().OrderBy(x => x.SORT_ORDER).ToList();
                foreach (SYS_MODULES Module in ModulesList)
                {
                    Ext.Net.MenuPanel AppPanel = CreateMenu(Module);
                    List<SYS_MENU> MenuItems = SYS_MENU.GetMenuItems(Module.MODULE_ID).OrderBy(x => x.SORT_ORDER).ToList();
                    foreach (SYS_MENU MenuItem in MenuItems)
                    {
                        Ext.Net.MenuItem AppMenuItem = CreateMenuItem(MenuItem);
                        AppMenuItem.Icon = (MenuItem.ICON == null ? Icon.None : (Icon)Enum.Parse(typeof(Icon), MenuItem.ICON));
                        AppPanel.Menu.Items.Add(AppMenuItem);
                    }
                    uxWest.Items.Add(AppPanel);
                }
                
            }
            else
            {
                    List<SYS_PERMISSIONS> SecondLevelPermissions = PermissionsHierarchy.FindAll(x => x.PARENT_PERM_ID == 1);
                    foreach (SYS_PERMISSIONS SecondLevelPermission in SecondLevelPermissions)
                    {
                        if (Permissions.Exists(x => x.PERMISSION_ID == SecondLevelPermission.PERMISSION_ID))
                        {
                            //App Admin, so load all menu items for module
                            SYS_MODULES Module = SYS_MODULES.GetModules(SecondLevelPermission.PERMISSION_ID);
                            Ext.Net.MenuPanel AppPanel = CreateMenu(Module);
                            List<SYS_MENU> MenuItems = SYS_MENU.GetMenuItems(Module.MODULE_ID);
                            foreach (SYS_MENU MenuItem in MenuItems)
                            {
                                Ext.Net.MenuItem AppMenuItem = CreateMenuItem(MenuItem);
                                AppMenuItem.Icon = (MenuItem.ICON == null ? Icon.None : (Icon)Enum.Parse(typeof(Icon), MenuItem.ICON));
                                AppPanel.Menu.Items.Add(AppMenuItem);
                            }
                            uxWest.Items.Add(AppPanel);
                            
                        }
                        else
                        {
                            if (Permissions.Exists(x => x.PARENT_PERM_ID == SecondLevelPermission.PERMISSION_ID))
                            {
                                SYS_MODULES Module = SYS_MODULES.GetModules(SecondLevelPermission.PERMISSION_ID);
                                Ext.Net.MenuPanel AppPanel;
                                decimal ModuleId = 0;

                                ModuleId = Module.MODULE_ID;
                                AppPanel = CreateMenu(Module);
                                
                                List<SYS_PERMISSIONS> ThirdLevelPermissions = PermissionsHierarchy.FindAll(x => x.PARENT_PERM_ID == SecondLevelPermission.PERMISSION_ID);
                                foreach (SYS_PERMISSIONS ThirdLevelPermission in ThirdLevelPermissions)
                                {
                                    if (Permissions.Exists(x => x.PERMISSION_ID == ThirdLevelPermission.PERMISSION_ID))
                                    {
                                        
                                        //Has App Permission, load this module
                                        List<SYS_MENU> MenuItems = SYS_MENU.GetMenuItems(ModuleId, ThirdLevelPermission.PERMISSION_ID);
                                        foreach (SYS_MENU MenuItem in MenuItems)
                                        {
                                            Ext.Net.MenuItem AppMenuItem = CreateMenuItem(MenuItem);
                                            AppMenuItem.Icon = (MenuItem.ICON == null ? Icon.None : (Icon)Enum.Parse(typeof(Icon), MenuItem.ICON));
                                            AppPanel.Menu.Items.Add(AppMenuItem);
                                        }
                                        
                                    }
                                }
                                uxWest.Items.Add(AppPanel);
                            }
                        }
                    
                }
            }
            //List<SYS_ACTIVITY> userActivities;

            //if(!User.IsInRole("SYS.Administrator"))
            //{
            //    //Get all roles from claims
            //    ClaimsIdentity claimsIdentity = (ClaimsIdentity)icp.Identity;

            //    List<string> AssignedRoles = (from c in claimsIdentity.Claims
            //                                  where c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
            //                                  select c.Value).ToList();
            //    //Get Button config from server
            //    Entities context = new Entities();
            //    userActivities = (from s in context.SYS_ACTIVITY
            //                        where AssignedRoles.Contains(s.NAME) && s.PATH != null
            //                        select s).OrderBy(x => x.SORT_NUMBER).ToList();
            //}
            //else
            //{
            //    Entities context = new Entities();
            //    userActivities = (from s in context.SYS_ACTIVITY
            //                        where !(string.IsNullOrEmpty(s.PATH))
            //                        select s).OrderBy(x => x.PARENT_ITEM_ID).OrderBy(x => x.SORT_NUMBER).ToList();
            //}
            ////Iterate through allowed activities
            //foreach (SYS_ACTIVITY userActivity in userActivities)
            //{
            //    if (userActivity.PARENT_ITEM_ID != null)
            //    {
            //        Ext.Net.MenuItem AppMenuItem = new Ext.Net.MenuItem()
            //        {
            //            ID = "uxMenuItem" + userActivity.ACTIVITY_ID.ToString(),
            //            Text = userActivity.CONTROL_TEXT,
            //            Icon = (Icon)Enum.Parse(typeof(Icon), userActivity.ICON)
            //        };

            //        //Add click DirectEvent
            //        AppMenuItem.DirectEvents.Click.Event += deLoadPage;

            //        //Add DirectEvent Parameters
            //        AppMenuItem.DirectEvents.Click.ExtraParams.Add(new Ext.Net.Parameter()
            //        {
            //            Name = "Location",
            //            Value = userActivity.CONTAINER
            //        });
            //        AppMenuItem.DirectEvents.Click.ExtraParams.Add(new Ext.Net.Parameter()
            //        {
            //            Name = "Page",
            //            Value = userActivity.PATH
            //        });
            //        Ext.Net.MenuPanel AppPanel = X.GetCmp("uxMenu" + userActivity.PARENT_ITEM_ID.ToString()) as Ext.Net.MenuPanel;
            //        AppPanel.Menu.Items.Add(AppMenuItem);
            //    }
            //    else
            //    {
            //        Ext.Net.MenuPanel AppPanel = new MenuPanel()
            //        {
            //            ID= "uxMenu" + userActivity.ACTIVITY_ID.ToString(),
            //            Title= userActivity.CONTROL_TEXT,
            //            Icon = (Icon)Enum.Parse(typeof(Icon), userActivity.ICON),
            //        };
            //        Ext.Net.MenuItem AppMenuItem = new Ext.Net.MenuItem()
            //        {
            //            ID = "uxMenuItem" + userActivity.ACTIVITY_ID.ToString(),
            //            Text = "Home",
            //            Icon = (Icon)Enum.Parse(typeof(Icon), userActivity.ICON),
                        
            //        };
            //        AppMenuItem.DirectEvents.Click.Event += deLoadPage;

            //        //Add DirectEvent Parameters
            //        AppMenuItem.DirectEvents.Click.ExtraParams.Add(new Ext.Net.Parameter()
            //        {
            //            Name = "Location",
            //            Value = userActivity.CONTAINER
            //        });
            //        if (userActivity.NAME == "SYS.EMSv1.View")
            //        {
            //            byte[] UserArray = System.Text.ASCIIEncoding.ASCII.GetBytes(User.Identity.Name.ToUpper());
            //            string EncodedUser =Convert.ToBase64String(UserArray);
            //            AppMenuItem.DirectEvents.Click.ExtraParams.Add(new Ext.Net.Parameter()
            //            {
            //                Name = "Page",
            //                Value = userActivity.PATH + "/Redirect.aspx?user=" + EncodedUser
            //            });
            //        }
            //        else
            //        {
            //            AppMenuItem.DirectEvents.Click.ExtraParams.Add(new Ext.Net.Parameter()
            //            {
            //                Name = "Page",
            //                Value = userActivity.PATH
            //            });
            //        }
            //        uxWest.Items.Add(AppPanel);
            //        AppPanel.Menu.Items.Add(AppMenuItem);
            //    }
            //}
        }

        protected Ext.Net.MenuPanel CreateMenu(SYS_MODULES Module)
        {
            Ext.Net.MenuPanel AppPanel = new MenuPanel()
            {
                ID = "uxMenu" + Module.MODULE_ID.ToString(),
                Title = Module.MODULE_NAME,
            };
            return AppPanel;
        }

        protected Ext.Net.MenuItem CreateMenuItem(SYS_MENU MenuItem)
        {
            Ext.Net.MenuItem AppMenuItem = new Ext.Net.MenuItem()
            {
                ID = "uxMenuItem" + MenuItem.MENU_ID.ToString(),
                Text = MenuItem.ITEM_NAME,
            };

            //Add click DirectEvent
            AppMenuItem.DirectEvents.Click.Event += deLoadPage;

            //Add DirectEvent Parameters
            AppMenuItem.DirectEvents.Click.ExtraParams.Add(new Ext.Net.Parameter()
            {
                Name = "Location",
                Value = "uxCenter"
            });

            if (MenuItem.ITEM_URL == "http://ems.dbiservices.com")
            {
                byte[] UserArray = System.Text.ASCIIEncoding.ASCII.GetBytes(User.Identity.Name.ToUpper());
                string EncodedUser = Convert.ToBase64String(UserArray);
                AppMenuItem.DirectEvents.Click.ExtraParams.Add(new Ext.Net.Parameter()
                {
                    Name = "Page",
                    Value = MenuItem.ITEM_URL + "/Redirect.aspx?user=" + EncodedUser
                });
            }
            else if (MenuItem.ITEM_URL == "http://emstest.dbiservices.com")
            {
                byte[] UserArray = System.Text.ASCIIEncoding.ASCII.GetBytes(User.Identity.Name.ToUpper());
                string EncodedUser = Convert.ToBase64String(UserArray);
                AppMenuItem.DirectEvents.Click.ExtraParams.Add(new Ext.Net.Parameter()
                {
                    Name = "Page",
                    Value = MenuItem.ITEM_URL + "/Redirect.aspx?user=" + EncodedUser
                });
            }
            else
            {
                AppMenuItem.DirectEvents.Click.ExtraParams.Add(new Ext.Net.Parameter()
                {
                    Name = "Page",
                    Value = MenuItem.ITEM_URL
                });
            }

            return AppMenuItem;
        }
    }
}
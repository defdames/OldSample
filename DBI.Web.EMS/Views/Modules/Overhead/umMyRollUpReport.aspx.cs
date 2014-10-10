using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public partial class umMyRollUpReport : DBI.Core.Web.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (!validateComponentSecurity("SYS.OverheadBudget.Report.User.Rollup"))
                {
                    X.Redirect("~/Views/uxDefault.aspx");
                }
            }
        }

        [DirectMethod]
        public void AddTabPanel(string id, string title, string url)
        {

            Ext.Net.Panel pan = new Ext.Net.Panel();

            pan.ID = "Tab" + id.Replace(":", "_");
            pan.Title = title;
            pan.CloseAction = CloseAction.Destroy;
            pan.Closable = true;
            pan.Loader = new ComponentLoader();
            pan.Loader.ID = "loader" + id.Replace(":", "_");
            pan.Loader.Url = url;
            pan.Loader.Mode = LoadMode.Frame;
            pan.Loader.LoadMask.ShowMask = true;
            pan.Loader.DisableCaching = true;
            pan.AddTo(uxCenterTabPanel);

            uxCenterTabPanel.SetActiveTab("Tab" + id.Replace(":", "_"));
        }


        protected void deLoadOrgTree(object sender, NodeLoadEventArgs e)
        {

            // Add LE
            if (e.NodeID == "0")
            {

                //Get a list of organizations assigned to my username.
                List<SYS_USER_ORGS> _userOrgs = SYS_USER_ORGS.GetUserOrgs(SYS_USER_INFORMATION.LoggedInUser().USER_ID);

                List<string> _leList = new List<string>();

                //Return the LE's I have access to view
                foreach(SYS_USER_ORGS _org in _userOrgs)
                {
                    string _leID = OVERHEAD_BUDGET_FORECAST.ReturnLEIDforOrganizationID(_org.ORG_ID).ToString();
                    _leList.Add(_leID);
                }


                var data = HR.LegalEntityHierarchies().Select(a => new { a.ORGANIZATION_ID, a.ORGANIZATION_NAME }).Where(x => _leList.Contains(x.ORGANIZATION_ID.ToString())).GroupBy(grp => new { grp.ORGANIZATION_ID, grp.ORGANIZATION_NAME }).ToList();


                //Build the treepanel
                foreach (var view in data)
                {

                    //Only show for the profile option OverheadBudgetHierarchy

                    string _profileValue = SYS_ORG_PROFILE_OPTIONS.OrganizationProfileOption("OverheadBudgetHierarchy", view.Key.ORGANIZATION_ID);

                    if (_profileValue.Length > 0)
                    {
                        long _profileLong = long.Parse(_profileValue);
                        var _hierData = HR.LegalEntityHierarchies().Where(a => a.ORGANIZATION_ID == view.Key.ORGANIZATION_ID & a.ORGANIZATION_STRUCTURE_ID == _profileLong).ToList();

                        //Check for incomplete setup 
                        int _cnt = OVERHEAD_BUDGET_TYPE.BudgetTypes(view.Key.ORGANIZATION_ID).Count();


                        //Create the Hierarchy Levels
                        Node node = new Node();
                        node.Text = view.Key.ORGANIZATION_NAME;
                        node.NodeID = string.Format("{0}:{1}", _profileValue.ToString(), view.Key.ORGANIZATION_ID.ToString());
                        node.Leaf = false;
                        node.Icon = Icon.Building;
                        e.Nodes.Add(node);
                    }
                }

            }

            // Add orgs
            else
            {
                List<string> _leList = new List<string>();

                //Get a list of organizations assigned to my username.
                List<SYS_USER_ORGS> _userOrgs = SYS_USER_ORGS.GetUserOrgs(SYS_USER_INFORMATION.LoggedInUser().USER_ID);

                //Return the LE's I have access to view
                foreach(SYS_USER_ORGS _org in _userOrgs)
                {
                    _leList.Add(_org.ORG_ID.ToString());
                }

                char[] delimChars = { ':' };
                string[] selID = e.NodeID.Split(delimChars);
                long hierarchyID = long.Parse(selID[0].ToString());
                long orgID = long.Parse(selID[1].ToString());
                long leID = 0;

                if (selID.Count() == 2)
                {
                    leID = orgID;
                }
                else
                {
                    leID = long.Parse(selID[2].ToString());
                }

                var data = HR.OverheadOrganizationStatusByHierarchy(hierarchyID, orgID);

                foreach (var view in data)
                {
                    if (view.HIER_LEVEL == 1)
                    {
                        Node node = new Node();
                        node.NodeID = string.Format("{0}:{1}:{2}", hierarchyID.ToString(), view.ORGANIZATION_ID.ToString(), leID.ToString());
                        node.Text = view.ORGANIZATION_NAME;

                        var nextData = HR.OverheadOrganizationStatusByHierarchy(hierarchyID, view.ORGANIZATION_ID);

                        Boolean _ActiveOrganizationsListed = nextData.Where(x => _leList.Contains(x.ORGANIZATION_ID.ToString()) & x.ORGANIZATION_STATUS == "Active").Count() > 0 ? true : false;

                        if (_ActiveOrganizationsListed)
                        {
                            //Lets add that node as the top level is active
                            if (nextData.Where(x => _leList.Contains(x.ORGANIZATION_ID.ToString())).Count() > 0)
                            {
                                node.Leaf = false;
                                node.Icon = Icon.Accept;
                            }
                            else
                            {
                                if (view.ORGANIZATION_STATUS == "Active")
                                {
                                    //Add the node as an active leaf
                                    node.Leaf = true;
                                    node.Icon = Icon.Accept;
                                }

                            }

             
                            e.Nodes.Add(node);
                        }
                        else
                        {
                            //Count is zero but does the current org match

                            if (_leList.Contains(view.ORGANIZATION_ID.ToString()))
                            {
                                if (view.ORGANIZATION_STATUS == "Active")
                                {
                                    //Add the node as an active leaf
                                    node.Leaf = true;
                                    node.Icon = Icon.Accept;
                                    e.Nodes.Add(node);
                                }
                            }

                        }
                    }
                }
            }
        }

        protected void deSelectNode(object sender, DirectEventArgs e)
        {
            TreeSelectionModel sm = uxOrgPanel.GetSelectionModel() as TreeSelectionModel;

            if (sm.SelectedRecordID != "0")
            {
                char[] delimChars = { ':' };
                string[] selID = sm.SelectedRecordID.Split(delimChars);
                long hierarchyID = long.Parse(selID[0].ToString());
                long orgID = long.Parse(selID[1].ToString());

                
                string _nodeText = e.ExtraParams["ORGANIZATION_NAME"];
                AddTab(sm.SelectedRecordID + "OS", _nodeText + " - Budget Rollup", "umViewBudget.aspx?securityView=Y&orgid=" + sm.SelectedRecordID + "&desc=" + _nodeText + " - Budget Rollup", false, true);
               

             }
        }

        protected void AddTab(string id, string title, string url, Boolean loadContent = false, Boolean setActive = false)
        {

            Ext.Net.Panel pan = new Ext.Net.Panel();

            pan.ID = "Tab" + id.Replace(":", "_");
            pan.Title = title;
            pan.CloseAction = CloseAction.Destroy;
            pan.Loader = new ComponentLoader();
            pan.Loader.ID = "loader" + id.Replace(":", "_");
            pan.Loader.Url = url;
            pan.Loader.Mode = LoadMode.Frame;
            pan.Closable = true;
            pan.Loader.LoadMask.ShowMask = true;
            pan.Loader.DisableCaching = true;
            pan.AddTo(uxCenterTabPanel);

            if (setActive)
                uxCenterTabPanel.SetActiveTab("Tab" + id.Replace(":", "_"));

            if (loadContent)
                pan.LoadContent();
        }
    }
}
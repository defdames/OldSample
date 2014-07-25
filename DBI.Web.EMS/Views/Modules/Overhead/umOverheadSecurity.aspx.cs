using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DBI.Core.Web;
using Ext.Net;
using DBI.Data;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public partial class umOverheadSecurity : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (!validateComponentSecurity("SYS.OverheadBudget.Security"))
                {
                    X.Redirect("~/Views/uxDefault.aspx");
                }
            }
        }

        /// <summary>
        /// Displayes all the legal entities that are in oracle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void deLoadLegalEntities(object sender, NodeLoadEventArgs e)
        {
            //Load LEs
            if (e.NodeID == "0")
            {
                var data = HR.LegalEntityHierarchies().Select(a => new { a.ORGANIZATION_ID, a.ORGANIZATION_NAME }).Distinct().ToList();

                //Build the treepanel
                foreach (var view in data)
                {

                    //Check for incomplete setup 
                    int _cnt = OVERHEAD_BUDGET_TYPE.BudgetTypes(view.ORGANIZATION_ID).Count();


                    //Create the Hierarchy Levels
                    Node node = new Node();
                    node.Text = view.ORGANIZATION_NAME;
                    node.NodeID = view.ORGANIZATION_ID.ToString();
                    if (_cnt > 0)
                    {
                        node.Icon = Icon.BulletGreen;
                    }
                    else
                    {
                        node.Icon = Icon.BulletRed;
                    }
                    e.Nodes.Add(node);
                }
            }
            else
            {
                long nodeID = long.Parse(e.NodeID);

                //Load Hierarchies for LE
                var data = HR.LegalEntityHierarchies().Where(a => a.ORGANIZATION_ID == nodeID).ToList();

                //Build the treepanel
                foreach (var view in data)
                {
                    //Create the Hierarchy Levels
                    Node node = new Node();
                    node.Icon = Icon.BulletMagnify;
                    node.Text = view.HIERARCHY_NAME;
                    node.NodeID = string.Format("{0}:{1}", view.ORGANIZATION_ID.ToString(), view.ORGANIZATION_STRUCTURE_ID.ToString());
                    node.Leaf = true;
                    e.Nodes.Add(node);
                }
            }

        }



        protected void deSelectNode(object sender, DirectEventArgs e)
        {
            TreeSelectionModel sm = uxOrganizationTreePanel.GetSelectionModel() as TreeSelectionModel;

            uxCenterTabPanel.RemoveAll();

            if (!sm.SelectedRecordID.Contains(":"))
            {
                AddTab(sm.SelectedRecordID + "BT", "Budget Types", "umOverheadBudgetTypes.aspx?leid=" + sm.SelectedRecordID, false, true);
                AddTab(sm.SelectedRecordID + "PM", "Open / Close Periods", "umOverheadPeriods.aspx?leid=" + sm.SelectedRecordID, false, false);
            }
            else
            {
                AddTab(sm.SelectedRecordID, "Organization Security / Account Maintenance", "umOverheadOrganizationSecurity.aspx?orgid=" + sm.SelectedRecordID, false, true);
            }
        }

        /// <summary>
        /// Adds a tab to the tabpanel
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="url"></param>
        /// <param name="loadContent"></param>
        /// <param name="setActive"></param>
        protected void AddTab(string id, string title, string url, Boolean loadContent = false, Boolean setActive = false)
        {

            Ext.Net.Panel pan = new Ext.Net.Panel();

            pan.ID = "Tab" + id.Replace(":","_");
            pan.Title = title;
            pan.CloseAction = CloseAction.Destroy;
            pan.Loader = new ComponentLoader();
            pan.Loader.ID = "loader" + id.Replace(":", "_");
            pan.Loader.Url = url;
            pan.Loader.Mode = LoadMode.Frame;
            pan.Loader.LoadMask.ShowMask = true;
            pan.Loader.DisableCaching = true;
            pan.AddTo(uxCenterTabPanel);

            if(setActive)
                uxCenterTabPanel.SetActiveTab("Tab" + id.Replace(":", "_"));

            if (loadContent)
                pan.LoadContent();
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

    }    
}
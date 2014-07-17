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
    public partial class umOverheadSecurity :BasePage
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
                    //Create the Hierarchy Levels
                    Node node = new Node();
                    node.Text = view.ORGANIZATION_NAME;
                    node.NodeID = view.ORGANIZATION_ID.ToString();
                    node.Icon = Icon.BulletEarth;
                    e.Nodes.Add(node);
                }
            }
            else if (!e.NodeID.Contains(":"))
            {
                long nodeID = long.Parse(e.NodeID);

                //Load Hierarchies for LE
                var data = HR.LegalEntityHierarchies().Where(a => a.ORGANIZATION_ID == nodeID).ToList();

                //Build the treepanel
                foreach (var view in data)
                {
                    //Create the Hierarchy Levels
                    Node node = new Node();
                    node.Text = view.HIERARCHY_NAME;
                    node.NodeID = string.Format("{0}:{1}", view.ORGANIZATION_ID.ToString(), view.ORGANIZATION_STRUCTURE_ID.ToString());
                    node.Leaf = false;
                    e.Nodes.Add(node);
                }

            }
            else // Load the lower levels
            {
                 char[] delimiterChars = { ':' };
                 string[] selectedID = e.NodeID.Split(delimiterChars);
                 long hierarchyID = long.Parse(selectedID[1].ToString());
                 long organizationID = long.Parse(selectedID[0].ToString());

                //Load Hierarchies for LE
                 var data = HR.ActiveOrganizationsByHierarchy(hierarchyID, organizationID);

                //Build the treepanel
                foreach (var view in data)
                {
                    if (view.HIER_LEVEL == 1)
                    {
                        //Create the Hierarchy Levels
                        Node node = new Node();
                        node.Text = view.ORGANIZATION_NAME;
                        node.NodeID = view.ORGANIZATION_ID.ToString();
                        node.Leaf = false;
                        e.Nodes.Add(node);
                    }
                }
            }
        }



    }
}
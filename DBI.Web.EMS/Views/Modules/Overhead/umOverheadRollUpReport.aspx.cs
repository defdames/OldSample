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
    public partial class umOverheadRollUpReport : DBI.Core.Web.BasePage
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

        protected void deLoadOrgTree(object sender, NodeLoadEventArgs e)
        {

            // Add LE
            if (e.NodeID == "0")
            {
                var data = HR.LegalEntityHierarchies().Select(a => new { a.ORGANIZATION_ID, a.ORGANIZATION_NAME }).Distinct().ToList();


                //Build the treepanel
                foreach (var view in data)
                {

                    //Only show for the profile option OverheadBudgetHierarchy

                    string _profileValue = SYS_ORG_PROFILE_OPTIONS.OrganizationProfileOption("OverheadBudgetHierarchy", view.ORGANIZATION_ID);

                    if (_profileValue.Length > 0)
                    {
                        long _profileLong = long.Parse(_profileValue);
                        var _hierData = HR.LegalEntityHierarchies().Where(a => a.ORGANIZATION_ID == view.ORGANIZATION_ID & a.ORGANIZATION_STRUCTURE_ID == _profileLong).ToList();

                        //Check for incomplete setup 
                        int _cnt = OVERHEAD_BUDGET_TYPE.BudgetTypes(view.ORGANIZATION_ID).Count();


                        //Create the Hierarchy Levels
                        Node node = new Node();
                        node.Text = view.ORGANIZATION_NAME;
                        node.NodeID = string.Format("{0}:{1}", _profileValue.ToString(), view.ORGANIZATION_ID.ToString());
                        node.Leaf = false;
                        e.Nodes.Add(node);
                    }
                }

            }

            // Add orgs
            else
            {
                char[] delimChars = { ':' };
                string[] selID = e.NodeID.Split(delimChars);
                long hierarchyID = long.Parse(selID[0].ToString());
                long orgID = long.Parse(selID[1].ToString());

                var data = HR.ActiveOrganizationsByHierarchy(hierarchyID, orgID);

                bool addNode;
                bool leafNode;
                bool colorNode;

                foreach (var view in data)
                {
                    if (view.HIER_LEVEL == 1)
                    {
                        addNode = false;
                        leafNode = true;
                        colorNode = false;
                        var nextData = HR.ActiveOrganizationsByHierarchy(hierarchyID, view.ORGANIZATION_ID);
                        addNode = true;
                        colorNode = true;
              

                        if (addNode == true)
                        {
                            Node node = new Node();
                            node.NodeID = string.Format("{0}:{1}", hierarchyID.ToString(), view.ORGANIZATION_ID.ToString());
                            node.Text = view.ORGANIZATION_NAME;
                            node.Leaf = leafNode;
                            if (colorNode == true)
                            {
                                if (BB.IsRollup(view.ORGANIZATION_ID) == true)
                                {
                                    node.Icon = Icon.PackageGreen;
                                }
                                else
                                {
                                    node.Icon = Icon.Accept;
                                }
                            }
                            else
                            {
                                if (BB.IsRollup(view.ORGANIZATION_ID) == true)
                                {
                                    node.Icon = Icon.PackageGreen;
                                }
                                else
                                {
                                    node.Icon = Icon.TagsGrey;
                                }
                            }
                            e.Nodes.Add(node);
                        }
                    }
                }
            }
        }
    }
}
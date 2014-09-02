﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using DBI.Data;
using DBI.Core.Security;

namespace DBI.Web.EMS.Views.Modules.BudgetBidding
{
    public partial class umBudgetBiddingMain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                BB.CleanOldTempRecords(2);
            }
        }

        protected void deLoadOrgTree(object sender, NodeLoadEventArgs e)
        {
            // User clicked on legal entity or hierarchy
            if (e.NodeID.Contains(":") == false)
            {
                if (e.NodeID == "0")
                {
                    var data = HR.LegalEntityHierarchies().Select(a => new { a.ORGANIZATION_ID, a.ORGANIZATION_NAME }).Distinct().ToList();
                    foreach (var view in data)
                    {
                        Node node = new Node();
                        node.Text = view.ORGANIZATION_NAME;
                        node.NodeID = view.ORGANIZATION_ID.ToString();
                        e.Nodes.Add(node);
                    }
                }
                else
                {
                    long nodeID = long.Parse(e.NodeID);
                    var data = HR.LegalEntityHierarchies().Where(a => a.ORGANIZATION_ID == nodeID);
                    var orgsList = BB.UserAllowedOrgs(SYS_USER_INFORMATION.UserID(User.Identity.Name));
                    foreach (var view in data)
                    {
                        var nextData = HR.ActiveOrganizationsByHierarchy(view.ORGANIZATION_STRUCTURE_ID, view.ORGANIZATION_ID);

                        // In next org?
                        foreach (long allowedOrgs in orgsList)
                        {
                            if (nextData.Select(x => x.ORGANIZATION_ID).Contains(allowedOrgs))
                            {
                                Node node = new Node();
                                node.Text = view.HIERARCHY_NAME;
                                node.NodeID = string.Format("{0}:{1}", view.ORGANIZATION_STRUCTURE_ID.ToString(), view.ORGANIZATION_ID.ToString());
                                e.Nodes.Add(node);
                                //break;
                            }
                        }
                    }
                }
            }

            // User clicked on org
            else
            {
                char[] delimChars = { ':' };
                string[] selID = e.NodeID.Split(delimChars);
                long hierarchyID = long.Parse(selID[0].ToString());
                long orgID = long.Parse(selID[1].ToString());
                var data = HR.ActiveOrganizationsByHierarchy(hierarchyID, orgID);
                var orgsList = BB.UserAllowedOrgs(SYS_USER_INFORMATION.UserID(User.Identity.Name));
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

                        // In this org?
                        if (BB.IsUserOrgAndAllowed(SYS_USER_INFORMATION.UserID(User.Identity.Name), view.ORGANIZATION_ID) == true)
                        {
                            addNode = true;
                            colorNode = true;
                        }

                        // In next org?
                        foreach (long allowedOrgs in orgsList)
                        {
                            if (nextData.Select(x => x.ORGANIZATION_ID).Contains(allowedOrgs))
                            {
                                addNode = true;
                                leafNode = false;
                                break;
                            }
                        }

                        if (addNode == true)
                        {
                            Node node = new Node();
                            node.NodeID = string.Format("{0}:{1}", hierarchyID.ToString(), view.ORGANIZATION_ID.ToString());
                            node.Text = view.ORGANIZATION_NAME;
                            node.Leaf = leafNode;
                            if (colorNode == true)
                            {
                                node.Icon = Icon.Accept;
                            }
                            else
                            {
                                node.Icon = Icon.FolderGo;
                            }
                            e.Nodes.Add(node);
                        }
                    }
                }
            }
        }        

        protected void deLoadFiscalYears(object sender, StoreReadDataEventArgs e)
        {
            uxFiscalYearStore.DataSource = PA.AllFiscalYears();
        }

        protected void deLoadBudgetVersions(object sender, StoreReadDataEventArgs e)
        {
            uxVersionStore.DataSource = BB.BudgetVersions();
        }

        protected void deSelectOrg(object sender, DirectEventArgs e)
        {
            bool prevBlank = PreviouslyBlankBudget();     
            string nodeID = uxOrgPanel.SelectedNodes[0].NodeID;
            char[] delimChars = { ':' };
            string[] selID = nodeID.Split(delimChars);

            if (nodeID == "0" || selID.GetUpperBound(0) == 0)
            {
                uxHidOrgOK.Text = "";
                uxYearVersionTitle.Text = "";
                DisableToolbar();
                LoadBudget(prevBlank);
                return;
            }

            long orgID = long.Parse(selID[1].ToString());
            
            if (BB.IsUserOrgAndAllowed(SYS_USER_INFORMATION.UserID(User.Identity.Name), orgID) == false)
            {
                uxHidOrgOK.Text = "";
                uxYearVersionTitle.Text = "";
                DisableToolbar();
                LoadBudget(prevBlank);
            }
            else
            {
                uxHidOrgOK.Text = "Y";
                uxYearVersionTitle.Text = uxOrgPanel.SelectedNodes[0].Text;
                EnableToolbar();
                LoadBudget(prevBlank);
            }     
        }

        protected void deSelectYear(object sender, DirectEventArgs e)
        {
            bool prevBlank = PreviouslyBlankBudget();

            uxHidYearOK.Text = "Y";
            LoadBudget(prevBlank);
        }

        protected void deSelectVersion(object sender, DirectEventArgs e)
        {
            bool prevBlank = PreviouslyBlankBudget();

            uxHidVerOK.Text = "Y";
            LoadBudget(prevBlank);            
        }

        protected bool PreviouslyBlankBudget()
        {
            string orgOK = uxHidOrgOK.Text;
            string yearOK = uxHidYearOK.Text;
            string verOK = uxHidVerOK.Text;
            bool prevBlank = (orgOK == "" || yearOK == "" || verOK == "") ? true : false;

            return prevBlank;
        }

        protected void LoadBudget(bool prevBlank)
        {
            string orgOK = uxHidOrgOK.Text;
            string yearOK = uxHidYearOK.Text;
            string verOK = uxHidVerOK.Text;
            string url = "umBlankBudget.aspx";

            if (orgOK == "Y" && yearOK == "Y" && verOK == "Y")
            {
                string nodeID = uxOrgPanel.SelectedNodes[0].NodeID;
                char[] delimChars = { ':' };
                string[] selID = nodeID.Split(delimChars);
                string hierarchyID = selID[0].ToString();
                long orgID = Convert.ToInt64(selID[1].ToString());

                string orgName = HttpUtility.UrlEncode(uxOrgPanel.SelectedNodes[0].Text);
                long fiscalYear = Convert.ToInt64(uxFiscalYear.SelectedItem.Value);
                long verID = Convert.ToInt64(uxVersion.SelectedItem.Value);
                string verName = HttpUtility.UrlEncode(uxVersion.SelectedItem.Text);

                if (BB.IsRollup(orgID) == false)
                {
                    url = "umYearBudget.aspx?hierID=" + hierarchyID + "&orgID=" + orgID + "&orgName=" + orgName + "&fiscalYear=" + fiscalYear + "&verID=" + verID + "&verName=" + verName;
                }
                else
                {
                    url = "umYearRollupSummary.aspx?hierID=" + hierarchyID + "&orgID=" + orgID + "&orgName=" + orgName + "&fiscalYear=" + fiscalYear + "&verID=" + verID + "&verName=" + verName;
                }
            }

            else if (prevBlank == true)
            {
                return;            
            }

            uxBudgetPanel.Loader.SuspendScripting();
            uxBudgetPanel.Loader.Url = url;
            uxBudgetPanel.Loader.Mode = LoadMode.Frame;
            uxBudgetPanel.Loader.DisableCaching = true;
            uxBudgetPanel.LoadContent();
        }
      
        protected void EnableToolbar()
        {
            uxFiscalYear.Enable();
            uxVersion.Enable();
            uxOrgSettings.Disable();  // FIX
        }

        protected void DisableToolbar()
        {
            uxFiscalYear.Disable();
            uxVersion.Disable();
            uxOrgSettings.Disable();
        }

        protected void StandardMsgBox(string title, string msg, string msgIcon)
        {
            // msgIcon can be:  ERROR, INFO, QUESTION, WARNING
            X.Msg.Show(new MessageBoxConfig()
            {
                Title = title,
                Message = msg,
                Buttons = MessageBox.Button.OK,
                Icon = (MessageBox.Icon)Enum.Parse(typeof(MessageBox.Icon), msgIcon)
            });
        }
    }
}
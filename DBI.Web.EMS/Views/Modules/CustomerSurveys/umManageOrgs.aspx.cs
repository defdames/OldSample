﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DBI.Core.Security;
using DBI.Core.Web;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.CustomerSurveys
{
    public partial class umManageOrgs : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void deReadForms(object sender, StoreReadDataEventArgs e)
        {
            string _selectedRecordID = uxCompanySelectionModel.SelectedRecordID;
            if (_selectedRecordID != "0" && _selectedRecordID.Contains(":"))
            {
                char[] _delimiterChars = { ':' };
                string[] _selectedID = _selectedRecordID.Split(_delimiterChars);
                long _hierarchyID = long.Parse(_selectedID[0].ToString());
                long _organizationID = long.Parse(_selectedID[1].ToString());
                List<CUSTOMER_SURVEY_FORMS> FormData = CUSTOMER_SURVEY_FORMS.GetForms().Where(x => x.ORG_ID == _organizationID).ToList();
                List<CustomerSurveyForms> AllData = new List<CustomerSurveyForms>();
                foreach (CUSTOMER_SURVEY_FORMS ThisForm in FormData)
                {
                    CustomerSurveyForms NewForm = new CustomerSurveyForms();
                    NewForm.FORMS_NAME = ThisForm.FORMS_NAME;
                    NewForm.ORG_ID = ThisForm.ORG_ID;
                    NewForm.FORM_ID = ThisForm.FORM_ID;
                    //var NumQuestions = CUSTOMER_SURVEY_FORMS.NumberOfQuestionsInForm(ThisForm.FORM_ID);

                   // NewForm.NUM_QUESTIONS = NumQuestions;
                    AllData.Add(NewForm);
                }
                //  uxFormsGridStore.DataSource = AllData;
            }
        }

        protected void deLoadOrgTree(object sender, NodeLoadEventArgs e)
        {
            // User clicked on legal entity or hierarchy
            if (e.NodeID.Contains(":") == false)
            {
                if (e.NodeID == "0")
                {
                    var data = HR.ActiveOverheadBudgetLegalEntities();
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
                    foreach (var view in data)
                    {
                        Node node = new Node();
                        node.Text = view.HIERARCHY_NAME;
                        node.NodeID = string.Format("{0}:{1}", view.ORGANIZATION_STRUCTURE_ID.ToString(), view.ORGANIZATION_ID.ToString());
                        e.Nodes.Add(node);
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
                var OrgsList = SYS_USER_ORGS.GetUserOrgs(SYS_USER_INFORMATION.UserID(User.Identity.Name)).Select(x => x.ORG_ID);
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
                        if (SYS_USER_ORGS.IsInOrg(SYS_USER_INFORMATION.UserID(User.Identity.Name), view.ORGANIZATION_ID) == true)
                        {
                            addNode = true;
                            colorNode = true;
                        }

                        // In next org?
                        foreach (long allowedOrgs in OrgsList)
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
                            //if (colorNode == true)
                            //{
                            //    node.Icon = Icon.Tick;
                            //}
                            //else
                            //{
                            //    node.Icon = Icon.FolderGo;
                            //}
                            e.Nodes.Add(node);
                        }
                    }
                }
            }
        }

        protected void deLoadFormThresholds(object sender, DirectEventArgs e)
        {
            string _selectedRecordID = uxCompanySelectionModel.SelectedRecordID;
            if (_selectedRecordID != "0" && _selectedRecordID.Contains(":"))
            {
                char[] _delimiterChars = { ':' };
                string[] _selectedID = _selectedRecordID.Split(_delimiterChars);
                long _hierarchyID = long.Parse(_selectedID[0].ToString());
                long _organizationID = long.Parse(_selectedID[1].ToString());
            }
            long FormId = long.Parse(e.ExtraParams["FormId"]);
        }

        protected void deSubmitThreshold(object sender, DirectEventArgs e)
        {

        }
    }
}
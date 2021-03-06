﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Ext.Net;
using DBI.Core.Web;
using DBI.Data;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public partial class umOverheadBudgetTypes_ : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (!validateComponentSecurity("SYS.OverheadBudget.Security"))
                {
                    X.Redirect("~/Views/uxDefault.aspx");
                }

                //Verify that the organization is valid for budget types, if not disable the add new button and display a warning message
                //long _businessUnitId = 0;
                //    _businessUnitId = long.Parse(Request.QueryString["leid"]);

                //int _cnt = OVERHEAD_BUDGET_TYPE.BudgetTypes(_businessUnitId).Count();
                
                //if(_cnt == 0)
                //{
                //    HR.ORGANIZATION _orgInfo = HR.Organization(_businessUnitId);
                //    X.Msg.Alert("Invalid business unit", string.Format("The {0} busineess unit is not vaild and does not have any oracle budget types assigned. You will not be able to complete the setup of this organization until a budget type is created.", _orgInfo.ORGANIZATION_NAME)).Show();
                //    uxBudgetTypeGridPanel.Disabled = true;
                //    return;
                //}

                //uxBudgetTypeGridPanel.GetStore().Reload();
            }
        }

        protected void deReadBudgetTypesByLegalEntity(object sender, StoreReadDataEventArgs e)
        {

            string _selectedRecordID = Request.QueryString["leid"];

            char[] _delimiterChars = { ':' };
            string[] _selectedID = _selectedRecordID.Split(_delimiterChars);
            long _hierarchyID = long.Parse(_selectedID[1].ToString());
            long _organizationID = long.Parse(_selectedID[0].ToString());

            if (_organizationID > 0)
            {
                List<OVERHEAD_BUDGET_TYPE> data = OVERHEAD_BUDGET_TYPE.BudgetTypes(_organizationID);
                uxBudgetTypeStore.DataSource = data;
                uxAssignBudgetType.Disabled = false;
            }


        }

          protected void deDeleteBudgetType(object sender, DirectEventArgs e)
        {           
                long _budgetTypeIDSelected;

                RowSelectionModel _rsm = uxBudgetTypeSelectionModel;
                Boolean _check = long.TryParse(_rsm.SelectedRecordID, out _budgetTypeIDSelected);

               //First check if its in use somewhere

              using(Entities _context = new Entities())
              {
                  if (_context.OVERHEAD_ORG_BUDGETS.Where(x => x.OVERHEAD_BUDGET_TYPE_ID == _budgetTypeIDSelected).Any())
                  {
                      X.Msg.Alert("Type in Use Error", "You can not delete this budget type as it is currently in use!").Show();
                      return;
                  }
              }
                OVERHEAD_BUDGET_TYPE _budgetType = OVERHEAD_BUDGET_TYPE.BudgetType(_budgetTypeIDSelected);
   
                GenericData.Delete<OVERHEAD_BUDGET_TYPE>(_budgetType);
                uxBudgetTypeStore.Reload();
                uxDeleteBudgetType.Disabled = true;
               
        }

        protected void deAddBudgetType(object sender, DirectEventArgs e)
        {
            string _selectedRecordID = Request.QueryString["leid"];

            char[] _delimiterChars = { ':' };
            string[] _selectedID = _selectedRecordID.Split(_delimiterChars);
            long _hierarchyID = long.Parse(_selectedID[1].ToString());
            long _organizationID = long.Parse(_selectedID[0].ToString());

                string _editMode = e.ExtraParams["Edit"];


                string url = "umAddRemoveBudgetType.aspx?buID=" + _organizationID;
                RowSelectionModel _recordID = uxBudgetTypeSelectionModel;

                if (!string.IsNullOrEmpty(_editMode))
                {
                    url = url + "&recordId=" + _recordID.SelectedRecordID.ToString();
                }

                Window win = new Window
                {
                    ID = "uxAddEditBudgetType",
                    Title = "Budget Types",
                    Height = 350,
                    Width = 550,
                    Modal = true,
                    Resizable = false,
                    CloseAction = CloseAction.Destroy,
                    Loader = new ComponentLoader
                    {
                        Mode = LoadMode.Frame,
                        DisableCaching = true,
                        Url = url,
                        AutoLoad = true,
                        LoadMask =
                        {
                            ShowMask = true
                        }
                    }
                };

                win.Listeners.Close.Handler = "#{uxBudgetTypeGridPanel}.getStore().load();";

                win.Render(this.Form);
                win.Show();
        }

    }
}
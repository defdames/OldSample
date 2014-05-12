using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.Overhead.BudgetType.AddEdit
{
    public partial class umAddEditBudgetType : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                string _businessUnitId = Request.QueryString["buID"].ToString();
                string _recordId = string.Empty;
                
                if (Request.QueryString["recordId"] != "" && Request.QueryString["recordId"] != null) 
                    {
                        _recordId = Request.QueryString["recordId"].ToString();
                    }

                
                long _editRecordId;
                bool _editRecordCheck = long.TryParse(_recordId, out _editRecordId);

                uxBudgetNameStore.Parameters.Add(new Ext.Net.StoreParameter("BUSINESSUNITID", _businessUnitId));
                uxParentBudgetStore.Parameters.Add(new Ext.Net.StoreParameter("BUSINESSUNITID", _businessUnitId));

                if (_editRecordId > 0)
                {
                    uxBudgetNameStore.Parameters.Add(new Ext.Net.StoreParameter("RECORDID", _editRecordId.ToString()));

                    uxBudgetNameStore.Reload();
                    OVERHEAD_BUDGET_TYPE _budgetType = OVERHEAD_BUDGET_TYPE.budgetTypeById(_editRecordId);
                    uxBudgetName.SetValueAndFireSelect(_budgetType.BUDGET_NAME);
                    uxBudgetDescription.Text = _budgetType.BUDGET_DESCRIPTION;
                    uxDeleteBudgetType.Disabled = false;
                    uxBudgetName.ReadOnly = true;
                    uxAddBudgetType.Disabled = false;
                }

            }

        }

        protected void deSaveBudgetType(object sender, DirectEventArgs e)
        {
            //Validate Form
            if(string.IsNullOrWhiteSpace(uxBudgetDescription.Text))
            {
                X.Msg.Alert("Fields Missing", "Budget description is required to save this record!").Show();
                e.Success = false;
                return;
            }


            long organizationID = long.Parse(Request.QueryString["buID"]);
            OVERHEAD_BUDGET_TYPE data = new OVERHEAD_BUDGET_TYPE();
            string _recordId = string.Empty;

            if (Request.QueryString["recordId"] != "" && Request.QueryString["recordId"] != null)
            {
                _recordId = Request.QueryString["recordId"].ToString();
                long _editRecordId;
                bool _editRecordCheck = long.TryParse(_recordId, out _editRecordId);

                data = OVERHEAD_BUDGET_TYPE.budgetTypeById(_editRecordId);
                data.BUDGET_DESCRIPTION = uxBudgetDescription.Text;
                data.MODIFY_DATE = DateTime.Now;
                data.MODIFIED_BY = User.Identity.Name;
            }
            else
            {
                 data.BUDGET_NAME = uxBudgetName.SelectedItem.Value;
                 data.BUDGET_DESCRIPTION = uxBudgetDescription.Text;
                 data.LE_ORD_ID = organizationID;
                 data.CREATE_DATE = DateTime.Now;
                 data.MODIFY_DATE = DateTime.Now;
                 data.CREATED_BY = User.Identity.Name;
                 data.MODIFIED_BY = User.Identity.Name;
            }

            GenericData.Insert<OVERHEAD_BUDGET_TYPE>(data);
        }

        protected void deDeleteBudgetType(object sender, DirectEventArgs e)
        {
            long _editRecordId;
            bool _editRecordCheck = long.TryParse(Request.QueryString["recordId"].ToString(), out _editRecordId);

            OVERHEAD_BUDGET_TYPE _budgetType = OVERHEAD_BUDGET_TYPE.budgetTypeById(_editRecordId);
            GenericData.Delete<OVERHEAD_BUDGET_TYPE>(_budgetType);

        }
    }
}
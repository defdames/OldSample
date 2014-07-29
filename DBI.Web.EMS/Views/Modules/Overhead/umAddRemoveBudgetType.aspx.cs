using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DBI.Core.Web;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.Overhead.Views
{
    public partial class umAddRemoveBudgetType : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (!validateComponentSecurity("SYS.OverheadBudget.Security"))
                {
                    X.Redirect("~/Views/uxDefault.aspx");
                }

                long _businessUnitId = 0;

                if (Request.QueryString["buID"] != "" && Request.QueryString["buID"] != null)
                    _businessUnitId = long.Parse(Request.QueryString["buID"]);
                
                // If list count is equal to 0 then hide the link data box
                List<OVERHEAD_BUDGET_TYPE> data = OVERHEAD_BUDGET_TYPE.BudgetTypes(_businessUnitId);

                if (data.Count == 0)
                    uxLinkedBudgetType.Hidden = true;
            }
       }

        protected void deLoadLinkedBudgetNames(object sender, StoreReadDataEventArgs e)
        {
            long _businessUnitId = 0;

            if (Request.QueryString["buID"] != "" && Request.QueryString["buID"] != null)
                _businessUnitId = long.Parse(Request.QueryString["buID"]);

            uxLinkedBudgetTypeStore.DataSource = GL.BudgetTypesEnteredAndAvailaible(_businessUnitId);
            uxLinkedBudgetTypeStore.DataBind();
        }

        protected void deLoadBudgetNames(object sender, StoreReadDataEventArgs e)
        {
            long _businessUnitId = 0;

            if (Request.QueryString["buID"] != "" && Request.QueryString["buID"] != null)
                _businessUnitId = long.Parse(Request.QueryString["buID"]);

            uxBudgetNameStore.DataSource = GL.BudgetTypesRemaining(_businessUnitId);
            uxBudgetNameStore.DataBind();

        }

        protected void deAssignBudgetType(object sender, DirectEventArgs e)
        {
            long _businessUnitId = 0;
            long _parentBudgetTypeId = 0;
            Boolean _convertBudgetTypeID;

            if (Request.QueryString["buID"] != "" && Request.QueryString["buID"] != null)
                _businessUnitId = long.Parse(Request.QueryString["buID"]);

            
            //Validation check, only allow a null value for the linked type if the record count of the database table is zero
            if(OVERHEAD_BUDGET_TYPE.BudgetTypes(_businessUnitId).Count() > 0 && uxLinkedBudgetType.SelectedItem.Value == null)
            {
                throw new DBICustomException("You must select a linked budget type in order to save this record!");
            }


            OVERHEAD_BUDGET_TYPE _data = new OVERHEAD_BUDGET_TYPE();
            _data.LE_ORG_ID = _businessUnitId;
            _data.BUDGET_NAME = uxBudgetName.SelectedItem.Value;
            _data.BUDGET_DESCRIPTION = uxBudgetDescription.Text;
            _data.CREATE_DATE = DateTime.Now;
            _data.MODIFY_DATE = DateTime.Now;
            _data.CREATED_BY = User.Identity.Name;
            _data.MODIFIED_BY = User.Identity.Name;

            if (uxLinkedBudgetType.SelectedItem.Value != null)
            {
                _convertBudgetTypeID = long.TryParse(uxLinkedBudgetType.SelectedItem.Value, out _parentBudgetTypeId);
                _data.PARENT_BUDGET_TYPE_ID = _parentBudgetTypeId;
            }

            GenericData.Insert<OVERHEAD_BUDGET_TYPE>(_data);

        }

    }
}
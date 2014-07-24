using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Data;
using Ext.Net;
using DBI.Core.Web;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public partial class umOverheadMaintainBudgets : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (!validateComponentSecurity("SYS.OverheadBudget.ViewAndMaintain"))
                {
                    X.Redirect("~/Views/uxDefault.aspx");
                }

            }
        }

        protected void deOrganizationList(object sender, StoreReadDataEventArgs e)
        {
            List<long> OrgsList = SYS_USER_ORGS.GetUserOrgs(SYS_USER_INFORMATION.UserID(User.Identity.Name)).Select(x => x.ORG_ID).ToList();

            //List<HR.ORGANIZATION_V1> _data = ActiveOrganizationsByHierarchy(hierarchyId, organizationId);
            //foreach (var view in _data)
            //{
            //    view.ORGANIZATION_STATUS = (SYS_ORG_PROFILE_OPTIONS.OrganizationProfileOption("OverheadBudgetOrganization", view.ORGANIZATION_ID) == "Y" ? "Budgeting Allowed" : "Not Active");
            //}
            //return _data;

        }

     
    }
}
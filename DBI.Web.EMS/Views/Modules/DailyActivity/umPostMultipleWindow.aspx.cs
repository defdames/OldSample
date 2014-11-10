﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Data;
using DBI.Core.Web;
using Ext.Net;


namespace DBI.Web.EMS.Views.Modules.DailyActivity
{
    public partial class umPostMultipleWindow : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!validateComponentSecurity("SYS.DailyActivity.View") && !validateComponentSecurity("SYS.DailyActivity.EmployeeView"))
            {
                X.Redirect("~/Views/uxDefault.aspx");

            }
            if (!X.IsAjaxRequest)
            {
            }
        }

        protected void deReadPostableData(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                List<long> OrgsList = SYS_USER_ORGS.GetUserOrgs(SYS_USER_INFORMATION.UserID(User.Identity.Name)).Select(x => x.ORG_ID).ToList();
                IQueryable<DAILY_ACTIVITY.HeaderData> HeaderList = DAILY_ACTIVITY.GetHeaders(_context, false, false, OrgsList).Where(x => x.STATUS == 3);

                int count;
                uxHeaderPostStore.DataSource = GenericData.ListFilterHeader<DAILY_ACTIVITY.HeaderData>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], HeaderList, out count);
                e.Total = count;
            }
        }

        protected void dePostData(object sender, DirectEventArgs e)
        {
            string json = e.ExtraParams["RowsToPost"];
            List<DAILY_ACTIVITY.HeaderData> HeadersToPost = JSON.Deserialize<List<DAILY_ACTIVITY.HeaderData>>(json);
            foreach (DAILY_ACTIVITY.HeaderData HeaderToPost in HeadersToPost)
            {
                Interface.PostToOracle(HeaderToPost.HEADER_ID, User.Identity.Name);
            }

            X.Js.Call("parent.App.uxManageGridStore.reload()");
        }
    }
}
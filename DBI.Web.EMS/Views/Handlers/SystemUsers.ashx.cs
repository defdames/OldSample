using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ext.Net;

namespace DBI.Web.EMS.Views.Handlers
{
    /// <summary>
    /// Summary description for SystemUsers
    /// </summary>
    public class SystemUsers : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/json";

            int start = 0;
            int limit = 10;
            string sort = string.Empty;
            string dir = string.Empty;
            string query = string.Empty;

            if (!string.IsNullOrEmpty(context.Request["start"]))
            {
                start = int.Parse(context.Request["start"]);
            }

            if (!string.IsNullOrEmpty(context.Request["limit"]))
            {
                limit = int.Parse(context.Request["limit"]);
            }

            if (!string.IsNullOrEmpty(context.Request["sort"]))
            {
                sort = context.Request["sort"];
            }

            if (!string.IsNullOrEmpty(context.Request["dir"]))
            {
                dir = context.Request["dir"];
            }

            if (!string.IsNullOrEmpty(context.Request["query"]))
            {
                query = context.Request["query"];
            }

            Paging<DBI.Data.SYS_USER_INFORMATION> users = DBI.Data.SYS_USER_INFORMATION.UsersLookup(start, limit, sort, dir, query, "EMPLOYEE_NAME");
            context.Response.Write(string.Format("{{TOTAL:{1},'USERS':{0}}}", JSON.Serialize(users.Data), users.TotalRecords));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
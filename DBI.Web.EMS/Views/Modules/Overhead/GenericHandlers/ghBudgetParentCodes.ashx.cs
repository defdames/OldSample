using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public class ghBudgetParentCodes : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/json";

            int start = 0;
            int limit = 1000;
            string sort = string.Empty;
            string dir = string.Empty;
            string query = string.Empty;
            string businessUnitId = string.Empty;
            string budgetname = string.Empty;

            if (!string.IsNullOrEmpty(context.Request["start"]))
            {
                start = int.Parse(context.Request["start"]);
            }

            if (!string.IsNullOrEmpty(context.Request["limit"]))
            {
                limit = int.Parse(context.Request["limit"]);
            }

            if (!string.IsNullOrEmpty(context.Request["query"]))
            {
                query = context.Request["query"];
            }

            if (!string.IsNullOrEmpty(context.Request["BUSINESSUNITID"]))
            {
                businessUnitId = context.Request["BUSINESSUNITID"];
            }

            if (!string.IsNullOrEmpty(context.Request["BUDGETNAME"]))
            {
                budgetname = context.Request["BUDGETNAME"];
            }

            Paging<OVERHEAD_BUDGET_TYPE> responseData = dataPaging(start, limit, query, businessUnitId);

            context.Response.Write(string.Format("{{total:{1},'data':{0}}}", JSON.Serialize(responseData.Data), responseData.TotalRecords));
        }

        public static Paging<OVERHEAD_BUDGET_TYPE> dataPaging(int start, int limit, string filter, string businessUnitId)
        {

            long _businessUnitId = long.Parse(businessUnitId);

            List<OVERHEAD_BUDGET_TYPE> data = OVERHEAD_BUDGET_TYPE.BudgetTypes(_businessUnitId);

            if (!string.IsNullOrEmpty(filter) && filter != "*")
            {
                data.RemoveAll(bu__1 => !bu__1.BUDGET_NAME.ToLower().StartsWith(filter.ToLower()));
            }

            if ((start + limit) > data.Count)
            {
                limit = data.Count - start;
            }

            List<OVERHEAD_BUDGET_TYPE> _range = (start < 0 || limit < 0) ? data : data.GetRange(start, limit);

            return new Paging<OVERHEAD_BUDGET_TYPE>(_range, data.Count);
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
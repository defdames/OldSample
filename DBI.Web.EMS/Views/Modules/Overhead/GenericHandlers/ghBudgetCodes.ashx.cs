using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public class ghBudgetCodes : IHttpHandler
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
            string recordId = string.Empty;

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

            if (!string.IsNullOrEmpty(context.Request["RECORDID"]))
            {
                recordId = context.Request["RECORDID"];
            }

            Paging<GL.BUDGET_TYPE> responseData = dataPaging(start, limit, query, businessUnitId, recordId);

            context.Response.Write(string.Format("{{total:{1},'data':{0}}}", JSON.Serialize(responseData.Data), responseData.TotalRecords));
        }

        public static Paging<GL.BUDGET_TYPE> dataPaging(int start, int limit, string filter, string businessUnitId, string recordId)
        {

            long _businessUnitId = long.Parse(businessUnitId);

            List<GL.BUDGET_TYPE> _data;

            if (!string.IsNullOrEmpty(recordId))
            {
                long _recordId = long.Parse(recordId.ToString());
                _data = GL.BudgetTypesRemaining(_businessUnitId, _recordId);
            }
            else
            {
                _data = GL.BudgetTypesRemaining(_businessUnitId);
            }

            if (!string.IsNullOrEmpty(filter) && filter != "*")
            {
                _data.RemoveAll(bu__1 => !bu__1.BUDGET_NAME.ToLower().StartsWith(filter.ToLower()));
            }

            if ((start + limit) > _data.Count)
            {
                limit = _data.Count - start;
            }

            List<GL.BUDGET_TYPE> _range = (start < 0 || limit < 0) ? _data : _data.GetRange(start, limit);

            return new Paging<GL.BUDGET_TYPE>(_range, _data.Count);
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    /// <summary>
    /// Summary description for GLCompanyList
    /// </summary>
    public class GLDivisionCodes : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/json";

            int start = 0;
            int limit = 10;
            string sort = string.Empty;
            string dir = string.Empty;
            string query = string.Empty;
            string segment1 = string.Empty;
            string segment2 = string.Empty;

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

             if (!string.IsNullOrEmpty(context.Request["segment1"]))
            {
                segment1 = context.Request["segment1"];
            }

             if (!string.IsNullOrEmpty(context.Request["segment2"]))
             {
                 segment2 = context.Request["segment2"];
             }

            Paging<ComboList> glCompanyCodes = GLDivisionCodes.GLCodePaging(start, limit, query, segment1, segment2);

            context.Response.Write(string.Format("{{total:{1},'glcodes':{0}}}", JSON.Serialize(glCompanyCodes.Data), glCompanyCodes.TotalRecords));
        }

        public static Paging<ComboList> GLCodePaging(int start, int limit, string filter, string segment1, string segment2)
        {
            List<ComboList> GLCodes = GlCodes(segment1,segment2);

            if (!string.IsNullOrEmpty(filter) && filter != "*")
            {
                GLCodes.RemoveAll(bu__1 => !bu__1.Name.ToLower().StartsWith(filter.ToLower()));
            }

            if ((start + limit) > GLCodes.Count)
            {
                limit = GLCodes.Count - start;
            }

            List<ComboList> rangeglaccounts = (start < 0 || limit < 0) ? GLCodes : GLCodes.GetRange(start, limit);

            return new Paging<ComboList>(rangeglaccounts, GLCodes.Count);
        }

        public static List<ComboList> GlCodes(string segment1, string segment2)
        {
            using (Entities _context = new Entities())
            {
                List<string> glCodes = _context.GL_ACCOUNTS_V.Where(a => a.SEGMENT1 == segment1).Where(s => s.SEGMENT2 == segment2).Select(a => a.SEGMENT3).Distinct().ToList();
                List<ComboList> comboListBox = new List<ComboList>();
                foreach (string code in glCodes)
                {
                    comboListBox.Add(new ComboList(code,code));
                }
                return comboListBox;
            }
        }

        public class ComboList
        {
            public ComboList()
            {
            }

            public ComboList(string ID, string Name)
            {
                _ID = ID;
                _Name = Name;
            }

            private string _ID { get; set; }
            public string ID
            {
                get { return _ID; }
                set { _ID = value; }
            }

            private string _Name { get; set; }
            public string Name
            {
                get { return _Name; }
                set { _Name = value; }
            }
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
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
    public class GLCompanyCodes : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/json";

            int start = 0;
            int limit = 1000;
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

            if (!string.IsNullOrEmpty(context.Request["query"]))
            {
                query = context.Request["query"];
            }

            Paging<ComboList> glCompanyCodes = GLCompanyCodes.GLCodePaging(start, limit, query);

            context.Response.Write(string.Format("{{total:{1},'glcodes':{0}}}", JSON.Serialize(glCompanyCodes.Data), glCompanyCodes.TotalRecords));
        }

        public static Paging<ComboList> GLCodePaging(int start, int limit, string filter)
        {
            List<ComboList> GLCodes = companyGlCodes();

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

        public static List<ComboList> companyGlCodes()
        {
            using (Entities _context = new Entities())
            {
               List<ComboList> glCodes = (from distinct in _context.GL_ACCOUNTS_V
                                          select new { distinct.SEGMENT1,
                   
                   
                   
                   
                   (from accounts in _context.GL_ACCOUNTS_V
                                          group accounts by new { accounts.SEGMENT1, accounts.SEGMENT1_DESC } into grp
                                           select new ComboList { ID = grp.Key.SEGMENT1, Name = grp.Key.SEGMENT1, Description = grp.Key.SEGMENT1_DESC }).OrderBy(o => o.ID).ToList();
                return glCodes;
            }
        }

        public class ComboList
        {
            public ComboList()
            {
            }

            public ComboList(string ID, string Name, string Description)
            {
                _ID = ID;
                _Name = Name;
                _Description = Description;
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

            private string _Description { get; set; }
            public string Description
            {
                get { return _Description; }
                set { _Description = value; }
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
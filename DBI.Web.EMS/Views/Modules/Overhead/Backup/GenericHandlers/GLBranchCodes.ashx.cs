﻿using System;
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
    public class GLBranchCodes : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/json";

            int start = 0;
            int limit = 1000;
            string sort = string.Empty;
            string dir = string.Empty;
            string query = string.Empty;
            string segment1 = string.Empty;
            string segment2 = string.Empty;
            string segment3 = string.Empty;

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

             if (!string.IsNullOrEmpty(context.Request["segment3"]))
             {
                 segment3 = context.Request["segment3"];
             }

            Paging<ComboList> glCompanyCodes = GLBranchCodes.GLCodePaging(start, limit, query, segment1, segment2, segment3);

            context.Response.Write(string.Format("{{total:{1},'glcodes':{0}}}", JSON.Serialize(glCompanyCodes.Data), glCompanyCodes.TotalRecords));
        }

        public static Paging<ComboList> GLCodePaging(int start, int limit, string filter, string segment1, string segment2, string segment3)
        {
            List<ComboList> GLCodes = GlCodes(segment1,segment2,segment3);

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

        public static List<ComboList> GlCodes(string segment1, string segment2, string segment3)
        {
            using (Entities _context = new Entities())
            {
                List<ComboList> glCodes = (from accounts in _context.GL_ACCOUNTS_V.Where(a => a.SEGMENT1 == segment1).Where(a => a.SEGMENT2 == segment2).Where(a => a.SEGMENT3 == segment3)
                                           select new ComboList { ID = accounts.SEGMENT4, Name = accounts.SEGMENT4}).Distinct().ToList();
                return glCodes.OrderBy(a => a.ID).ToList();
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
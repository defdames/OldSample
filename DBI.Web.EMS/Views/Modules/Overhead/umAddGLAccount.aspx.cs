using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Data;
using Ext.Net;
using System.Data.Entity;
using System.Collections;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public partial class umAddGLAccount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        public List<string> distinctGLBySegment(string segment)
        {

            using (Entities _context = new Entities())
            {
                return _context.GL_ACCOUNTS_V.Select(a => a.SEGMENT1).Distinct().ToList();
            }
        }


        protected void deFilterEvents(object sender, DirectEventArgs e)
        {
            uxGlAccountSecurityStore.RemoveAll();
            uxGlAccountSecurityStore.ClearFilter();
            uxGlAccountSecurityStore.Reload();
        }


        protected void deReadGLSecurityCodes(object sender, StoreReadDataEventArgs e)
        {
            string segment1 = e.Parameters["SEGMENT1"].ToString();
            string segment2 = e.Parameters["SEGMENT2"].ToString();
            string segment3 = e.Parameters["SEGMENT3"].ToString();
            string segment4 = e.Parameters["SEGMENT4"].ToString();

                using (Entities _context = new Entities())
                {
                    var data = _context.GL_ACCOUNTS_V.AsNoTracking().Where(a => a.SEGMENT1 == segment1);
                        data = data.Where(a => a.SEGMENT2 == segment2);
                        data = data.Where(a => a.SEGMENT3 == segment3);
                        data = data.Where(a => a.SEGMENT4 == segment4);

                    var dataFilter = data.ToList<GL_ACCOUNTS_V>();
                    int count;
                    uxGlAccountSecurityStore.DataSource = GenericData.EnumerableFilterHeader<GL_ACCOUNTS_V>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], dataFilter, out count);
                    e.Total = data.Count();
                }

        }

        protected void deAddSelectedGlCodes(object sender, DirectEventArgs e)
        {
            long organizationID = long.Parse(Request.QueryString["OrgID"]);

            CheckboxSelectionModel model = uxGlAccountSecurityGridSelectionModel;

            foreach(SelectedRow row in model.SelectedRows)
            {
               OVERHEAD_GL_ACCOUNT account = new OVERHEAD_GL_ACCOUNT();
                account.CODE_COMBO_ID = long.Parse(row.RecordID);
                account.OVERHEAD_ORG_ID = organizationID;
                account.CREATE_DATE = DateTime.Now;
                account.MODIFY_DATE = DateTime.Now;
                account.CREATED_BY = HttpContext.Current.User.Identity.ToString();
                account.MODIFIED_BY = HttpContext.Current.User.Identity.ToString();
                GenericData.Insert<OVERHEAD_GL_ACCOUNT>(account);
            }

        }

    }
}
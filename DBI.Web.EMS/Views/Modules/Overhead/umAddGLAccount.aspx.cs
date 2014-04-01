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

      
        protected void deReadGLSecurityCodes(object sender, StoreReadDataEventArgs e)
        {

            using (Entities _context = new Entities())
                {
                    var data = _context.GL_ACCOUNTS_V.AsNoTracking().ToList<GL_ACCOUNTS_V>();
                    int count;
                    uxGlAccountSecurityStore.DataSource = GenericData.EnumerableFilterHeader<GL_ACCOUNTS_V>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                    e.Total = data.Count();
                }
            }
        }
}
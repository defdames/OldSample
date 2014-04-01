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
            string segment1 = uxSegment1.SelectedItem.Value.ToString();
            string segment2 = uxSegment2.SelectedItem.Value.ToString();
            string segment3 = uxSegment3.SelectedItem.Value.ToString();
            string segment4 = uxSegment4.SelectedItem.Value.ToString();

            if (segment4.Length > 0)
            {
                using (Entities _context = new Entities())
                {
                    var data = _context.GL_ACCOUNTS_V.AsNoTracking().Where(a => a.SEGMENT1 == segment1).Where(a => a.SEGMENT2 == segment2).Where(a => a.SEGMENT3 == segment3).Where(a => a.SEGMENT4 == segment4).ToList<GL_ACCOUNTS_V>();
                    int count;
                    uxGlAccountSecurityStore.DataSource = GenericData.EnumerableFilterHeader<GL_ACCOUNTS_V>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                    e.Total = data.Count();
                }
            }

        }

    }
}
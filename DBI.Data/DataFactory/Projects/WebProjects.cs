using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ext.Net;

namespace DBI.Data
{
    public partial class WEB_PROJECTS_V
    {
        public static List<WEB_PROJECTS_V> ProjectList(int OrganizationId = 0)
        {
            using (Entities _context = new Entities())
            {
                //todo Update Organization ID to claim
                var data = from p in _context.PROJECTS_V
                            where p.PROJECT_TYPE == "CUSTOMER BILLING" && p.TEMPLATE_FLAG == "N" && p.PROJECT_STATUS_CODE == "APPROVED"
                            select p;
                if (OrganizationId != 0)
                {
                    data = data.Where(p => p.CARRYING_OUT_ORGANIZATION_ID == OrganizationId);
                }
                
                var dataList = data.ToList();
                
                List<WEB_PROJECTS_V> returnList = new List<WEB_PROJECTS_V>();
                foreach (PROJECTS_V item in dataList)
                {
                    WEB_PROJECTS_V rItem = new WEB_PROJECTS_V();
                    rItem.LONG_NAME = item.LONG_NAME;
                    rItem.ORGANIZATION_NAME = item.ORGANIZATION_NAME;
                    rItem.SEGMENT1 = item.SEGMENT1;

                    returnList.Add(rItem);
                }
                return returnList;
            }
        }

        /// <summary>
        /// Returns a list of project information from oracle that will be used in lookups for comboboxes
        /// </summary>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="sort"></param>
        /// <param name="filter"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static Ext.Net.Paging<WEB_PROJECTS_V> ProjectLookup(int start, int limit, string sort, string dir, string filter)
        {
            var dataIn = ProjectList();

            return GenericData.PagingFilter<WEB_PROJECTS_V>(start, limit, sort, dir, filter, dataIn, "ORGANIZATION_NAME");
        }
    }
}

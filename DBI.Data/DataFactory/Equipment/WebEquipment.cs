using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data
{
    public partial class WEB_EQUIPMENT_V
    {
        public static List<WEB_EQUIPMENT_V> ListEquipment(List<long> OrganizationId)
        {
            using(Entities _context = new Entities())
            {
                List<WEB_EQUIPMENT_V> returnList = new List<WEB_EQUIPMENT_V>();
                var data = from p in _context.CLASS_CODES_V
                                                where p.PROJECT_TYPE == "TRUCK & EQUIPMENT"
                                                select p;
                data = data.Where(p => OrganizationId.Contains(p.CARRYING_OUT_ORGANIZATION_ID));

                List<CLASS_CODES_V> projectList = data.ToList();

                foreach (CLASS_CODES_V project in projectList)
                {
                    WEB_EQUIPMENT_V rItem = new WEB_EQUIPMENT_V();
                    rItem.CLASS_CODE = project.CLASS_CODE;
                    rItem.NAME = project.NAME;
                    rItem.ORGANIZATION_NAME = project.ORGANIZATION_NAME;
                    rItem.ORG_ID = Double.Parse(project.ORG_ID.ToString());
                    rItem.ORGANIZATION_ID = project.CARRYING_OUT_ORGANIZATION_ID;
                    rItem.PROJECT_ID = project.PROJECT_ID;
                    rItem.PROJECT_STATUS_CODE = project.PROJECT_STATUS_CODE;
                    rItem.SEGMENT1 = project.SEGMENT1;

                    returnList.Add(rItem);
                }
                return returnList;
            }
        }

        public static List<WEB_EQUIPMENT_V> ListEquipment(int OrganizationId = 0)
        {
            using (Entities _context = new Entities())
            {
                List<WEB_EQUIPMENT_V> returnList = new List<WEB_EQUIPMENT_V>();
                var data = from p in _context.CLASS_CODES_V
                           where p.PROJECT_TYPE == "TRUCK & EQUIPMENT"
                           select p;
                if (OrganizationId != 0)
                {
                    data = data.Where(p => p.CARRYING_OUT_ORGANIZATION_ID == OrganizationId);
                }

                List<CLASS_CODES_V> projectList = data.ToList();

                foreach (CLASS_CODES_V project in projectList)
                {
                    WEB_EQUIPMENT_V rItem = new WEB_EQUIPMENT_V();
                    rItem.CLASS_CODE = project.CLASS_CODE;
                    rItem.NAME = project.NAME;
                    rItem.ORGANIZATION_NAME = project.ORGANIZATION_NAME;
                    rItem.ORG_ID = Double.Parse(project.ORG_ID.ToString());
                    rItem.ORGANIZATION_ID = project.CARRYING_OUT_ORGANIZATION_ID;
                    rItem.PROJECT_ID = project.PROJECT_ID;
                    rItem.PROJECT_STATUS_CODE = project.PROJECT_STATUS_CODE;
                    rItem.SEGMENT1 = project.SEGMENT1;

                    returnList.Add(rItem);
                }
                return returnList;
            }
        }
    }
}

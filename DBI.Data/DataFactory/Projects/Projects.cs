using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data
{
    public partial class PROJECTS_V
    {

        public static List<PROJECTS_V> Projects()
        {
            using (Entities _context = new Entities())
            {
                return _context.Set<PROJECTS_V>().ToList();
            }
        }

        public static String ClassCodeByProjectId(long projectId)
        {
            using (Entities _context = new Entities())
            {
                return _context.Set<CLASS_CODES_V>().Where(c =>c.PROJECT_ID == projectId).Select(c => c.CLASS_CODE).SingleOrDefault();
            }

        }


    }
}

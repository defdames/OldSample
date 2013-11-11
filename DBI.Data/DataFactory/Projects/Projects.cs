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

        


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data
{
    public partial class OVERHEAD_GL_ACCOUNT
    {

        public static List<GL_ACCOUNT_V> overheadAccountsByOrganizationId(long organizationID)
        {
            using (Entities _context = new Entities())
            {
                var data = (from gl in _context.OVERHEAD_GL_ACCOUNT.Where(c => c.OVERHEAD_ORG_ID == organizationID)
                            join gla in _context.GL_ACCOUNTS_V on gl.CODE_COMBO_ID equals gla.CODE_COMBINATION_ID
                            select new GL_ACCOUNT_V
                            {
                                OVERHEAD_GL_ID = (long)gl.OVERHEAD_GL_ID,
                                CODE_COMBINATION_ID = gla.CODE_COMBINATION_ID,
                                SEGMENT1 = gla.SEGMENT1,
                                SEGMENT2 = gla.SEGMENT2,
                                SEGMENT3 = gla.SEGMENT3,
                                SEGMENT4 = gla.SEGMENT4,
                                SEGMENT5 = gla.SEGMENT5,
                                SEGMENT6 = gla.SEGMENT6,
                                SEGMENT7 = gla.SEGMENT7,
                                SEGMENT5DESC = gla.SEGMENT5_DESC,
                                SEGMENT1DESC = gla.SEGMENT1_DESC,
                                SEGMENT2DESC = gla.SEGMENT2_DESC,
                                SEGMENT3DESC = gla.SEGMENT3_DESC,
                                SEGMENT4DESC = gla.SEGMENT4_DESC,
                                SEGMENT6DESC = gla.SEGMENT5_DESC,
                                SEGMENT7DESC = gla.SEGMENT7_DESC
                            }).ToList();

                return data;
            }
        }

        public static void deleteOverheadGLAccountByID(long deleteId)
        {
            using (Entities _context = new Entities())
            {
                OVERHEAD_GL_ACCOUNT account = _context.OVERHEAD_GL_ACCOUNT.Where(a => a.OVERHEAD_GL_ID == deleteId).SingleOrDefault();
                GenericData.Delete<OVERHEAD_GL_ACCOUNT>(account);
            }
        }

        public static int countOverheadGLAccountsByOrganizationId(long organizationId)
        {
            using (Entities _context = new Entities())
            {
                int cnt = _context.OVERHEAD_GL_ACCOUNT.Where(a => a.OVERHEAD_ORG_ID == organizationId).Count();
                return cnt;
            }
        }



        public class GL_ACCOUNT_V
        {
            public long OVERHEAD_GL_ID { get; set; }
            public long CODE_COMBINATION_ID { get; set; }
            public string SEGMENT1 { get; set; }
            public string SEGMENT2 { get; set; }
            public string SEGMENT3 { get; set; }
            public string SEGMENT4 { get; set; }
            public string SEGMENT5 { get; set; }
            public string SEGMENT6 { get; set; }
            public string SEGMENT7 { get; set; }
            public string SEGMENT1DESC { get; set; }
            public string SEGMENT2DESC { get; set; }
            public string SEGMENT3DESC { get; set; }
            public string SEGMENT4DESC { get; set; }
            public string SEGMENT5DESC { get; set; }
            public string SEGMENT6DESC { get; set; }
            public string SEGMENT7DESC { get; set; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public partial class umBudgetTypes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LoadLegalEntitiesTreePanel(object sender, NodeLoadEventArgs e)
        {
            Entities _context = new Entities();

            string sql = @"select organization_id,name as organization_name from apps.hr_all_organization_units where type = 'LE' and ((sysdate between date_from and date_to) or (date_to is null))
            order by 2";

            //Load LEs
            if (e.NodeID == "0")
            {
                var data = _context.Database.SqlQuery<LEGAL_ENTITY>(sql).Select(a => new { a.ORGANIZATION_ID, a.ORGANIZATION_NAME }).Distinct().ToList();

                //Build the treepanel
                foreach (var view in data)
                {
                    //Create the Hierarchy Levels
                    Node node = new Node();
                    node.Text = view.ORGANIZATION_NAME;
                    node.NodeID = view.ORGANIZATION_ID.ToString();
                    node.Leaf = true;
                    e.Nodes.Add(node);
                }
            } 
        }

        protected void deReadBudgetTypesByOrganization(object sender, StoreReadDataEventArgs e)
        {
            long _organizationID;

            RowSelectionModel selection = uxLegalEntityTreeSelectionModel;
            Boolean check = long.TryParse(selection.SelectedRecordID, out _organizationID);

            if (_organizationID > 0)
            {
                using (Entities _context = new Entities())
                {
                    var data = (from gl in _context.OVERHEAD_GL_ACCOUNT.Where(c => c.OVERHEAD_ORG_ID == _organizationID)
                                join gla in _context.GL_ACCOUNTS_V on gl.CODE_COMBO_ID equals gla.CODE_COMBINATION_ID
                                select new GL_ACCOUNT_VIEW
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
                    int count;
                    uxGlAccountSecurityStore.DataSource = GenericData.EnumerableFilterHeader<GL_ACCOUNT_VIEW>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                    e.Total = count;
                }
            }
        }


        public class LEGAL_ENTITY
        {
            public long ORGANIZATION_ID { get; set; }
            public string ORGANIZATION_NAME { get; set; }
        } 
    }
}
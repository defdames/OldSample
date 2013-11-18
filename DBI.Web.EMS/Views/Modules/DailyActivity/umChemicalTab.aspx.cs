using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.DailyActivity
{
    public partial class umChemicalTab : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetGridData();
        }

        protected void GetGridData()
        {
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            using (Entities _context = new Entities())
            {
                var data = (from d in _context.DAILY_ACTIVITY_CHEMICAL_MIX
                            where d.HEADER_ID == HeaderId
                            orderby d.CHEMICAL_MIX_NUMBER
                            select d).ToList();
                uxCurrentChemicalStore.DataSource = data;
            }
        }
        protected void deAddChemical(object sender, DirectEventArgs e)
        {

        }

        protected void deEditChemicalForm(object sender, DirectEventArgs e)
        {
            string JsonValues = e.ExtraParams["ChemicalInfo"];
            Dictionary<string, string>[] ChemicalInfo = JSON.Deserialize<Dictionary<string, string>[]>(JsonValues);
        }

        protected void deRemoveChemical(object sender, DirectEventArgs e)
        {

        }

        protected void deEditChemical(object sender, DirectEventArgs e)
        {

        }
    }
}
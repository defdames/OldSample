using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using DBI.Core.Security;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.DailyActivity
{
    public partial class umChoosePerDiem : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FillComboBox(Request.QueryString["HeaderList"]);
        }

        protected void FillComboBox(string HeaderList)
        {
            Dictionary<string, string>[] Headers = JSON.Deserialize<Dictionary<string, string>[]>(HeaderList);

            List<HeaderDetails> HeaderComboStore = new List<HeaderDetails>();

            foreach (Dictionary<string, string> Header in Headers)
            {
                long HeaderId = long.Parse(Header["HEADER_ID"]);
                string LongName = Header["LONG_NAME"].ToString();

                HeaderComboStore.Add(new HeaderDetails()
                {
                    HEADER_ID = HeaderId,
                    LONG_NAME = LongName
                });
            }
            uxChoosePerDiemHeaderIdStore.DataSource = HeaderComboStore;
        }

        protected void deUpdatePerDiem(object sender, DirectEventArgs e)
        {
            long HeaderId = long.Parse(uxChoosePerDiemHeaderId.SelectedItem.Value.ToString());
            DAILY_ACTIVITY_EMPLOYEE RecordToUpdate;
            using (Entities _context = new Entities())
            {
                 RecordToUpdate = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                                          where d.HEADER_ID == HeaderId
                                                          select d).Single();
                RecordToUpdate.PER_DIEM = "Y";

            }

            GenericData.Update<DAILY_ACTIVITY_EMPLOYEE>(RecordToUpdate);

            X.Js.Call("parent.App['uxSubmitActivityWindow'].show()");

        }
    }

    public class HeaderDetails
    {
        public long HEADER_ID { get; set; }
        public string LONG_NAME { get; set; }
        public long PERSON_ID { get; set; }
    }
}
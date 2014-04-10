using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using System.Security.Claims;
using DBI.Core.Security;
using DBI.Data;
using Ext.Net;
using DBI.Data.DataFactory;

namespace DBI.Web.EMS.Views.Modules.TimeClock
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            GetTimeRecord();

        }


        protected void deSetTimeIn(object sender, DirectEventArgs e)
        {
            //Insert person_id time_in and supervisor_id.  Also set some feilds to default values


            SYS_USER_INFORMATION data;
            decimal person_id = Convert.ToDecimal(Authentication.GetClaimValue("PersonId", User as ClaimsPrincipal));
            uxTime_InTextBox.Text = DateTime.Now.ToString();
            uxTimeInButton.Disabled = true;
            uxTimeOutButton.Enabled = true;

            using (Entities _context = new Entities())
            {
                data = (from su in _context.SYS_USER_INFORMATION
                        where su.PERSON_ID == person_id
                        select su).Single();

            }

            TIME_CLOCK Record = new TIME_CLOCK
            {
                PERSON_ID = person_id,
                TIME_IN = Convert.ToDateTime(uxTime_InTextBox.Text),
                APPROVED = "N",
                COMPLETED = "N",
                SUBMITTED = "N",
                SUPERVISOR_ID = data.SUPERVISOR_ID,

            };
            GenericData.Insert<TIME_CLOCK>(Record);
        }

        protected void deSetTimeOut(object sender, DirectEventArgs e)
        {   //Insert Time_out and set compelted flag to Y

            TIME_CLOCK data;

            uxTime_OutTextBox.Text = DateTime.Now.ToString();
            uxTimeOutButton.Disabled = true;
            decimal person_id = Convert.ToDecimal(Authentication.GetClaimValue("PersonId", User as ClaimsPrincipal));

            using (Entities _context = new Entities())
            {


                data = (from tc in _context.TIME_CLOCK
                        where tc.PERSON_ID == person_id && tc.COMPLETED == "N"
                        select tc).SingleOrDefault();
            }
            data.TIME_OUT = Convert.ToDateTime(uxTime_OutTextBox.Text);
            data.COMPLETED = "Y";

            GenericData.Update<TIME_CLOCK>(data);


        }


        protected void deGetTimeRecord(object sender, DirectEventArgs e)
        {
            GetTimeRecord();

        }

        protected void GetTimeRecord()
        {       //Check for an get exsisting record so user can clock out


            decimal person_id = Decimal.Parse(Authentication.GetClaimValue("PersonId", User as ClaimsPrincipal));

            TIME_CLOCK data;
            using (Entities _context = new Entities())
            {

                data = (from tc in _context.TIME_CLOCK
                        where tc.PERSON_ID == person_id && tc.COMPLETED == "N"
                        select tc).SingleOrDefault();

            }

            if (data != null)
            {
                uxTime_InTextBox.Text = data.TIME_IN.ToString();
                uxTimeInButton.Disabled = true;
            }



        }
    }
}
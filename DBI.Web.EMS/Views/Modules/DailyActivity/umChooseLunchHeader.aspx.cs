using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Data;
using DBI.Core.Web;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.DailyActivity
{
    public partial class umChooseLunchHeader : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            long HeaderId = long.Parse(Request.QueryString["HeaderID"]);
        }

        protected void deReadLunchHeaders(object sender, StoreReadDataEventArgs e)
        {
            long EmployeeId = long.Parse(Request.QueryString["EmployeeId"]);
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            using (Entities _context = new Entities())
            {
                DateTime HeaderDate = (from d in _context.DAILY_ACTIVITY_HEADER
                                       where d.HEADER_ID == HeaderId
                                       select (DateTime)d.DA_DATE).Single();
                List<DAILY_ACTIVITY_HEADER> HeaderList = (from em in _context.DAILY_ACTIVITY_EMPLOYEE
                                                          join d in _context.DAILY_ACTIVITY_HEADER on em.HEADER_ID equals d.HEADER_ID
                                                          where em.EMPLOYEE_ID == EmployeeId && EntityFunctions.TruncateTime(d.DA_DATE) == EntityFunctions.TruncateTime(HeaderDate)
                                                          select d).ToList();
                List<LunchInfo> LunchList = new List<LunchInfo>();
                foreach (DAILY_ACTIVITY_HEADER Header in HeaderList)
                {
                    string ProjectName = (from p in _context.PROJECTS_V
                                          where p.PROJECT_ID == Header.PROJECT_ID
                                          select p.LONG_NAME).Single();
                    LunchList.Add(new LunchInfo
                    {
                        HeaderId = Header.HEADER_ID,
                        ProjectTask = string.Format("{0} (DRS Id: {1})", ProjectName, Header.HEADER_ID.ToString())
                    });
                
                }
                
                uxLunchHeaderStore.DataSource = LunchList;

            }
        }

        protected void deReadLunchTasks(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                long HeaderId = long.Parse(e.Parameters["HeaderId"]);
                long ProjectId = _context.DAILY_ACTIVITY_HEADER.Where(x => x.HEADER_ID == HeaderId).Select(x => (long)x.PROJECT_ID).Single();

                List<PA_TASKS_V> TaskList = _context.PA_TASKS_V.Where(x => x.PROJECT_ID == ProjectId).ToList();
                uxLunchTaskStore.DataSource = TaskList;
            }
        }

        protected void deStoreLunchChoice(object sender, DirectEventArgs e)
        {
            long HeaderId = long.Parse(Request.QueryString["HeaderId"]);
            long EmployeeId = long.Parse(Request.QueryString["EmployeeId"]);
            long ChosenLunch = long.Parse(uxLunchHeader.Value.ToString());
            DAILY_ACTIVITY_EMPLOYEE ExistingLunch;
            DAILY_ACTIVITY_EMPLOYEE EmployeeToUpdate;

            using (Entities _context = new Entities())
            {
                //Get Person Id
                long PersonId = _context.DAILY_ACTIVITY_EMPLOYEE.Where(x => x.EMPLOYEE_ID == EmployeeId).Select(x => x.PERSON_ID).Single();

                //Get Lunch date
                DateTime HeaderDate = _context.DAILY_ACTIVITY_HEADER.Where(x => x.HEADER_ID == HeaderId).Select(x => (DateTime)x.DA_DATE).Single();
                //Check for existing lunch
                ExistingLunch = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                 join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                                 where d.PERSON_ID == PersonId && EntityFunctions.TruncateTime(h.DA_DATE) == EntityFunctions.TruncateTime(HeaderDate) && d.LUNCH == "Y"
                                 select d).SingleOrDefault();
                if (ExistingLunch != null)
                {
                    ExistingLunch.LUNCH_LENGTH = null;
                    ExistingLunch.LUNCH = null;
                }
                //Create New Lunch Record
                EmployeeToUpdate = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                    where d.HEADER_ID == ChosenLunch && d.EMPLOYEE_ID == EmployeeId
                                    select d).Single();
                EmployeeToUpdate.LUNCH = "Y";
                EmployeeToUpdate.LUNCH_LENGTH = GetLunchLength(PersonId, HeaderDate);
            }
            GenericData.Update<DAILY_ACTIVITY_EMPLOYEE>(ExistingLunch);
            GenericData.Update<DAILY_ACTIVITY_EMPLOYEE>(EmployeeToUpdate);

            X.Js.Call("parent.App.uxPlaceholderWindow.hide()");
        }

        protected int GetLunchLength(long PersonId, DateTime HeaderDate)
        {
            using (Entities _context = new Entities())
            {
                var TotalMinutes = (from d in _context.DAILY_ACTIVITY_EMPLOYEE
                                    join h in _context.DAILY_ACTIVITY_HEADER on d.HEADER_ID equals h.HEADER_ID
                                    where EntityFunctions.TruncateTime(h.DA_DATE) == EntityFunctions.TruncateTime(HeaderDate) && d.PERSON_ID == PersonId && h.STATUS != 5
                                    group d by new { d.PERSON_ID } into g
                                    select new { g.Key.PERSON_ID, TotalMinutes = g.Sum(d => EntityFunctions.DiffMinutes(d.TIME_IN.Value, d.TIME_OUT.Value)), TravelTime = g.Sum(d => d.TRAVEL_TIME), DriveTime = g.Sum(d => d.DRIVE_TIME) }).Single();

                decimal TotalTime = (decimal)TotalMinutes.TotalMinutes;
                try
                {
                    TotalTime = TotalTime - ((decimal)TotalMinutes.TravelTime * 60) - ((decimal)TotalMinutes.DriveTime * 60);
                }
                catch { }
                if (TotalTime >= 308 && TotalTime < 728)
                {
                    return 30;
                }
                else
                {
                    return 60;
                }
            }
        }
    }
}

public class LunchInfo
{
    public long HeaderId { get; set; }
    public string ProjectTask { get; set; }
}
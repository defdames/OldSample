using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using DBI.Data;
using Newtonsoft.Json;

namespace DBI.Mobile.EMS.Controllers
{
    public class DARSubmitController : ApiController
    {
        [Authorize]
        [HttpPost]
        public void Post(HttpRequestMessage req)
        {
            string jsonString = req.Content.ReadAsStringAsync().Result;

            System.IO.StreamWriter file2 = new System.IO.StreamWriter("c:\\temp\\json.txt");
            file2.Write(jsonString);
            file2.Close();

            try
            {
                var jsonObj = JsonConvert.DeserializeObject<DailyActivityResponse.RootObject>(jsonString);
                


                DAILY_ACTIVITY_HEADER h = new DAILY_ACTIVITY_HEADER();
                foreach (DailyActivityResponse.DailyActivityHeader j in jsonObj.daily_activity_header)
                {
                        h.PROJECT_ID = j.project_id;
                        h.DA_DATE = DateTime.ParseExact(j.da_date, "dd-MMM-yyyy", null);
                        h.SUBDIVISION = j.subdivision;
                        h.CONTRACTOR = j.contractor;
                        h.PERSON_ID = j.person_id;
                        h.LICENSE = j.license;
                        h.STATE = j.state;
                        h.APPLICATION_TYPE = j.application_type;
                        h.DENSITY = j.density;
                        h.CREATE_DATE = DateTime.Now;
                        h.MODIFY_DATE = DateTime.Now;
                        h.CREATED_BY = "EMSMOBILE";
                        h.MODIFIED_BY = "EMSMOBILE";
                        h.STATUS = 1;
                        GenericData.Insert<DAILY_ACTIVITY_HEADER>(h);
                }

                foreach (DailyActivityResponse.DailyActivityEquipment j in jsonObj.daily_activity_equipment)
                {
                    DAILY_ACTIVITY_EQUIPMENT e = new DAILY_ACTIVITY_EQUIPMENT();
                    e.HEADER_ID = h.HEADER_ID;
                    e.PROJECT_ID = j.project_id;
                    e.ODOMETER_START = j.odometer_start;
                    e.ODOMETER_END = j.odometer_end;
                    e.CREATE_DATE = DateTime.Now;
                    e.MODIFY_DATE = DateTime.Now;
                    e.CREATED_BY = "EMSMOBILE";
                    e.MODIFIED_BY = "EMSMOBILE";
                    GenericData.Insert<DAILY_ACTIVITY_EQUIPMENT>(e);
                }

                foreach (DailyActivityResponse.DailyActivityEmployee j in jsonObj.daily_activity_employee)
                {
                    DAILY_ACTIVITY_EMPLOYEE e = new DAILY_ACTIVITY_EMPLOYEE();
                    e.HEADER_ID = h.HEADER_ID;
                    e.PERSON_ID = j.person_id;
                    e.TIME_IN = DateTime.ParseExact(j.time_in, "dd-MMM-yyyy HH:mm", null);
                    e.TIME_OUT = DateTime.ParseExact(j.time_out, "dd-MMM-yyyy HH:mm", null);
                    e.TRAVEL_TIME = j.travel_time;
                    e.DRIVE_TIME = j.drive_time;
                    e.PER_DIEM = j.per_diem;
                    e.COMMENTS = j.comments;
                    e.FOREMAN_LICENSE = j.foreman_license;
                    e.CREATE_DATE = DateTime.Now;
                    e.MODIFY_DATE = DateTime.Now;
                    e.CREATED_BY = "EMSMOBILE";
                    e.MODIFIED_BY = "EMSMOBILE";

                    if (j.equipment_id > 0)
                    {
                        Entities _context = new Entities();
                        DAILY_ACTIVITY_EQUIPMENT m = _context.DAILY_ACTIVITY_EQUIPMENT.Where(s => s.PROJECT_ID == j.equipment_id & s.HEADER_ID == h.HEADER_ID).SingleOrDefault();
                        e.EQUIPMENT_ID = m.EQUIPMENT_ID;
                    }
 
                    GenericData.Insert<DAILY_ACTIVITY_EMPLOYEE>(e);
                }

                foreach (DailyActivityResponse.DailyActivityProduction j in jsonObj.daily_activity_production)
                {
                    DAILY_ACTIVITY_PRODUCTION e = new DAILY_ACTIVITY_PRODUCTION();
                    e.HEADER_ID = h.HEADER_ID;
                    e.TIME_IN = DateTime.ParseExact(j.time_in, "dd-MMM-yyyy HH:mm", null);
                    e.TIME_OUT = DateTime.ParseExact(j.time_out, "dd-MMM-yyyy HH:mm", null);
                    e.TASK_ID = j.task_id;
                    e.WORK_AREA = j.work_area;
                    e.POLE_FROM = j.pole_from;
                    e.POLE_TO  = j.pole_to;
                    e.ACRES_MILE = j.acres_mile;
                    e.GALLONS = j.gallons;
                    e.CREATE_DATE = DateTime.Now;
                    e.MODIFY_DATE = DateTime.Now;
                    e.CREATED_BY = "EMSMOBILE";
                    e.MODIFIED_BY = "EMSMOBILE";
                    GenericData.Insert<DAILY_ACTIVITY_PRODUCTION>(e);
                }

                foreach (DailyActivityResponse.DailyActivityWeather j in jsonObj.daily_activity_weather)
                {
                    DAILY_ACTIVITY_WEATHER e = new DAILY_ACTIVITY_WEATHER();
                    e.HEADER_ID = h.HEADER_ID;
                    e.WEATHER_DATE_TIME = DateTime.ParseExact(j.weather_date_time, "dd-MMM-yyyy HH:mm", null);
                    e.TEMP = j.temp.ToString();
                    e.WIND_DIRECTION = j.wind_direction;
                    e.WIND_VELOCITY = j.wind_velocity;
                    e.HUMIDITY = j.humidity.ToString();
                    e.COMMENTS = j.comments;
                    e.CREATE_DATE = DateTime.Now;
                    e.MODIFY_DATE = DateTime.Now;
                    e.CREATED_BY = "EMSMOBILE";
                    e.MODIFIED_BY = "EMSMOBILE";
                    GenericData.Insert<DAILY_ACTIVITY_WEATHER>(e);
                }

                foreach (DailyActivityResponse.DailyActivityChemicalMix j in jsonObj.daily_activity_chemical_mix)
                {
                    DAILY_ACTIVITY_CHEMICAL_MIX e = new DAILY_ACTIVITY_CHEMICAL_MIX();
                    e.HEADER_ID = h.HEADER_ID;
                    e.CHEMICAL_MIX_NUMBER = j.chemical_mix_number;
                    e.TARGET_AREA = j.target_area;
                    e.GALLON_ACRE = j.gallon_acre;
                    e.GALLON_STARTING = j.gallon_starting;
                    e.GALLON_MIXED = j.gallon_mixed;
                    e.GALLON_REMAINING = j.gallon_remaining;
                    e.ACRES_SPRAYED = decimal.Parse(j.acres_sprayed.ToString());
                    e.STATE = j.state;
                    e.COUNTY = j.county;
                    e.CREATE_DATE = DateTime.Now;
                    e.MODIFY_DATE = DateTime.Now;
                    e.CREATED_BY = "EMSMOBILE";
                    e.MODIFIED_BY = "EMSMOBILE";
                    GenericData.Insert<DAILY_ACTIVITY_CHEMICAL_MIX>(e);
                }

                foreach (DailyActivityResponse.DailyActivityInventory j in jsonObj.daily_activity_inventory)
                {
                    DAILY_ACTIVITY_INVENTORY e = new DAILY_ACTIVITY_INVENTORY();
                    e.HEADER_ID = h.HEADER_ID;
                    e.SUB_INVENTORY_SECONDARY_NAME = j.sub_inventory_secondary_name;
                    e.SUB_INVENTORY_ORG_ID = j.sub_inventory_org_id;
                    e.ITEM_ID = j.item_id;
                    e.RATE = j.rate;
                    e.UNIT_OF_MEASURE = j.unit_of_measure;
                    e.EPA_NUMBER = j.epa_number;
                    e.CREATE_DATE = DateTime.Now;
                    e.MODIFY_DATE = DateTime.Now;
                    e.CREATED_BY = "EMSMOBILE";
                    e.MODIFIED_BY = "EMSMOBILE";

                    if (j.chemical_mix_id > 0)
                    {
                        Entities _context = new Entities();
                        DAILY_ACTIVITY_CHEMICAL_MIX m = _context.DAILY_ACTIVITY_CHEMICAL_MIX.Where(s => s.CHEMICAL_MIX_NUMBER == j.chemical_mix_id & s.HEADER_ID == h.HEADER_ID).SingleOrDefault();
                        e.CHEMICAL_MIX_ID = m.CHEMICAL_MIX_ID;
                    }


                    GenericData.Insert<DAILY_ACTIVITY_INVENTORY>(e);
                }

                foreach (DailyActivityResponse.DailyActivityFooter j in jsonObj.daily_activity_footer)
                {
                    DAILY_ACTIVITY_FOOTER e = new DAILY_ACTIVITY_FOOTER();
                    e.HEADER_ID = h.HEADER_ID;
                    e.REASON_FOR_NO_WORK = j.reason_for_no_work;
                    e.HOTEL_NAME = j.hotel_name;
                    e.HOTEL_CITY = j.hotel_city;
                    e.HOTEL_STATE = j.hotel_state;
                    e.HOTEL_PHONE = j.hotel_phone;
                    e.FOREMAN_SIGNATURE = Convert.FromBase64String(j.foreman_signature);
                    e.CONTRACT_REP = Convert.FromBase64String(j.contract_rep);
                    e.CONTRACT_REP_NAME = j.contract_rep_name;
                    e.CREATE_DATE = DateTime.Now;
                    e.MODIFY_DATE = DateTime.Now;
                    e.CREATED_BY = "EMSMOBILE";
                    e.MODIFIED_BY = "EMSMOBILE";
                    GenericData.Insert<DAILY_ACTIVITY_FOOTER>(e);
                }

            
            }
            catch (Exception ex)
            {
                System.IO.StreamWriter file3 = new System.IO.StreamWriter("c:\\temp\\error.txt");
                file3.Write(ex.ToString());
                file3.Close();
            }


         



        }

    }
}

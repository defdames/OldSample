﻿using System;
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

            IEnumerable<string> headerValues = req.Headers.GetValues("DeviceID");
            var id = headerValues.FirstOrDefault();

            //DEBUG TESTING
            //System.IO.StreamWriter file2 = new System.IO.StreamWriter("c:\\temp\\" + id.ToString() + ".txt");
            //file2.Write(jsonString);
            //file2.Close();

            var jsonObj = new DailyActivityResponse.RootObject();

            try
            {
                jsonObj = JsonConvert.DeserializeObject<DailyActivityResponse.RootObject>(jsonString);
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            DAILY_ACTIVITY_HEADER h = new DAILY_ACTIVITY_HEADER();
            List<DAILY_ACTIVITY_EQUIPMENT> eq = new List<DAILY_ACTIVITY_EQUIPMENT>();
            List<DAILY_ACTIVITY_EMPLOYEE> em = new List<DAILY_ACTIVITY_EMPLOYEE>();
            List<DAILY_ACTIVITY_PRODUCTION> pr = new List<DAILY_ACTIVITY_PRODUCTION>();
            List<DAILY_ACTIVITY_WEATHER> we = new List<DAILY_ACTIVITY_WEATHER>();
            List<DAILY_ACTIVITY_CHEMICAL_MIX> cm = new List<DAILY_ACTIVITY_CHEMICAL_MIX>();
            List<DAILY_ACTIVITY_INVENTORY> iv = new List<DAILY_ACTIVITY_INVENTORY>();
            DAILY_ACTIVITY_FOOTER f = new DAILY_ACTIVITY_FOOTER();

            try
            {

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
                        h.CREATED_BY = j.created_by.ToUpper();
                        h.MODIFIED_BY = j.created_by.ToUpper();
                        h.STATUS = 1;
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
                    e.CREATED_BY = h.CREATED_BY;
                    e.MODIFIED_BY = h.CREATED_BY;
                    eq.Add(e);
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
                    e.CREATED_BY = h.CREATED_BY;
                    e.MODIFIED_BY = h.CREATED_BY;
                    e.ROLE_TYPE = j.role_type;
                    e.STATE = j.state;
                    e.COUNTY = j.county;

                    if (j.equipment_id > 0)
                    {
                        Entities _context = new Entities();
                        DAILY_ACTIVITY_EQUIPMENT m = _context.DAILY_ACTIVITY_EQUIPMENT.Where(s => s.PROJECT_ID == j.equipment_id & s.HEADER_ID == h.HEADER_ID).FirstOrDefault();
                        e.EQUIPMENT_ID = m.EQUIPMENT_ID;
                    }
                    em.Add(e);
                }

                foreach (DailyActivityResponse.DailyActivityProduction j in jsonObj.daily_activity_production)
                {
                    DAILY_ACTIVITY_PRODUCTION e = new DAILY_ACTIVITY_PRODUCTION();
                    e.HEADER_ID = h.HEADER_ID; 
                    e.TASK_ID = j.task_id;
                    e.WORK_AREA = j.work_area;
                    e.POLE_FROM = j.pole_from;
                    e.POLE_TO  = j.pole_to;
                    e.ACRES_MILE = j.acres_mile;
                    e.QUANTITY = j.quantity;
                    e.UNIT_OF_MEASURE = j.uom;
                    if (j.bill_rate.ToString().Length > 0)
                    {
                        e.BILL_RATE = (Decimal)j.bill_rate;
                    }
                    e.STATION = j.station;
                    e.EXPENDITURE_TYPE = j.expenditure_type;
                    e.COMMENTS = j.comments;
                    e.CREATE_DATE = DateTime.Now;
                    e.MODIFY_DATE = DateTime.Now;
                    e.CREATED_BY = h.CREATED_BY;
                    e.MODIFIED_BY = h.CREATED_BY;
                    pr.Add(e);
                }

                foreach (DailyActivityResponse.DailyActivityWeather j in jsonObj.daily_activity_weather)
                {
                    DAILY_ACTIVITY_WEATHER e = new DAILY_ACTIVITY_WEATHER();
                    e.HEADER_ID = h.HEADER_ID;
                    if (j.weather_date_time.Length > 0)
                    {
                        e.WEATHER_DATE_TIME = DateTime.ParseExact(j.weather_date_time, "dd-MMM-yyyy HH:mm", null);
                    }
                    e.TEMP = j.temp.ToString();
                    e.WIND_DIRECTION = j.wind_direction;
                    e.WIND_VELOCITY = j.wind_velocity;
                    e.HUMIDITY = j.humidity.ToString();
                    e.COMMENTS = j.comments;
                    e.CREATE_DATE = DateTime.Now;
                    e.MODIFY_DATE = DateTime.Now;
                    e.CREATED_BY = h.CREATED_BY;
                    e.MODIFIED_BY = h.CREATED_BY;
                    we.Add(e);
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
                    e.CREATED_BY = h.CREATED_BY;
                    e.MODIFIED_BY = h.CREATED_BY;
                    cm.Add(e);
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
                    e.CREATED_BY = h.CREATED_BY;
                    e.MODIFIED_BY = h.CREATED_BY;

                    if (j.chemical_mix_id > 0)
                    {
                        Entities _context = new Entities();
                        DAILY_ACTIVITY_CHEMICAL_MIX m = _context.DAILY_ACTIVITY_CHEMICAL_MIX.Where(s => s.CHEMICAL_MIX_NUMBER == j.chemical_mix_id & s.HEADER_ID == h.HEADER_ID).FirstOrDefault();
                        e.CHEMICAL_MIX_ID = m.CHEMICAL_MIX_ID;
                    }
                    iv.Add(e);
                }

                foreach (DailyActivityResponse.DailyActivityFooter j in jsonObj.daily_activity_footer)
                {
                    f.HEADER_ID = h.HEADER_ID;
                    f.HOTEL_NAME = j.hotel_name;
                    f.HOTEL_CITY = j.hotel_city;
                    f.HOTEL_STATE = j.hotel_state;
                    f.HOTEL_PHONE = j.hotel_phone;

                    f.DOT_REP = Convert.FromBase64String(j.dot_rep);
                    f.DOT_REP_NAME = j.dot_rep_name;
                    f.COMMENTS = j.comments;
                    f.FOREMAN_SIGNATURE = Convert.FromBase64String(j.foreman_signature);
                    f.CONTRACT_REP = Convert.FromBase64String(j.contract_rep);
                    f.CONTRACT_REP_NAME = j.contract_rep_name;
                    f.CREATE_DATE = DateTime.Now;
                    f.MODIFY_DATE = DateTime.Now;
                    f.CREATED_BY = h.CREATED_BY;
                    f.MODIFIED_BY = h.CREATED_BY;
                }

            
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
               //DEBUG TESTING
               //System.IO.StreamWriter file3 = new System.IO.StreamWriter("c:\\temp\\error.txt");
               //file3.Write(ex.ToString());
               //file3.Close();
            }

            // Perform Save
            try
            {
                GenericData.Insert<DAILY_ACTIVITY_HEADER>(h);
                foreach (DAILY_ACTIVITY_EQUIPMENT e in eq)
                {
                    GenericData.Insert<DAILY_ACTIVITY_EQUIPMENT>(e);
                }
                foreach (DAILY_ACTIVITY_EMPLOYEE e in em)
                {
                    GenericData.Insert<DAILY_ACTIVITY_EMPLOYEE>(e);
                }
                foreach (DAILY_ACTIVITY_PRODUCTION e in pr)
                {
                    GenericData.Insert<DAILY_ACTIVITY_PRODUCTION>(e);
                }
                foreach (DAILY_ACTIVITY_WEATHER e in we)
                {
                    GenericData.Insert<DAILY_ACTIVITY_WEATHER>(e);
                }
                foreach (DAILY_ACTIVITY_CHEMICAL_MIX e in cm)
                {
                    GenericData.Insert<DAILY_ACTIVITY_CHEMICAL_MIX>(e);
                }
                foreach (DAILY_ACTIVITY_INVENTORY e in iv)
                {
                    GenericData.Insert<DAILY_ACTIVITY_INVENTORY>(e);
                }
                GenericData.Insert<DAILY_ACTIVITY_FOOTER>(f);
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
                throw;
            }



        }

    }
}

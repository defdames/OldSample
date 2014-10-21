using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DBI.Data;

namespace DBI.Mobile.EMS.Models
{
    public class DailyActivityResponse
    {
        public class DailyActivityChemicalMix
        {
            public string county { get; set; }
            public decimal gallon_acre { get; set; }
            public decimal gallon_starting { get; set; }
            public decimal gallon_remaining { get; set; }
            public int chemical_mix_number { get; set; }
            public string state { get; set; }
            public decimal gallon_mixed { get; set; }
            public decimal acres_sprayed { get; set; }
            public string target_area { get; set; }
        }

        public class DailyActivityInventory
        {
            public string epa_number { get; set; }
            public int item_id { get; set; }
            public int sub_inventory_org_id { get; set; }
            public string unit_of_measure { get; set; }
            public decimal rate { get; set; }
            public int chemical_mix_id { get; set; }
            public string sub_inventory_secondary_name { get; set; }
            public decimal total { get; set; }
            public string contractorSupplied { get; set; }
        }

        public class DailyActivityFooter
        {
            public string hotel_city { get; set; }
            public string dot_rep { get; set; }
            public string foreman_signature { get; set; }
            public string hotel_name { get; set; }
            public string comments { get; set; }
            public string contract_rep { get; set; }
            public string hotel_phone { get; set; }
            public string dot_rep_name { get; set; }
            public string hotel_state { get; set; }
            public string contract_rep_name { get; set; }
            public string contract_rep_email { get; set; }
            public string dot_rep_email { get; set; }
        }

        public class DailyActivityWeather
        {
            public string temp { get; set; }
            public string comments { get; set; }
            public string wind_direction { get; set; }
            public string wind_velocity { get; set; }
            public string humidity { get; set; }
            public string weather_date_time { get; set; }
        }

        public class DailyActivityProduction
        {
            public string uom { get; set; }
            public decimal acres_mile { get; set; }
            public string pole_from { get; set; }
            public string pole_to { get; set; }
            public decimal quantity { get; set; }
            public int task_id { get; set; }
            public decimal bill_rate { get; set; }
            public string station { get; set; }
            public string expenditure_type { get; set; }
            public string time_out { get; set; }
            public string comments { get; set; }
            public string work_area { get; set; }
            public string time_in { get; set; }
            public string surface_type { get; set; }
        }

        public class DailyActivityHeader
        {
            public string license { get; set; }
            public int project_id { get; set; }
            public string created_by { get; set; }
            public int person_id { get; set; }
            public string da_date { get; set; }
            public string application_type { get; set; }
            public string contractor { get; set; }
            public string state { get; set; }
            public int status { get; set; }
            public string density { get; set; }
            public string subdivision { get; set; }
        }

        public class DailyActivityEquipment
        {
            public int project_id { get; set; }
            public int odometer_start { get; set; }
            public int odometer_end { get; set; }
        }

        public class DailyActivityEmployee
        {
            public decimal drive_time { get; set; }
            public int equipment_id { get; set; }
            public string state { get; set; }
            public string foreman_license { get; set; }
            public string county { get; set; }
            public decimal travel_time { get; set; }
            public string role_type { get; set; }
            public string time_out { get; set; }
            public string comments { get; set; }
            public string per_diem { get; set; }
            public int person_id { get; set; }
            public string time_in { get; set; }
            public decimal shoptime_am { get; set; }
            public decimal shoptime_pm { get; set; }
        }

        public class DailyActivityPhoto
        {
            public string image_data { get; set; }
            public string gps_lon { get; set; }
            public string mime_type { get; set; }
            public string image_date { get; set; }
            public string size { get; set; }
            public string gps_lat { get; set; }
        }

        public class RootObject
        {
            public List<DailyActivityChemicalMix> daily_activity_chemical_mix { get; set; }
            public List<DailyActivityInventory> daily_activity_inventory { get; set; }
            public List<DailyActivityFooter> daily_activity_footer { get; set; }
            public List<DailyActivityWeather> daily_activity_weather { get; set; }
            public List<DailyActivityProduction> daily_activity_production { get; set; }
            public List<DailyActivityHeader> daily_activity_header { get; set; }
            public List<DailyActivityEquipment> daily_activity_equipment { get; set; }
            public List<DailyActivityEmployee> daily_activity_employee { get; set; }
            public List<DailyActivityPhoto> daily_activity_photos { get; set; }
        }


    }
}
//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DBI.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class DAILY_ACTIVITY_EMPLOYEE
    {
        public long EMPLOYEE_ID { get; set; }
        public long HEADER_ID { get; set; }
        public int PERSON_ID { get; set; }
        public Nullable<long> EQUIPMENT_ID { get; set; }
        public Nullable<System.DateTime> TIME_IN { get; set; }
        public Nullable<System.DateTime> TIME_OUT { get; set; }
        public Nullable<decimal> TRAVEL_TIME { get; set; }
        public Nullable<decimal> DRIVE_TIME { get; set; }
        public string PER_DIEM { get; set; }
        public string COMMENTS { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public Nullable<System.DateTime> MODIFY_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public string MODIFIED_BY { get; set; }
        public string FOREMAN_LICENSE { get; set; }
        public string ROLE_TYPE { get; set; }
        public string STATE { get; set; }
        public string COUNTY { get; set; }
    
        public virtual DAILY_ACTIVITY_HEADER DAILY_ACTIVITY_HEADER { get; set; }
    }
}

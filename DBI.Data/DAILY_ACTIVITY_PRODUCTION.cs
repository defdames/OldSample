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
    
    public partial class DAILY_ACTIVITY_PRODUCTION
    {
        public long PRODUCTION_ID { get; set; }
        public long HEADER_ID { get; set; }
        public Nullable<System.DateTime> TIME_IN { get; set; }
        public Nullable<System.DateTime> TIME_OUT { get; set; }
        public Nullable<long> TASK_ID { get; set; }
        public string POLE_FROM { get; set; }
        public string POLE_TO { get; set; }
        public Nullable<decimal> ACRES_MILE { get; set; }
        public Nullable<decimal> QUANTITY { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public Nullable<System.DateTime> MODIFY_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public string MODIFIED_BY { get; set; }
        public string STATION { get; set; }
        public string COMMENTS { get; set; }
        public string EXPENDITURE_TYPE { get; set; }
        public Nullable<decimal> BILL_RATE { get; set; }
        public string UNIT_OF_MEASURE { get; set; }
        public string SURFACE_TYPE { get; set; }
        public string WORK_AREA { get; set; }
    
        public virtual DAILY_ACTIVITY_HEADER DAILY_ACTIVITY_HEADER { get; set; }
    }
}

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
    
    public partial class TIME_CLOCK
    {
        public decimal TIME_CLOCK_ID { get; set; }
        public decimal PERSON_ID { get; set; }
        public Nullable<System.DateTime> TIME_IN { get; set; }
        public Nullable<System.DateTime> TIME_OUT { get; set; }
        public Nullable<System.DateTime> MODIFIED_TIME_IN { get; set; }
        public Nullable<System.DateTime> MODIFIED_TIME_OUT { get; set; }
        public Nullable<decimal> ACTUAL_HOURS { get; set; }
        public Nullable<decimal> ADJUSTED_HOURS { get; set; }
        public Nullable<decimal> ADJUSTED_LUNCH { get; set; }
        public string DAY_OF_WEEK { get; set; }
        public string SUBMITTED { get; set; }
        public string APPROVED { get; set; }
        public string COMPLETED { get; set; }
        public string DELETED { get; set; }
        public string DELETED_COMMENTS { get; set; }
        public Nullable<int> SUPERVISOR_ID { get; set; }
        public string MODIFIED_BY { get; set; }
        public Nullable<System.DateTime> MODIFY_DATE { get; set; }
    }
}

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
    
    public partial class CUSTOMER_SURVEY_THRESHOLDS
    {
        public decimal THRESHOLD_ID { get; set; }
        public Nullable<long> ORG_ID { get; set; }
        public Nullable<decimal> SMALL_THRESHOLD { get; set; }
        public Nullable<decimal> LARGE_THRESHOLD1 { get; set; }
        public Nullable<decimal> LARGE_THRESHOLD2 { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public Nullable<System.DateTime> MODIFY_DATE { get; set; }
    }
}

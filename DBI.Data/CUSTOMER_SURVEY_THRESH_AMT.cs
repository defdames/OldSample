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
    
    public partial class CUSTOMER_SURVEY_THRESH_AMT
    {
        public CUSTOMER_SURVEY_THRESH_AMT()
        {
            this.CUSTOMER_SURVEY_THRESHOLDS = new HashSet<CUSTOMER_SURVEY_THRESHOLDS>();
        }
    
        public decimal AMOUNT_ID { get; set; }
        public Nullable<decimal> LOW_DOLLAR_AMT { get; set; }
        public Nullable<decimal> HIGH_DOLLAR_AMT { get; set; }
        public Nullable<long> ORG_ID { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> MODIFY_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
    
        public virtual ICollection<CUSTOMER_SURVEY_THRESHOLDS> CUSTOMER_SURVEY_THRESHOLDS { get; set; }
    }
}

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
    
    public partial class OVERHEAD_BUDGET_DETAIL
    {
        public long BUDGET_DETAIL_ID { get; set; }
        public Nullable<long> CODE_COMBINATION_ID { get; set; }
        public string PERIOD_NAME { get; set; }
        public Nullable<long> PERIOD_NUM { get; set; }
        public Nullable<decimal> AMOUNT { get; set; }
        public Nullable<long> ORG_BUDGET_ID { get; set; }
        public string ACTUALS_IMPORTED_FLAG { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public Nullable<System.DateTime> MODIFY_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public string MODIFIED_BY { get; set; }
        public string ACCOUNT_NOTES { get; set; }
    }
}

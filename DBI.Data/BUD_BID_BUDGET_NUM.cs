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
    
    public partial class BUD_BID_BUDGET_NUM
    {
        public decimal BUDGET_NUM_ID { get; set; }
        public long PROJECT_ID { get; set; }
        public decimal DETAIL_TASK_ID { get; set; }
        public decimal LINE_ID { get; set; }
        public Nullable<decimal> NOV { get; set; }
        public Nullable<decimal> DEC { get; set; }
        public Nullable<decimal> JAN { get; set; }
        public Nullable<decimal> FEB { get; set; }
        public Nullable<decimal> MAR { get; set; }
        public Nullable<decimal> APR { get; set; }
        public Nullable<decimal> MAY { get; set; }
        public Nullable<decimal> JUN { get; set; }
        public Nullable<decimal> JUL { get; set; }
        public Nullable<decimal> AUG { get; set; }
        public Nullable<decimal> SEP { get; set; }
        public Nullable<decimal> OCT { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> MODIFY_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
    
        public virtual BUD_BID_DETAIL_TASK BUD_BID_DETAIL_TASK { get; set; }
        public virtual BUD_BID_LINES BUD_BID_LINES { get; set; }
    }
}

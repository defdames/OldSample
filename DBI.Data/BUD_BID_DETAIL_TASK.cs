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
    
    public partial class BUD_BID_DETAIL_TASK
    {
        public BUD_BID_DETAIL_TASK()
        {
            this.BUD_BID_ACTUAL_NUM = new HashSet<BUD_BID_ACTUAL_NUM>();
            this.BUD_BID_BUDGET_NUM = new HashSet<BUD_BID_BUDGET_NUM>();
            this.BUD_BID_DETAIL_SHEET = new HashSet<BUD_BID_DETAIL_SHEET>();
        }
    
        public decimal DETAIL_TASK_ID { get; set; }
        public long PROJECT_ID { get; set; }
        public string DETAIL_NAME { get; set; }
        public Nullable<decimal> SHEET_ORDER { get; set; }
        public string COMMENTS { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> MODIFY_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public string TASK_ID { get; set; }
        public string TASK_TYPE { get; set; }
        public string LIABILITY { get; set; }
    
        public virtual ICollection<BUD_BID_ACTUAL_NUM> BUD_BID_ACTUAL_NUM { get; set; }
        public virtual ICollection<BUD_BID_BUDGET_NUM> BUD_BID_BUDGET_NUM { get; set; }
        public virtual ICollection<BUD_BID_DETAIL_SHEET> BUD_BID_DETAIL_SHEET { get; set; }
    }
}

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
    
    public partial class DAILY_ACTIVITY_CHEMICAL_MIX
    {
        public DAILY_ACTIVITY_CHEMICAL_MIX()
        {
            this.DAILY_ACTIVITY_INVENTORY = new HashSet<DAILY_ACTIVITY_INVENTORY>();
        }
    
        public long CHEMICAL_MIX_ID { get; set; }
        public long CHEMICAL_MIX_NUMBER { get; set; }
        public long HEADER_ID { get; set; }
        public string TARGET_AREA { get; set; }
        public Nullable<decimal> GALLON_ACRE { get; set; }
        public Nullable<decimal> GALLON_STARTING { get; set; }
        public Nullable<decimal> GALLON_MIXED { get; set; }
        public Nullable<decimal> GALLON_REMAINING { get; set; }
        public Nullable<decimal> ACRES_SPRAYED { get; set; }
        public string STATE { get; set; }
        public string COUNTY { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public Nullable<System.DateTime> MODIFY_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public string MODIFIED_BY { get; set; }
    
        public virtual DAILY_ACTIVITY_HEADER DAILY_ACTIVITY_HEADER { get; set; }
        public virtual ICollection<DAILY_ACTIVITY_INVENTORY> DAILY_ACTIVITY_INVENTORY { get; set; }
    }
}

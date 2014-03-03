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
    
    public partial class DAILY_ACTIVITY_HEADER
    {
        public DAILY_ACTIVITY_HEADER()
        {
            this.DAILY_ACTIVITY_CHEMICAL_MIX = new HashSet<DAILY_ACTIVITY_CHEMICAL_MIX>();
            this.DAILY_ACTIVITY_EQUIPMENT = new HashSet<DAILY_ACTIVITY_EQUIPMENT>();
            this.DAILY_ACTIVITY_WEATHER = new HashSet<DAILY_ACTIVITY_WEATHER>();
            this.DAILY_ACTIVITY_AUDIT = new HashSet<DAILY_ACTIVITY_AUDIT>();
            this.DAILY_ACTIVITY_INVENTORY = new HashSet<DAILY_ACTIVITY_INVENTORY>();
            this.DAILY_ACTIVITY_PRODUCTION = new HashSet<DAILY_ACTIVITY_PRODUCTION>();
            this.DAILY_ACTIVITY_FOOTER = new HashSet<DAILY_ACTIVITY_FOOTER>();
            this.DAILY_ACTIVITY_EMPLOYEE = new HashSet<DAILY_ACTIVITY_EMPLOYEE>();
        }
    
        public long HEADER_ID { get; set; }
        public Nullable<long> PROJECT_ID { get; set; }
        public Nullable<System.DateTime> DA_DATE { get; set; }
        public string SUBDIVISION { get; set; }
        public string CONTRACTOR { get; set; }
        public Nullable<int> PERSON_ID { get; set; }
        public string LICENSE { get; set; }
        public string STATE { get; set; }
        public string APPLICATION_TYPE { get; set; }
        public string DENSITY { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public Nullable<System.DateTime> MODIFY_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public string MODIFIED_BY { get; set; }
        public Nullable<int> STATUS { get; set; }
        public string DEVICE_ID { get; set; }
        public Nullable<decimal> DA_HEADER_ID { get; set; }
    
        public virtual ICollection<DAILY_ACTIVITY_CHEMICAL_MIX> DAILY_ACTIVITY_CHEMICAL_MIX { get; set; }
        public virtual ICollection<DAILY_ACTIVITY_EQUIPMENT> DAILY_ACTIVITY_EQUIPMENT { get; set; }
        public virtual ICollection<DAILY_ACTIVITY_WEATHER> DAILY_ACTIVITY_WEATHER { get; set; }
        public virtual ICollection<DAILY_ACTIVITY_AUDIT> DAILY_ACTIVITY_AUDIT { get; set; }
        public virtual ICollection<DAILY_ACTIVITY_INVENTORY> DAILY_ACTIVITY_INVENTORY { get; set; }
        public virtual ICollection<DAILY_ACTIVITY_PRODUCTION> DAILY_ACTIVITY_PRODUCTION { get; set; }
        public virtual ICollection<DAILY_ACTIVITY_FOOTER> DAILY_ACTIVITY_FOOTER { get; set; }
        public virtual ICollection<DAILY_ACTIVITY_EMPLOYEE> DAILY_ACTIVITY_EMPLOYEE { get; set; }
    }
}

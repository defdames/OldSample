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
    
    public partial class BUD_BID_PROJECTS
    {
        public decimal BUD_BID_PROJECTS_ID { get; set; }
        public long PROJECT_ID { get; set; }
        public string OVERRIDE_PROJ_NAME { get; set; }
        public Nullable<long> ORG_ID { get; set; }
        public Nullable<decimal> YEAR_ID { get; set; }
        public Nullable<decimal> VER_ID { get; set; }
        public Nullable<long> COMPARE_PROJ_NUM { get; set; }
        public Nullable<decimal> STATUS_ID { get; set; }
        public Nullable<decimal> ACRES { get; set; }
        public Nullable<decimal> DAYS { get; set; }
        public Nullable<decimal> OH_ID { get; set; }
        public string APP_TYPE { get; set; }
        public string CHEMICAL_MIX { get; set; }
        public string COMMENTS { get; set; }
        public Nullable<System.DateTime> WE_DATE { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> MODIFY_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
    
        public virtual BUD_BID_STATUS BUD_BID_STATUS { get; set; }
    }
}

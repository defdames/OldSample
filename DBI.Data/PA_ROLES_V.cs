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
    
    public partial class PA_ROLES_V
    {
        public decimal ROWNUM { get; set; }
        public long PROJECT_ID { get; set; }
        public string PROJECT_ROLE_TYPE { get; set; }
        public string STATE { get; set; }
        public string COUNTY { get; set; }
        public string MEANING { get; set; }
        public System.DateTime EFFECTIVE_START_DATE { get; set; }
        public Nullable<System.DateTime> EFFECTIVE_END_DATE { get; set; }
        public System.DateTime LAST_UPDATE_DATE { get; set; }
        public string PROJECT_STATUS_CODE { get; set; }
    }
}

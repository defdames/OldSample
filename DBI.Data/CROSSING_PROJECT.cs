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
    
    public partial class CROSSING_PROJECT
    {
        public decimal PROJ_CORSSING_ID { get; set; }
        public Nullable<long> PROJECT_ID { get; set; }
        public string PROJECT_NAME { get; set; }
        public Nullable<decimal> RAILROAD_ID { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public Nullable<System.DateTime> MODIFY_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public string MODIFIED_BY { get; set; }
    
        public virtual CROSSING_RAILROAD CROSSING_RAILROAD { get; set; }
    }
}

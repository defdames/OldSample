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
    
    public partial class OVERHEAD_BUDGET_VERSION
    {
        public long OVERHEAD_BUDGET_VER_ID { get; set; }
        public long OVERHEAD_ORG_ID { get; set; }
        public long PARENT_BUDGET_VER_ID { get; set; }
        public long OVERHEAD_BUDGET_TYPE_ID { get; set; }
        public short FISCAL_YEAR { get; set; }
        public string STATUS { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public Nullable<System.DateTime> MODIFY_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public string MODIFIED_BY { get; set; }
    }
}

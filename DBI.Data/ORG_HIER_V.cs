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
    
    public partial class ORG_HIER_V
    {
        public string PARENT_ORG { get; set; }
        public long ORG_ID_PARENT { get; set; }
        public long ORG_ID_CHILD { get; set; }
        public string ORG_HIER { get; set; }
        public long HIERARCHY_ID { get; set; }
        public Nullable<decimal> LEVEL_SORT { get; set; }
        public long ORG_ID { get; set; }
        public string TYPE { get; set; }
        public string BU_ORG { get; set; }
    }
}

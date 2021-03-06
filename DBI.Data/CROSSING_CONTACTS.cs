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
    
    public partial class CROSSING_CONTACTS
    {
        public CROSSING_CONTACTS()
        {
            this.CROSSINGS = new HashSet<CROSSING>();
        }
    
        public long CONTACT_ID { get; set; }
        public string CONTACT_NAME { get; set; }
        public string ADDRESS_1 { get; set; }
        public string ADDRESS_2 { get; set; }
        public string CITY { get; set; }
        public string STATE { get; set; }
        public string ZIP_CODE { get; set; }
        public string WORK_NUMBER { get; set; }
        public string CELL_NUMBER { get; set; }
        public string RAILROAD { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public Nullable<System.DateTime> MODIFY_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public string MODIFIED_BY { get; set; }
        public Nullable<decimal> RAILROAD_ID { get; set; }
    
        public virtual CROSSING_RAILROAD CROSSING_RAILROAD { get; set; }
        public virtual ICollection<CROSSING> CROSSINGS { get; set; }
    }
}

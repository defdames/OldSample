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
    
    public partial class SYS_USERS
    {
        public SYS_USERS()
        {
            this.SYS_USER_INFORMATION = new HashSet<SYS_USER_INFORMATION>();
            this.SYS_USER_ACTIVITY = new HashSet<SYS_USER_ACTIVITY>();
            this.SYS_LOG = new HashSet<SYS_LOG>();
        }
    
        public long USER_ID { get; set; }
        public Nullable<System.DateTime> LAST_ACTIVITY_DATE { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public Nullable<System.DateTime> MODIFY_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public string MODIFIED_BY { get; set; }
    
        public virtual ICollection<SYS_USER_INFORMATION> SYS_USER_INFORMATION { get; set; }
        public virtual ICollection<SYS_USER_ACTIVITY> SYS_USER_ACTIVITY { get; set; }
        public virtual ICollection<SYS_LOG> SYS_LOG { get; set; }
    }
}

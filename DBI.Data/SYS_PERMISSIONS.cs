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
    
    public partial class SYS_PERMISSIONS
    {
        public SYS_PERMISSIONS()
        {
            this.SYS_GROUPS_PERMS = new HashSet<SYS_GROUPS_PERMS>();
            this.SYS_MODULES = new HashSet<SYS_MODULES>();
            this.SYS_USER_PERMS = new HashSet<SYS_USER_PERMS>();
            this.SYS_MENU = new HashSet<SYS_MENU>();
        }
    
        public decimal PERMISSION_ID { get; set; }
        public string PERMISSION_NAME { get; set; }
        public Nullable<decimal> PARENT_PERM_ID { get; set; }
    
        public virtual ICollection<SYS_GROUPS_PERMS> SYS_GROUPS_PERMS { get; set; }
        public virtual ICollection<SYS_MODULES> SYS_MODULES { get; set; }
        public virtual ICollection<SYS_USER_PERMS> SYS_USER_PERMS { get; set; }
        public virtual ICollection<SYS_MENU> SYS_MENU { get; set; }
    }
}

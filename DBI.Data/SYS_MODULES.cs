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
    
    public partial class SYS_MODULES
    {
        public SYS_MODULES()
        {
            this.SYS_MENU = new HashSet<SYS_MENU>();
            this.SYS_MODULE_PROFILE_OPTIONS = new HashSet<SYS_MODULE_PROFILE_OPTIONS>();
        }
    
        public decimal MODULE_ID { get; set; }
        public string MODULE_NAME { get; set; }
        public decimal PERMISSION_ID { get; set; }
    
        public virtual ICollection<SYS_MENU> SYS_MENU { get; set; }
        public virtual SYS_PERMISSIONS SYS_PERMISSIONS { get; set; }
        public virtual ICollection<SYS_MODULE_PROFILE_OPTIONS> SYS_MODULE_PROFILE_OPTIONS { get; set; }
    }
}

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
    
    public partial class SYS_USER_ROLES
    {
        public long USER_ROLE_ID { get; set; }
        public long USER_ID { get; set; }
        public long ROLE_ID { get; set; }
    
        public virtual SYS_ROLES SYS_ROLES { get; set; }
        public virtual SYS_USERS SYS_USERS { get; set; }
    }
}

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
    
    public partial class SYS_ACTIVITY
    {
        public SYS_ACTIVITY()
        {
            this.SYS_USER_ACTIVITY = new HashSet<SYS_USER_ACTIVITY>();
        }
    
        public long ACTIVITY_ID { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public Nullable<System.DateTime> CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> LAST_UPDATED { get; set; }
        public string LAST_UPDATED_BY { get; set; }
        public string PATH { get; set; }
        public string CONTAINER { get; set; }
        public string ICON { get; set; }
        public string CONTROL_TEXT { get; set; }
        public Nullable<long> PARENT_ITEM_ID { get; set; }
        public long SORT_NUMBER { get; set; }
    
        public virtual ICollection<SYS_USER_ACTIVITY> SYS_USER_ACTIVITY { get; set; }
    }
}

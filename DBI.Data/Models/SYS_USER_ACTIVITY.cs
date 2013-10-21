using System;
using System.Collections.Generic;

namespace DBI.Data.Models
{
    public partial class SYS_USER_ACTIVITY
    {
        public long USER_ACTIVITY_ID { get; set; }
        public long ACTIVITY_ID { get; set; }
        public long USER_ID { get; set; }
        public Nullable<System.DateTime> CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> LAST_UPDATED { get; set; }
        public string LAST_UPDATED_BY { get; set; }
    }
}

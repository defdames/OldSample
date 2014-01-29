using System;
using System.Collections.Generic;

namespace DBI.Data.Models
{
    public partial class SYS_ACTIVITY
    {
        public long ACTIVITY_ID { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public Nullable<System.DateTime> CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> LAST_UPDATED { get; set; }
        public string LAST_UPDATED_BY { get; set; }
    }
}

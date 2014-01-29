using System;
using System.Collections.Generic;

namespace DBI.Data.Models
{
    public partial class SYS_LOG
    {
        public long ID { get; set; }
        public long USER_ID { get; set; }
        public string USER_CULTURE { get; set; }
        public string GUID { get; set; }
        public string MESSAGE { get; set; }
        public string INNER_EXCEPTION { get; set; }
        public string SOURCE { get; set; }
        public string STACKTRACE { get; set; }
        public string DEBUG { get; set; }
        public Nullable<System.DateTime> CREATED_DATE { get; set; }
    }
}

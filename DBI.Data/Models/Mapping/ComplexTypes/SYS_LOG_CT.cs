using System;

namespace DBI.Data
{
    public partial class SYS_LOG_CT
    {
        public long ID { get; set; }
        public string USER_NAME { get; set; }
        public string EMPLOYEE_NAME { get; set; }
        public string USER_CULTURE { get; set; }
        public string MESSAGE { get; set; }
        public string INNER_EXCEPTION { get; set; }
        public string SOURCE { get; set; }
        public string STACKTRACE { get; set; }
        public Nullable<System.DateTime> CREATED_DATE { get; set; }
        public string DEBUG { get; set; }
        public string GUID { get; set; }
    }
}

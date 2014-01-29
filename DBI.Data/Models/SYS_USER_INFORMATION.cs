using System;
using System.Collections.Generic;

namespace DBI.Data.Models
{
    public partial class SYS_USER_INFORMATION
    {
        public long USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public string EMPLOYEE_NAME { get; set; }
        public string EMPLOYEE_NUMBER { get; set; }
        public string EMAIL_ADDRESS { get; set; }
        public string JOB_NAME { get; set; }
        public string CURRENT_ORGANIZATION { get; set; }
        public long CURRENT_ORG_ID { get; set; }
        public string ORACLE_ACCOUNT_STATUS { get; set; }
        public string SUPERVISOR_NAME { get; set; }
        public Nullable<int> SUPERVISOR_ID { get; set; }
        public string SUPERVISOR_EMAIL_ADDRESS { get; set; }
        public string SUPERVISOR_USER_NAME { get; set; }
        public string LOCATION_NAME { get; set; }
    }
}

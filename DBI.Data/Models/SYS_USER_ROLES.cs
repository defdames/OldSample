using System;
using System.Collections.Generic;

namespace DBI.Data.Models
{
    public partial class SYS_USER_ROLES
    {
        public long USER_ROLE_ID { get; set; }
        public long USER_ID { get; set; }
        public long ROLE_ID { get; set; }
    }
}

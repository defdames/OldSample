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
    
    public partial class DAILY_ACTIVITY_FOOTER
    {
        public decimal FOOTER_ID { get; set; }
        public long HEADER_ID { get; set; }
        public string COMMENTS { get; set; }
        public string HOTEL_NAME { get; set; }
        public string HOTEL_CITY { get; set; }
        public string HOTEL_STATE { get; set; }
        public string HOTEL_PHONE { get; set; }
        public byte[] FOREMAN_SIGNATURE { get; set; }
        public byte[] CONTRACT_REP { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public Nullable<System.DateTime> MODIFY_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public string MODIFIED_BY { get; set; }
        public string CONTRACT_REP_NAME { get; set; }
        public byte[] DOT_REP { get; set; }
        public string DOT_REP_NAME { get; set; }
        public string CONTRACT_REP_EMAIL { get; set; }
        public string DOT_REP_EMAIL { get; set; }
    
        public virtual DAILY_ACTIVITY_HEADER DAILY_ACTIVITY_HEADER { get; set; }
    }
}

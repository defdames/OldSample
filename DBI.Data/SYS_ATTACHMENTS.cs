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
    
    public partial class SYS_ATTACHMENTS
    {
        public long ATTACHMENT_ID { get; set; }
        public Nullable<long> MODULE_ID { get; set; }
        public string REFERENCE_TABLE { get; set; }
        public Nullable<long> REFERENCE_NUMBER { get; set; }
        public string ATTACHMENT_DESC { get; set; }
        public string ATTACHMENT_FILENAME { get; set; }
        public string ATTACHMENT_MIME { get; set; }
        public byte[] DATA { get; set; }
        public Nullable<System.DateTime> CREATED_DATE { get; set; }
        public Nullable<System.DateTime> MODIFIED_DATE { get; set; }
        public Nullable<decimal> GPS_LATITUDE { get; set; }
        public Nullable<decimal> GPS_LONGITUDE { get; set; }
        public string CREATED_BY { get; set; }
        public string MODIFIED_BY { get; set; }
    }
}

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
    
    public partial class INVENTORY_V
    {
        public decimal ITEM_ID { get; set; }
        public string SEGMENT1 { get; set; }
        public string DESCRIPTION { get; set; }
        public string UOM_CODE { get; set; }
        public string ENABLED_FLAG { get; set; }
        public string ACTIVE { get; set; }
        public Nullable<decimal> ITEM_COST { get; set; }
        public Nullable<System.DateTime> LAST_UPDATE_DATE { get; set; }
        public string LE { get; set; }
        public string ATTRIBUTE2 { get; set; }
        public string INV_NAME { get; set; }
        public Nullable<long> INV_LOCATION { get; set; }
        public long ORGANIZATION_ID { get; set; }
    }
}

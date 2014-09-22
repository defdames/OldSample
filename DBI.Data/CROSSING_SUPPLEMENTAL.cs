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
    
    public partial class CROSSING_SUPPLEMENTAL
    {
        public long SUPPLEMENTAL_ID { get; set; }
        public long CROSSING_ID { get; set; }
        public string SERVICE_TYPE { get; set; }
        public string SERVICE_UNIT { get; set; }
        public string RAILROAD { get; set; }
        public string SUB_DIVISION { get; set; }
        public Nullable<System.DateTime> APPROVED_DATE { get; set; }
        public Nullable<System.DateTime> COMPLETED_DATE { get; set; }
        public string TRUCK_NUMBER { get; set; }
        public Nullable<decimal> SQUARE_FEET { get; set; }
        public Nullable<System.DateTime> INSPECT_START { get; set; }
        public Nullable<System.DateTime> INSPECT_END { get; set; }
        public string SPRAY { get; set; }
        public string CUT { get; set; }
        public string MAINTAIN { get; set; }
        public string INSPECT { get; set; }
        public string RECURRING { get; set; }
        public string REMARKS { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public Nullable<System.DateTime> MODIFY_DATE { get; set; }
        public Nullable<decimal> INVOICE_SUPP_ID { get; set; }
        public Nullable<System.DateTime> CUT_TIME { get; set; }
        public Nullable<long> PROJECT_ID { get; set; }
    
        public virtual CROSSING CROSSING { get; set; }
    }
}

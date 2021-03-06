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
    
    public partial class CROSSING
    {
        public CROSSING()
        {
            this.CROSSING_DATA_ENTRY = new HashSet<CROSSING_DATA_ENTRY>();
            this.CROSSING_INCIDENT = new HashSet<CROSSING_INCIDENT>();
            this.CROSSING_SUPPLEMENTAL = new HashSet<CROSSING_SUPPLEMENTAL>();
        }
    
        public long CROSSING_ID { get; set; }
        public string DOT { get; set; }
        public string PREF { get; set; }
        public Nullable<decimal> MILE_POST { get; set; }
        public string DIVISION { get; set; }
        public string SUB_DIVISION { get; set; }
        public string CITY { get; set; }
        public string STREET { get; set; }
        public string STATE { get; set; }
        public string COUNTY { get; set; }
        public string RESTRICTED_COUNTY { get; set; }
        public string REMARKS { get; set; }
        public string ROUTE { get; set; }
        public string THIRD_APP_REQUIRED { get; set; }
        public string PRICE_CATEGORY { get; set; }
        public Nullable<long> MAIN_TRACKS { get; set; }
        public Nullable<long> OTHER_TRACKS { get; set; }
        public string ON_SPUR { get; set; }
        public Nullable<long> MAX_SPEED { get; set; }
        public string SUB_CONTRACTED { get; set; }
        public string FENCE_ENCROACHMENT { get; set; }
        public Nullable<decimal> LATITUDE { get; set; }
        public Nullable<decimal> LONGITUDE { get; set; }
        public string PROPERTY_TYPE { get; set; }
        public string WARNING_DEVICE { get; set; }
        public string SURFACE { get; set; }
        public Nullable<decimal> ROWNE { get; set; }
        public Nullable<decimal> ROWNW { get; set; }
        public Nullable<decimal> ROWSE { get; set; }
        public Nullable<decimal> ROWSW { get; set; }
        public string ROW_WIDTH { get; set; }
        public string EXTENSION_REQUIRED { get; set; }
        public Nullable<decimal> EXTNE { get; set; }
        public Nullable<decimal> EXTNW { get; set; }
        public Nullable<decimal> EXTSE { get; set; }
        public Nullable<decimal> EXTSW { get; set; }
        public string TEMP_ACTIVTY { get; set; }
        public string DELETED_CROSSING { get; set; }
        public string DELETED_BY { get; set; }
        public Nullable<System.DateTime> DELETED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> CREATED_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public Nullable<System.DateTime> MODIFIED_DATE { get; set; }
        public string MTM { get; set; }
        public string MTM_PHONE_NUMBER { get; set; }
        public string MTM_OFFICE_NUMBER { get; set; }
        public string CROSSING_NUMBER { get; set; }
        public Nullable<long> CONTACT_ID { get; set; }
        public string RAILROAD { get; set; }
        public string SERVICE_UNIT { get; set; }
        public Nullable<long> PROJECT_ID { get; set; }
        public string SPECIAL_INSTRUCTIONS { get; set; }
        public string STATUS { get; set; }
        public Nullable<decimal> RAILROAD_ID { get; set; }
        public string CUT_ONLY { get; set; }
    
        public virtual CROSSING_CONTACTS CROSSING_CONTACTS { get; set; }
        public virtual ICollection<CROSSING_DATA_ENTRY> CROSSING_DATA_ENTRY { get; set; }
        public virtual ICollection<CROSSING_INCIDENT> CROSSING_INCIDENT { get; set; }
        public virtual CROSSING_RAILROAD CROSSING_RAILROAD { get; set; }
        public virtual ICollection<CROSSING_SUPPLEMENTAL> CROSSING_SUPPLEMENTAL { get; set; }
    }
}

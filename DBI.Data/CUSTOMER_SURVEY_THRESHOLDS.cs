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
    
    public partial class CUSTOMER_SURVEY_THRESHOLDS
    {
        public CUSTOMER_SURVEY_THRESHOLDS()
        {
            this.SURVEY_FORMS_COMP = new HashSet<SURVEY_FORMS_COMP>();
        }
    
        public decimal THRESHOLD_ID { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public Nullable<System.DateTime> MODIFY_DATE { get; set; }
        public decimal AMOUNT_ID { get; set; }
        public decimal THRESHOLD { get; set; }
    
        public virtual CUSTOMER_SURVEY_THRESH_AMT CUSTOMER_SURVEY_THRESH_AMT { get; set; }
        public virtual ICollection<SURVEY_FORMS_COMP> SURVEY_FORMS_COMP { get; set; }
    }
}

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
    
    public partial class CUSTOMER_SURVEY_RELATION
    {
        public decimal RELATION_ID { get; set; }
        public decimal FIELDSET_ID { get; set; }
        public decimal QUESTION_ID { get; set; }
        public Nullable<decimal> SORT_ORDER { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public Nullable<System.DateTime> MODIFY_DATE { get; set; }
    
        public virtual CUSTOMER_SURVEY_FIELDSETS CUSTOMER_SURVEY_FIELDSETS { get; set; }
        public virtual CUSTOMER_SURVEY_QUESTIONS CUSTOMER_SURVEY_QUESTIONS { get; set; }
    }
}

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
    
    public partial class CUSTOMER_SURVEY_FORMS_COMP
    {
        public CUSTOMER_SURVEY_FORMS_COMP()
        {
            this.CUSTOMER_SURVEY_FORMS_ANS = new HashSet<CUSTOMER_SURVEY_FORMS_ANS>();
        }
    
        public decimal COMPLETION_ID { get; set; }
        public decimal FORM_ID { get; set; }
        public string FILLED_BY { get; set; }
        public Nullable<System.DateTime> FILLED_ON { get; set; }
        public string CREATED_BY { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public Nullable<System.DateTime> MODIFY_DATE { get; set; }
    
        public virtual ICollection<CUSTOMER_SURVEY_FORMS_ANS> CUSTOMER_SURVEY_FORMS_ANS { get; set; }
        public virtual CUSTOMER_SURVEY_FORMS CUSTOMER_SURVEY_FORMS { get; set; }
    }
}

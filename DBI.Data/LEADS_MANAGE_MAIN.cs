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
    
    public partial class LEADS_MANAGE_MAIN
    {
        public LEADS_MANAGE_MAIN()
        {
            this.LEADS_COMMENTS = new HashSet<LEADS_COMMENTS>();
            this.LEADS_CONTACTS = new HashSet<LEADS_CONTACTS>();
        }
    
        public decimal LEAD_ID { get; set; }
        public decimal CSR { get; set; }
        public Nullable<System.DateTime> ENTRY_DATE { get; set; }
        public string STATE { get; set; }
        public string COUNTRY { get; set; }
        public string BID_TITLE { get; set; }
        public string BID_SOLICIT_NUMBER { get; set; }
        public string PROSPECT { get; set; }
        public Nullable<long> ORG_ID { get; set; }
        public string DESCRIPTION { get; set; }
        public string PROSPECT_CONTACT { get; set; }
        public string CONTACT_NUMBER { get; set; }
        public string CONTACT_EMAIL { get; set; }
        public Nullable<System.DateTime> POSTING_DATE { get; set; }
        public Nullable<System.DateTime> PRE_BID_DATE { get; set; }
        public Nullable<System.DateTime> LETTING_DATE { get; set; }
        public Nullable<decimal> LEAD_MANANGER { get; set; }
        public Nullable<decimal> BOND { get; set; }
        public Nullable<decimal> SET_ASIDE { get; set; }
        public string BIDDING_Y_N { get; set; }
        public string AWARDED { get; set; }
        public string CONTRACT_TERMS { get; set; }
        public Nullable<decimal> EST_COST { get; set; }
        public string URL_SOURCE { get; set; }
        public string DOC_SOURCE { get; set; }
        public Nullable<decimal> DOC_COST { get; set; }
        public Nullable<System.DateTime> DATE_ORDERED { get; set; }
        public Nullable<System.DateTime> DATE_RECEIVED { get; set; }
        public string DOC_LOGIN { get; set; }
        public string DOC_PASS { get; set; }
        public string BID_TABS { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public Nullable<System.DateTime> MODIFY_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public string MODIFIED_BY { get; set; }
    
        public virtual ICollection<LEADS_COMMENTS> LEADS_COMMENTS { get; set; }
        public virtual ICollection<LEADS_CONTACTS> LEADS_CONTACTS { get; set; }
    }
}

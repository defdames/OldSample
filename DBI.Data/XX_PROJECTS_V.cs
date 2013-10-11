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
    
    public partial class XX_PROJECTS_V
    {
        public long PROJECT_ID { get; set; }
        public string NAME { get; set; }
        public string SEGMENT1 { get; set; }
        public System.DateTime LAST_UPDATE_DATE { get; set; }
        public long LAST_UPDATED_BY { get; set; }
        public System.DateTime CREATION_DATE { get; set; }
        public long CREATED_BY { get; set; }
        public long LAST_UPDATE_LOGIN { get; set; }
        public string PROJECT_TYPE { get; set; }
        public long CARRYING_OUT_ORGANIZATION_ID { get; set; }
        public string PUBLIC_SECTOR_FLAG { get; set; }
        public string PROJECT_STATUS_CODE { get; set; }
        public string DESCRIPTION { get; set; }
        public Nullable<System.DateTime> START_DATE { get; set; }
        public Nullable<System.DateTime> COMPLETION_DATE { get; set; }
        public Nullable<System.DateTime> CLOSED_DATE { get; set; }
        public string DISTRIBUTION_RULE { get; set; }
        public Nullable<long> LABOR_INVOICE_FORMAT_ID { get; set; }
        public Nullable<long> NON_LABOR_INVOICE_FORMAT_ID { get; set; }
        public Nullable<long> RETENTION_INVOICE_FORMAT_ID { get; set; }
        public Nullable<decimal> RETENTION_PERCENTAGE { get; set; }
        public Nullable<long> BILLING_OFFSET { get; set; }
        public Nullable<long> BILLING_CYCLE { get; set; }
        public string LABOR_STD_BILL_RATE_SCHDL { get; set; }
        public Nullable<long> LABOR_BILL_RATE_ORG_ID { get; set; }
        public Nullable<System.DateTime> LABOR_SCHEDULE_FIXED_DATE { get; set; }
        public Nullable<decimal> LABOR_SCHEDULE_DISCOUNT { get; set; }
        public string NON_LABOR_STD_BILL_RATE_SCHDL { get; set; }
        public Nullable<long> NON_LABOR_BILL_RATE_ORG_ID { get; set; }
        public Nullable<System.DateTime> NON_LABOR_SCHEDULE_FIXED_DATE { get; set; }
        public Nullable<decimal> NON_LABOR_SCHEDULE_DISCOUNT { get; set; }
        public string LIMIT_TO_TXN_CONTROLS_FLAG { get; set; }
        public string PROJECT_LEVEL_FUNDING_FLAG { get; set; }
        public string INVOICE_COMMENT { get; set; }
        public Nullable<decimal> UNBILLED_RECEIVABLE_DR { get; set; }
        public Nullable<decimal> UNEARNED_REVENUE_CR { get; set; }
        public Nullable<long> REQUEST_ID { get; set; }
        public Nullable<long> PROGRAM_ID { get; set; }
        public Nullable<long> PROGRAM_APPLICATION_ID { get; set; }
        public Nullable<System.DateTime> PROGRAM_UPDATE_DATE { get; set; }
        public string SUMMARY_FLAG { get; set; }
        public string ENABLED_FLAG { get; set; }
        public string SEGMENT2 { get; set; }
        public string SEGMENT3 { get; set; }
        public string SEGMENT4 { get; set; }
        public string SEGMENT5 { get; set; }
        public string SEGMENT6 { get; set; }
        public string SEGMENT7 { get; set; }
        public string SEGMENT8 { get; set; }
        public string SEGMENT9 { get; set; }
        public string SEGMENT10 { get; set; }
        public string ATTRIBUTE_CATEGORY { get; set; }
        public string ATTRIBUTE1 { get; set; }
        public string ATTRIBUTE2 { get; set; }
        public string ATTRIBUTE3 { get; set; }
        public string ATTRIBUTE4 { get; set; }
        public string ATTRIBUTE5 { get; set; }
        public string ATTRIBUTE6 { get; set; }
        public string ATTRIBUTE7 { get; set; }
        public string ATTRIBUTE8 { get; set; }
        public string ATTRIBUTE9 { get; set; }
        public string ATTRIBUTE10 { get; set; }
        public Nullable<long> COST_IND_RATE_SCH_ID { get; set; }
        public Nullable<long> REV_IND_RATE_SCH_ID { get; set; }
        public Nullable<long> INV_IND_RATE_SCH_ID { get; set; }
        public Nullable<System.DateTime> COST_IND_SCH_FIXED_DATE { get; set; }
        public Nullable<System.DateTime> REV_IND_SCH_FIXED_DATE { get; set; }
        public Nullable<System.DateTime> INV_IND_SCH_FIXED_DATE { get; set; }
        public string LABOR_SCH_TYPE { get; set; }
        public string NON_LABOR_SCH_TYPE { get; set; }
        public Nullable<long> OVR_COST_IND_RATE_SCH_ID { get; set; }
        public Nullable<long> OVR_REV_IND_RATE_SCH_ID { get; set; }
        public Nullable<long> OVR_INV_IND_RATE_SCH_ID { get; set; }
        public string TEMPLATE_FLAG { get; set; }
        public Nullable<System.DateTime> VERIFICATION_DATE { get; set; }
        public Nullable<long> CREATED_FROM_PROJECT_ID { get; set; }
        public Nullable<System.DateTime> TEMPLATE_START_DATE_ACTIVE { get; set; }
        public Nullable<System.DateTime> TEMPLATE_END_DATE_ACTIVE { get; set; }
        public Nullable<long> ORG_ID { get; set; }
        public string PM_PRODUCT_CODE { get; set; }
        public string PM_PROJECT_REFERENCE { get; set; }
        public Nullable<System.DateTime> ACTUAL_START_DATE { get; set; }
        public Nullable<System.DateTime> ACTUAL_FINISH_DATE { get; set; }
        public Nullable<System.DateTime> EARLY_START_DATE { get; set; }
        public Nullable<System.DateTime> EARLY_FINISH_DATE { get; set; }
        public Nullable<System.DateTime> LATE_START_DATE { get; set; }
        public Nullable<System.DateTime> LATE_FINISH_DATE { get; set; }
        public Nullable<System.DateTime> SCHEDULED_START_DATE { get; set; }
        public Nullable<System.DateTime> SCHEDULED_FINISH_DATE { get; set; }
        public Nullable<long> BILLING_CYCLE_ID { get; set; }
        public string ADW_NOTIFY_FLAG { get; set; }
        public string WF_STATUS_CODE { get; set; }
        public string OUTPUT_TAX_CODE { get; set; }
        public string RETENTION_TAX_CODE { get; set; }
        public string PROJECT_CURRENCY_CODE { get; set; }
        public string ALLOW_CROSS_CHARGE_FLAG { get; set; }
        public Nullable<System.DateTime> PROJECT_RATE_DATE { get; set; }
        public string PROJECT_RATE_TYPE { get; set; }
        public string CC_PROCESS_LABOR_FLAG { get; set; }
        public Nullable<decimal> LABOR_TP_SCHEDULE_ID { get; set; }
        public Nullable<System.DateTime> LABOR_TP_FIXED_DATE { get; set; }
        public string CC_PROCESS_NL_FLAG { get; set; }
        public Nullable<decimal> NL_TP_SCHEDULE_ID { get; set; }
        public Nullable<System.DateTime> NL_TP_FIXED_DATE { get; set; }
        public Nullable<decimal> CC_TAX_TASK_ID { get; set; }
        public Nullable<long> BILL_JOB_GROUP_ID { get; set; }
        public Nullable<long> COST_JOB_GROUP_ID { get; set; }
        public Nullable<long> ROLE_LIST_ID { get; set; }
        public Nullable<long> WORK_TYPE_ID { get; set; }
        public Nullable<long> CALENDAR_ID { get; set; }
        public Nullable<long> LOCATION_ID { get; set; }
        public Nullable<long> PROBABILITY_MEMBER_ID { get; set; }
        public Nullable<decimal> PROJECT_VALUE { get; set; }
        public Nullable<System.DateTime> EXPECTED_APPROVAL_DATE { get; set; }
        public Nullable<long> RECORD_VERSION_NUMBER { get; set; }
        public Nullable<long> INITIAL_TEAM_TEMPLATE_ID { get; set; }
        public Nullable<decimal> JOB_BILL_RATE_SCHEDULE_ID { get; set; }
        public Nullable<decimal> EMP_BILL_RATE_SCHEDULE_ID { get; set; }
        public Nullable<decimal> COMPETENCE_MATCH_WT { get; set; }
        public Nullable<decimal> AVAILABILITY_MATCH_WT { get; set; }
        public Nullable<decimal> JOB_LEVEL_MATCH_WT { get; set; }
        public string ENABLE_AUTOMATED_SEARCH { get; set; }
        public Nullable<decimal> SEARCH_MIN_AVAILABILITY { get; set; }
        public Nullable<long> SEARCH_ORG_HIER_ID { get; set; }
        public Nullable<long> SEARCH_STARTING_ORG_ID { get; set; }
        public string SEARCH_COUNTRY_CODE { get; set; }
        public Nullable<decimal> MIN_CAND_SCORE_REQD_FOR_NOM { get; set; }
        public Nullable<long> NON_LAB_STD_BILL_RT_SCH_ID { get; set; }
        public string INVPROC_CURRENCY_TYPE { get; set; }
        public string REVPROC_CURRENCY_CODE { get; set; }
        public string PROJECT_BIL_RATE_DATE_CODE { get; set; }
        public string PROJECT_BIL_RATE_TYPE { get; set; }
        public Nullable<System.DateTime> PROJECT_BIL_RATE_DATE { get; set; }
        public Nullable<decimal> PROJECT_BIL_EXCHANGE_RATE { get; set; }
        public string PROJFUNC_CURRENCY_CODE { get; set; }
        public string PROJFUNC_BIL_RATE_DATE_CODE { get; set; }
        public string PROJFUNC_BIL_RATE_TYPE { get; set; }
        public Nullable<System.DateTime> PROJFUNC_BIL_RATE_DATE { get; set; }
        public Nullable<decimal> PROJFUNC_BIL_EXCHANGE_RATE { get; set; }
        public string FUNDING_RATE_DATE_CODE { get; set; }
        public string FUNDING_RATE_TYPE { get; set; }
        public Nullable<System.DateTime> FUNDING_RATE_DATE { get; set; }
        public Nullable<decimal> FUNDING_EXCHANGE_RATE { get; set; }
        public string BASELINE_FUNDING_FLAG { get; set; }
        public string PROJFUNC_COST_RATE_TYPE { get; set; }
        public Nullable<System.DateTime> PROJFUNC_COST_RATE_DATE { get; set; }
        public string INV_BY_BILL_TRANS_CURR_FLAG { get; set; }
        public string MULTI_CURRENCY_BILLING_FLAG { get; set; }
        public string SPLIT_COST_FROM_WORKPLAN_FLAG { get; set; }
        public string SPLIT_COST_FROM_BILL_FLAG { get; set; }
        public string ASSIGN_PRECEDES_TASK { get; set; }
        public string PRIORITY_CODE { get; set; }
        public Nullable<long> RETN_BILLING_INV_FORMAT_ID { get; set; }
        public string RETN_ACCOUNTING_FLAG { get; set; }
        public Nullable<long> ADV_ACTION_SET_ID { get; set; }
        public string START_ADV_ACTION_SET_FLAG { get; set; }
        public string REVALUATE_FUNDING_FLAG { get; set; }
        public string INCLUDE_GAINS_LOSSES_FLAG { get; set; }
        public Nullable<System.DateTime> TARGET_START_DATE { get; set; }
        public Nullable<System.DateTime> TARGET_FINISH_DATE { get; set; }
        public Nullable<System.DateTime> BASELINE_START_DATE { get; set; }
        public Nullable<System.DateTime> BASELINE_FINISH_DATE { get; set; }
        public Nullable<System.DateTime> SCHEDULED_AS_OF_DATE { get; set; }
        public Nullable<System.DateTime> BASELINE_AS_OF_DATE { get; set; }
        public string LABOR_DISC_REASON_CODE { get; set; }
        public string NON_LABOR_DISC_REASON_CODE { get; set; }
        public Nullable<decimal> SECURITY_LEVEL { get; set; }
        public Nullable<System.DateTime> ACTUAL_AS_OF_DATE { get; set; }
        public Nullable<decimal> SCHEDULED_DURATION { get; set; }
        public Nullable<decimal> BASELINE_DURATION { get; set; }
        public Nullable<decimal> ACTUAL_DURATION { get; set; }
        public string LONG_NAME { get; set; }
        public string BTC_COST_BASE_REV_CODE { get; set; }
        public string ASSET_ALLOCATION_METHOD { get; set; }
        public string CAPITAL_EVENT_PROCESSING { get; set; }
        public Nullable<long> CINT_RATE_SCH_ID { get; set; }
        public string CINT_ELIGIBLE_FLAG { get; set; }
        public Nullable<System.DateTime> CINT_STOP_DATE { get; set; }
        public string SYS_PROGRAM_FLAG { get; set; }
        public string STRUCTURE_SHARING_CODE { get; set; }
        public string ENABLE_TOP_TASK_CUSTOMER_FLAG { get; set; }
        public string ENABLE_TOP_TASK_INV_MTH_FLAG { get; set; }
        public string REVENUE_ACCRUAL_METHOD { get; set; }
        public string INVOICE_METHOD { get; set; }
        public string PROJFUNC_ATTR_FOR_AR_FLAG { get; set; }
        public string PJI_SOURCE_FLAG { get; set; }
        public string ALLOW_MULTI_PROGRAM_ROLLUP { get; set; }
        public Nullable<decimal> PROJ_REQ_RES_FORMAT_ID { get; set; }
        public Nullable<decimal> PROJ_ASGMT_RES_FORMAT_ID { get; set; }
        public string FUNDING_APPROVAL_STATUS_CODE { get; set; }
    }
}

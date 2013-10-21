using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DBI.Data.Models.Mapping
{
    public class PROJECTS_VMap : EntityTypeConfiguration<PROJECTS_V>
    {
        public PROJECTS_VMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ORGANIZATION_NAME, t.PROJECT_ID, t.NAME, t.SEGMENT1, t.LAST_UPDATE_DATE, t.LAST_UPDATED_BY, t.CREATION_DATE, t.CREATED_BY, t.LAST_UPDATE_LOGIN, t.PROJECT_TYPE, t.CARRYING_OUT_ORGANIZATION_ID, t.PUBLIC_SECTOR_FLAG, t.PROJECT_STATUS_CODE, t.SUMMARY_FLAG, t.ENABLED_FLAG, t.PROJECT_CURRENCY_CODE, t.ALLOW_CROSS_CHARGE_FLAG, t.CC_PROCESS_LABOR_FLAG, t.CC_PROCESS_NL_FLAG });

            // Properties
            this.Property(t => t.ORGANIZATION_NAME)
                .IsRequired()
                .HasMaxLength(240);

            this.Property(t => t.PROJECT_ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.NAME)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.SEGMENT1)
                .IsRequired()
                .HasMaxLength(25);

            this.Property(t => t.LAST_UPDATED_BY)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CREATED_BY)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.LAST_UPDATE_LOGIN)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.PROJECT_TYPE)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.CARRYING_OUT_ORGANIZATION_ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.PUBLIC_SECTOR_FLAG)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.PROJECT_STATUS_CODE)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.DESCRIPTION)
                .HasMaxLength(250);

            this.Property(t => t.DISTRIBUTION_RULE)
                .HasMaxLength(30);

            this.Property(t => t.LABOR_STD_BILL_RATE_SCHDL)
                .HasMaxLength(20);

            this.Property(t => t.NON_LABOR_STD_BILL_RATE_SCHDL)
                .HasMaxLength(30);

            this.Property(t => t.LIMIT_TO_TXN_CONTROLS_FLAG)
                .HasMaxLength(1);

            this.Property(t => t.PROJECT_LEVEL_FUNDING_FLAG)
                .HasMaxLength(1);

            this.Property(t => t.INVOICE_COMMENT)
                .HasMaxLength(240);

            this.Property(t => t.SUMMARY_FLAG)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.ENABLED_FLAG)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.SEGMENT2)
                .HasMaxLength(25);

            this.Property(t => t.SEGMENT3)
                .HasMaxLength(25);

            this.Property(t => t.SEGMENT4)
                .HasMaxLength(25);

            this.Property(t => t.SEGMENT5)
                .HasMaxLength(25);

            this.Property(t => t.SEGMENT6)
                .HasMaxLength(25);

            this.Property(t => t.SEGMENT7)
                .HasMaxLength(25);

            this.Property(t => t.SEGMENT8)
                .HasMaxLength(25);

            this.Property(t => t.SEGMENT9)
                .HasMaxLength(25);

            this.Property(t => t.SEGMENT10)
                .HasMaxLength(25);

            this.Property(t => t.ATTRIBUTE_CATEGORY)
                .HasMaxLength(30);

            this.Property(t => t.ATTRIBUTE1)
                .HasMaxLength(150);

            this.Property(t => t.ATTRIBUTE2)
                .HasMaxLength(150);

            this.Property(t => t.ATTRIBUTE3)
                .HasMaxLength(150);

            this.Property(t => t.ATTRIBUTE4)
                .HasMaxLength(150);

            this.Property(t => t.ATTRIBUTE5)
                .HasMaxLength(150);

            this.Property(t => t.ATTRIBUTE6)
                .HasMaxLength(150);

            this.Property(t => t.ATTRIBUTE7)
                .HasMaxLength(150);

            this.Property(t => t.ATTRIBUTE8)
                .HasMaxLength(150);

            this.Property(t => t.ATTRIBUTE9)
                .HasMaxLength(150);

            this.Property(t => t.ATTRIBUTE10)
                .HasMaxLength(150);

            this.Property(t => t.LABOR_SCH_TYPE)
                .HasMaxLength(1);

            this.Property(t => t.NON_LABOR_SCH_TYPE)
                .HasMaxLength(1);

            this.Property(t => t.TEMPLATE_FLAG)
                .HasMaxLength(1);

            this.Property(t => t.PM_PRODUCT_CODE)
                .HasMaxLength(30);

            this.Property(t => t.PM_PROJECT_REFERENCE)
                .HasMaxLength(25);

            this.Property(t => t.ADW_NOTIFY_FLAG)
                .HasMaxLength(1);

            this.Property(t => t.WF_STATUS_CODE)
                .HasMaxLength(30);

            this.Property(t => t.OUTPUT_TAX_CODE)
                .HasMaxLength(50);

            this.Property(t => t.RETENTION_TAX_CODE)
                .HasMaxLength(50);

            this.Property(t => t.PROJECT_CURRENCY_CODE)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.ALLOW_CROSS_CHARGE_FLAG)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.PROJECT_RATE_TYPE)
                .HasMaxLength(30);

            this.Property(t => t.CC_PROCESS_LABOR_FLAG)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.CC_PROCESS_NL_FLAG)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.ENABLE_AUTOMATED_SEARCH)
                .HasMaxLength(1);

            this.Property(t => t.SEARCH_COUNTRY_CODE)
                .HasMaxLength(2);

            this.Property(t => t.INVPROC_CURRENCY_TYPE)
                .HasMaxLength(30);

            this.Property(t => t.REVPROC_CURRENCY_CODE)
                .HasMaxLength(15);

            this.Property(t => t.PROJECT_BIL_RATE_DATE_CODE)
                .HasMaxLength(30);

            this.Property(t => t.PROJECT_BIL_RATE_TYPE)
                .HasMaxLength(30);

            this.Property(t => t.PROJFUNC_CURRENCY_CODE)
                .HasMaxLength(15);

            this.Property(t => t.PROJFUNC_BIL_RATE_DATE_CODE)
                .HasMaxLength(30);

            this.Property(t => t.PROJFUNC_BIL_RATE_TYPE)
                .HasMaxLength(30);

            this.Property(t => t.FUNDING_RATE_DATE_CODE)
                .HasMaxLength(30);

            this.Property(t => t.FUNDING_RATE_TYPE)
                .HasMaxLength(30);

            this.Property(t => t.BASELINE_FUNDING_FLAG)
                .HasMaxLength(1);

            this.Property(t => t.PROJFUNC_COST_RATE_TYPE)
                .HasMaxLength(30);

            this.Property(t => t.INV_BY_BILL_TRANS_CURR_FLAG)
                .HasMaxLength(1);

            this.Property(t => t.MULTI_CURRENCY_BILLING_FLAG)
                .HasMaxLength(1);

            this.Property(t => t.SPLIT_COST_FROM_WORKPLAN_FLAG)
                .HasMaxLength(1);

            this.Property(t => t.SPLIT_COST_FROM_BILL_FLAG)
                .HasMaxLength(1);

            this.Property(t => t.ASSIGN_PRECEDES_TASK)
                .HasMaxLength(1);

            this.Property(t => t.PRIORITY_CODE)
                .HasMaxLength(30);

            this.Property(t => t.RETN_ACCOUNTING_FLAG)
                .HasMaxLength(1);

            this.Property(t => t.START_ADV_ACTION_SET_FLAG)
                .HasMaxLength(1);

            this.Property(t => t.REVALUATE_FUNDING_FLAG)
                .HasMaxLength(1);

            this.Property(t => t.INCLUDE_GAINS_LOSSES_FLAG)
                .HasMaxLength(1);

            this.Property(t => t.LABOR_DISC_REASON_CODE)
                .HasMaxLength(30);

            this.Property(t => t.NON_LABOR_DISC_REASON_CODE)
                .HasMaxLength(30);

            this.Property(t => t.LONG_NAME)
                .HasMaxLength(240);

            this.Property(t => t.BTC_COST_BASE_REV_CODE)
                .HasMaxLength(90);

            this.Property(t => t.ASSET_ALLOCATION_METHOD)
                .HasMaxLength(30);

            this.Property(t => t.CAPITAL_EVENT_PROCESSING)
                .HasMaxLength(30);

            this.Property(t => t.CINT_ELIGIBLE_FLAG)
                .HasMaxLength(1);

            this.Property(t => t.SYS_PROGRAM_FLAG)
                .HasMaxLength(1);

            this.Property(t => t.STRUCTURE_SHARING_CODE)
                .HasMaxLength(30);

            this.Property(t => t.ENABLE_TOP_TASK_CUSTOMER_FLAG)
                .HasMaxLength(1);

            this.Property(t => t.ENABLE_TOP_TASK_INV_MTH_FLAG)
                .HasMaxLength(1);

            this.Property(t => t.REVENUE_ACCRUAL_METHOD)
                .HasMaxLength(30);

            this.Property(t => t.INVOICE_METHOD)
                .HasMaxLength(30);

            this.Property(t => t.PROJFUNC_ATTR_FOR_AR_FLAG)
                .HasMaxLength(1);

            this.Property(t => t.PJI_SOURCE_FLAG)
                .HasMaxLength(1);

            this.Property(t => t.ALLOW_MULTI_PROGRAM_ROLLUP)
                .HasMaxLength(1);

            this.Property(t => t.FUNDING_APPROVAL_STATUS_CODE)
                .HasMaxLength(30);

            // Table & Column Mappings
            this.ToTable("PROJECTS_V", "XXEMS");
            this.Property(t => t.ORGANIZATION_NAME).HasColumnName("ORGANIZATION_NAME");
            this.Property(t => t.PROJECT_ID).HasColumnName("PROJECT_ID");
            this.Property(t => t.NAME).HasColumnName("NAME");
            this.Property(t => t.SEGMENT1).HasColumnName("SEGMENT1");
            this.Property(t => t.LAST_UPDATE_DATE).HasColumnName("LAST_UPDATE_DATE");
            this.Property(t => t.LAST_UPDATED_BY).HasColumnName("LAST_UPDATED_BY");
            this.Property(t => t.CREATION_DATE).HasColumnName("CREATION_DATE");
            this.Property(t => t.CREATED_BY).HasColumnName("CREATED_BY");
            this.Property(t => t.LAST_UPDATE_LOGIN).HasColumnName("LAST_UPDATE_LOGIN");
            this.Property(t => t.PROJECT_TYPE).HasColumnName("PROJECT_TYPE");
            this.Property(t => t.CARRYING_OUT_ORGANIZATION_ID).HasColumnName("CARRYING_OUT_ORGANIZATION_ID");
            this.Property(t => t.PUBLIC_SECTOR_FLAG).HasColumnName("PUBLIC_SECTOR_FLAG");
            this.Property(t => t.PROJECT_STATUS_CODE).HasColumnName("PROJECT_STATUS_CODE");
            this.Property(t => t.DESCRIPTION).HasColumnName("DESCRIPTION");
            this.Property(t => t.START_DATE).HasColumnName("START_DATE");
            this.Property(t => t.COMPLETION_DATE).HasColumnName("COMPLETION_DATE");
            this.Property(t => t.CLOSED_DATE).HasColumnName("CLOSED_DATE");
            this.Property(t => t.DISTRIBUTION_RULE).HasColumnName("DISTRIBUTION_RULE");
            this.Property(t => t.LABOR_INVOICE_FORMAT_ID).HasColumnName("LABOR_INVOICE_FORMAT_ID");
            this.Property(t => t.NON_LABOR_INVOICE_FORMAT_ID).HasColumnName("NON_LABOR_INVOICE_FORMAT_ID");
            this.Property(t => t.RETENTION_INVOICE_FORMAT_ID).HasColumnName("RETENTION_INVOICE_FORMAT_ID");
            this.Property(t => t.RETENTION_PERCENTAGE).HasColumnName("RETENTION_PERCENTAGE");
            this.Property(t => t.BILLING_OFFSET).HasColumnName("BILLING_OFFSET");
            this.Property(t => t.BILLING_CYCLE).HasColumnName("BILLING_CYCLE");
            this.Property(t => t.LABOR_STD_BILL_RATE_SCHDL).HasColumnName("LABOR_STD_BILL_RATE_SCHDL");
            this.Property(t => t.LABOR_BILL_RATE_ORG_ID).HasColumnName("LABOR_BILL_RATE_ORG_ID");
            this.Property(t => t.LABOR_SCHEDULE_FIXED_DATE).HasColumnName("LABOR_SCHEDULE_FIXED_DATE");
            this.Property(t => t.LABOR_SCHEDULE_DISCOUNT).HasColumnName("LABOR_SCHEDULE_DISCOUNT");
            this.Property(t => t.NON_LABOR_STD_BILL_RATE_SCHDL).HasColumnName("NON_LABOR_STD_BILL_RATE_SCHDL");
            this.Property(t => t.NON_LABOR_BILL_RATE_ORG_ID).HasColumnName("NON_LABOR_BILL_RATE_ORG_ID");
            this.Property(t => t.NON_LABOR_SCHEDULE_FIXED_DATE).HasColumnName("NON_LABOR_SCHEDULE_FIXED_DATE");
            this.Property(t => t.NON_LABOR_SCHEDULE_DISCOUNT).HasColumnName("NON_LABOR_SCHEDULE_DISCOUNT");
            this.Property(t => t.LIMIT_TO_TXN_CONTROLS_FLAG).HasColumnName("LIMIT_TO_TXN_CONTROLS_FLAG");
            this.Property(t => t.PROJECT_LEVEL_FUNDING_FLAG).HasColumnName("PROJECT_LEVEL_FUNDING_FLAG");
            this.Property(t => t.INVOICE_COMMENT).HasColumnName("INVOICE_COMMENT");
            this.Property(t => t.UNBILLED_RECEIVABLE_DR).HasColumnName("UNBILLED_RECEIVABLE_DR");
            this.Property(t => t.UNEARNED_REVENUE_CR).HasColumnName("UNEARNED_REVENUE_CR");
            this.Property(t => t.REQUEST_ID).HasColumnName("REQUEST_ID");
            this.Property(t => t.PROGRAM_ID).HasColumnName("PROGRAM_ID");
            this.Property(t => t.PROGRAM_APPLICATION_ID).HasColumnName("PROGRAM_APPLICATION_ID");
            this.Property(t => t.PROGRAM_UPDATE_DATE).HasColumnName("PROGRAM_UPDATE_DATE");
            this.Property(t => t.SUMMARY_FLAG).HasColumnName("SUMMARY_FLAG");
            this.Property(t => t.ENABLED_FLAG).HasColumnName("ENABLED_FLAG");
            this.Property(t => t.SEGMENT2).HasColumnName("SEGMENT2");
            this.Property(t => t.SEGMENT3).HasColumnName("SEGMENT3");
            this.Property(t => t.SEGMENT4).HasColumnName("SEGMENT4");
            this.Property(t => t.SEGMENT5).HasColumnName("SEGMENT5");
            this.Property(t => t.SEGMENT6).HasColumnName("SEGMENT6");
            this.Property(t => t.SEGMENT7).HasColumnName("SEGMENT7");
            this.Property(t => t.SEGMENT8).HasColumnName("SEGMENT8");
            this.Property(t => t.SEGMENT9).HasColumnName("SEGMENT9");
            this.Property(t => t.SEGMENT10).HasColumnName("SEGMENT10");
            this.Property(t => t.ATTRIBUTE_CATEGORY).HasColumnName("ATTRIBUTE_CATEGORY");
            this.Property(t => t.ATTRIBUTE1).HasColumnName("ATTRIBUTE1");
            this.Property(t => t.ATTRIBUTE2).HasColumnName("ATTRIBUTE2");
            this.Property(t => t.ATTRIBUTE3).HasColumnName("ATTRIBUTE3");
            this.Property(t => t.ATTRIBUTE4).HasColumnName("ATTRIBUTE4");
            this.Property(t => t.ATTRIBUTE5).HasColumnName("ATTRIBUTE5");
            this.Property(t => t.ATTRIBUTE6).HasColumnName("ATTRIBUTE6");
            this.Property(t => t.ATTRIBUTE7).HasColumnName("ATTRIBUTE7");
            this.Property(t => t.ATTRIBUTE8).HasColumnName("ATTRIBUTE8");
            this.Property(t => t.ATTRIBUTE9).HasColumnName("ATTRIBUTE9");
            this.Property(t => t.ATTRIBUTE10).HasColumnName("ATTRIBUTE10");
            this.Property(t => t.COST_IND_RATE_SCH_ID).HasColumnName("COST_IND_RATE_SCH_ID");
            this.Property(t => t.REV_IND_RATE_SCH_ID).HasColumnName("REV_IND_RATE_SCH_ID");
            this.Property(t => t.INV_IND_RATE_SCH_ID).HasColumnName("INV_IND_RATE_SCH_ID");
            this.Property(t => t.COST_IND_SCH_FIXED_DATE).HasColumnName("COST_IND_SCH_FIXED_DATE");
            this.Property(t => t.REV_IND_SCH_FIXED_DATE).HasColumnName("REV_IND_SCH_FIXED_DATE");
            this.Property(t => t.INV_IND_SCH_FIXED_DATE).HasColumnName("INV_IND_SCH_FIXED_DATE");
            this.Property(t => t.LABOR_SCH_TYPE).HasColumnName("LABOR_SCH_TYPE");
            this.Property(t => t.NON_LABOR_SCH_TYPE).HasColumnName("NON_LABOR_SCH_TYPE");
            this.Property(t => t.OVR_COST_IND_RATE_SCH_ID).HasColumnName("OVR_COST_IND_RATE_SCH_ID");
            this.Property(t => t.OVR_REV_IND_RATE_SCH_ID).HasColumnName("OVR_REV_IND_RATE_SCH_ID");
            this.Property(t => t.OVR_INV_IND_RATE_SCH_ID).HasColumnName("OVR_INV_IND_RATE_SCH_ID");
            this.Property(t => t.TEMPLATE_FLAG).HasColumnName("TEMPLATE_FLAG");
            this.Property(t => t.VERIFICATION_DATE).HasColumnName("VERIFICATION_DATE");
            this.Property(t => t.CREATED_FROM_PROJECT_ID).HasColumnName("CREATED_FROM_PROJECT_ID");
            this.Property(t => t.TEMPLATE_START_DATE_ACTIVE).HasColumnName("TEMPLATE_START_DATE_ACTIVE");
            this.Property(t => t.TEMPLATE_END_DATE_ACTIVE).HasColumnName("TEMPLATE_END_DATE_ACTIVE");
            this.Property(t => t.ORG_ID).HasColumnName("ORG_ID");
            this.Property(t => t.PM_PRODUCT_CODE).HasColumnName("PM_PRODUCT_CODE");
            this.Property(t => t.PM_PROJECT_REFERENCE).HasColumnName("PM_PROJECT_REFERENCE");
            this.Property(t => t.ACTUAL_START_DATE).HasColumnName("ACTUAL_START_DATE");
            this.Property(t => t.ACTUAL_FINISH_DATE).HasColumnName("ACTUAL_FINISH_DATE");
            this.Property(t => t.EARLY_START_DATE).HasColumnName("EARLY_START_DATE");
            this.Property(t => t.EARLY_FINISH_DATE).HasColumnName("EARLY_FINISH_DATE");
            this.Property(t => t.LATE_START_DATE).HasColumnName("LATE_START_DATE");
            this.Property(t => t.LATE_FINISH_DATE).HasColumnName("LATE_FINISH_DATE");
            this.Property(t => t.SCHEDULED_START_DATE).HasColumnName("SCHEDULED_START_DATE");
            this.Property(t => t.SCHEDULED_FINISH_DATE).HasColumnName("SCHEDULED_FINISH_DATE");
            this.Property(t => t.BILLING_CYCLE_ID).HasColumnName("BILLING_CYCLE_ID");
            this.Property(t => t.ADW_NOTIFY_FLAG).HasColumnName("ADW_NOTIFY_FLAG");
            this.Property(t => t.WF_STATUS_CODE).HasColumnName("WF_STATUS_CODE");
            this.Property(t => t.OUTPUT_TAX_CODE).HasColumnName("OUTPUT_TAX_CODE");
            this.Property(t => t.RETENTION_TAX_CODE).HasColumnName("RETENTION_TAX_CODE");
            this.Property(t => t.PROJECT_CURRENCY_CODE).HasColumnName("PROJECT_CURRENCY_CODE");
            this.Property(t => t.ALLOW_CROSS_CHARGE_FLAG).HasColumnName("ALLOW_CROSS_CHARGE_FLAG");
            this.Property(t => t.PROJECT_RATE_DATE).HasColumnName("PROJECT_RATE_DATE");
            this.Property(t => t.PROJECT_RATE_TYPE).HasColumnName("PROJECT_RATE_TYPE");
            this.Property(t => t.CC_PROCESS_LABOR_FLAG).HasColumnName("CC_PROCESS_LABOR_FLAG");
            this.Property(t => t.LABOR_TP_SCHEDULE_ID).HasColumnName("LABOR_TP_SCHEDULE_ID");
            this.Property(t => t.LABOR_TP_FIXED_DATE).HasColumnName("LABOR_TP_FIXED_DATE");
            this.Property(t => t.CC_PROCESS_NL_FLAG).HasColumnName("CC_PROCESS_NL_FLAG");
            this.Property(t => t.NL_TP_SCHEDULE_ID).HasColumnName("NL_TP_SCHEDULE_ID");
            this.Property(t => t.NL_TP_FIXED_DATE).HasColumnName("NL_TP_FIXED_DATE");
            this.Property(t => t.CC_TAX_TASK_ID).HasColumnName("CC_TAX_TASK_ID");
            this.Property(t => t.BILL_JOB_GROUP_ID).HasColumnName("BILL_JOB_GROUP_ID");
            this.Property(t => t.COST_JOB_GROUP_ID).HasColumnName("COST_JOB_GROUP_ID");
            this.Property(t => t.ROLE_LIST_ID).HasColumnName("ROLE_LIST_ID");
            this.Property(t => t.WORK_TYPE_ID).HasColumnName("WORK_TYPE_ID");
            this.Property(t => t.CALENDAR_ID).HasColumnName("CALENDAR_ID");
            this.Property(t => t.LOCATION_ID).HasColumnName("LOCATION_ID");
            this.Property(t => t.PROBABILITY_MEMBER_ID).HasColumnName("PROBABILITY_MEMBER_ID");
            this.Property(t => t.PROJECT_VALUE).HasColumnName("PROJECT_VALUE");
            this.Property(t => t.EXPECTED_APPROVAL_DATE).HasColumnName("EXPECTED_APPROVAL_DATE");
            this.Property(t => t.RECORD_VERSION_NUMBER).HasColumnName("RECORD_VERSION_NUMBER");
            this.Property(t => t.INITIAL_TEAM_TEMPLATE_ID).HasColumnName("INITIAL_TEAM_TEMPLATE_ID");
            this.Property(t => t.JOB_BILL_RATE_SCHEDULE_ID).HasColumnName("JOB_BILL_RATE_SCHEDULE_ID");
            this.Property(t => t.EMP_BILL_RATE_SCHEDULE_ID).HasColumnName("EMP_BILL_RATE_SCHEDULE_ID");
            this.Property(t => t.COMPETENCE_MATCH_WT).HasColumnName("COMPETENCE_MATCH_WT");
            this.Property(t => t.AVAILABILITY_MATCH_WT).HasColumnName("AVAILABILITY_MATCH_WT");
            this.Property(t => t.JOB_LEVEL_MATCH_WT).HasColumnName("JOB_LEVEL_MATCH_WT");
            this.Property(t => t.ENABLE_AUTOMATED_SEARCH).HasColumnName("ENABLE_AUTOMATED_SEARCH");
            this.Property(t => t.SEARCH_MIN_AVAILABILITY).HasColumnName("SEARCH_MIN_AVAILABILITY");
            this.Property(t => t.SEARCH_ORG_HIER_ID).HasColumnName("SEARCH_ORG_HIER_ID");
            this.Property(t => t.SEARCH_STARTING_ORG_ID).HasColumnName("SEARCH_STARTING_ORG_ID");
            this.Property(t => t.SEARCH_COUNTRY_CODE).HasColumnName("SEARCH_COUNTRY_CODE");
            this.Property(t => t.MIN_CAND_SCORE_REQD_FOR_NOM).HasColumnName("MIN_CAND_SCORE_REQD_FOR_NOM");
            this.Property(t => t.NON_LAB_STD_BILL_RT_SCH_ID).HasColumnName("NON_LAB_STD_BILL_RT_SCH_ID");
            this.Property(t => t.INVPROC_CURRENCY_TYPE).HasColumnName("INVPROC_CURRENCY_TYPE");
            this.Property(t => t.REVPROC_CURRENCY_CODE).HasColumnName("REVPROC_CURRENCY_CODE");
            this.Property(t => t.PROJECT_BIL_RATE_DATE_CODE).HasColumnName("PROJECT_BIL_RATE_DATE_CODE");
            this.Property(t => t.PROJECT_BIL_RATE_TYPE).HasColumnName("PROJECT_BIL_RATE_TYPE");
            this.Property(t => t.PROJECT_BIL_RATE_DATE).HasColumnName("PROJECT_BIL_RATE_DATE");
            this.Property(t => t.PROJECT_BIL_EXCHANGE_RATE).HasColumnName("PROJECT_BIL_EXCHANGE_RATE");
            this.Property(t => t.PROJFUNC_CURRENCY_CODE).HasColumnName("PROJFUNC_CURRENCY_CODE");
            this.Property(t => t.PROJFUNC_BIL_RATE_DATE_CODE).HasColumnName("PROJFUNC_BIL_RATE_DATE_CODE");
            this.Property(t => t.PROJFUNC_BIL_RATE_TYPE).HasColumnName("PROJFUNC_BIL_RATE_TYPE");
            this.Property(t => t.PROJFUNC_BIL_RATE_DATE).HasColumnName("PROJFUNC_BIL_RATE_DATE");
            this.Property(t => t.PROJFUNC_BIL_EXCHANGE_RATE).HasColumnName("PROJFUNC_BIL_EXCHANGE_RATE");
            this.Property(t => t.FUNDING_RATE_DATE_CODE).HasColumnName("FUNDING_RATE_DATE_CODE");
            this.Property(t => t.FUNDING_RATE_TYPE).HasColumnName("FUNDING_RATE_TYPE");
            this.Property(t => t.FUNDING_RATE_DATE).HasColumnName("FUNDING_RATE_DATE");
            this.Property(t => t.FUNDING_EXCHANGE_RATE).HasColumnName("FUNDING_EXCHANGE_RATE");
            this.Property(t => t.BASELINE_FUNDING_FLAG).HasColumnName("BASELINE_FUNDING_FLAG");
            this.Property(t => t.PROJFUNC_COST_RATE_TYPE).HasColumnName("PROJFUNC_COST_RATE_TYPE");
            this.Property(t => t.PROJFUNC_COST_RATE_DATE).HasColumnName("PROJFUNC_COST_RATE_DATE");
            this.Property(t => t.INV_BY_BILL_TRANS_CURR_FLAG).HasColumnName("INV_BY_BILL_TRANS_CURR_FLAG");
            this.Property(t => t.MULTI_CURRENCY_BILLING_FLAG).HasColumnName("MULTI_CURRENCY_BILLING_FLAG");
            this.Property(t => t.SPLIT_COST_FROM_WORKPLAN_FLAG).HasColumnName("SPLIT_COST_FROM_WORKPLAN_FLAG");
            this.Property(t => t.SPLIT_COST_FROM_BILL_FLAG).HasColumnName("SPLIT_COST_FROM_BILL_FLAG");
            this.Property(t => t.ASSIGN_PRECEDES_TASK).HasColumnName("ASSIGN_PRECEDES_TASK");
            this.Property(t => t.PRIORITY_CODE).HasColumnName("PRIORITY_CODE");
            this.Property(t => t.RETN_BILLING_INV_FORMAT_ID).HasColumnName("RETN_BILLING_INV_FORMAT_ID");
            this.Property(t => t.RETN_ACCOUNTING_FLAG).HasColumnName("RETN_ACCOUNTING_FLAG");
            this.Property(t => t.ADV_ACTION_SET_ID).HasColumnName("ADV_ACTION_SET_ID");
            this.Property(t => t.START_ADV_ACTION_SET_FLAG).HasColumnName("START_ADV_ACTION_SET_FLAG");
            this.Property(t => t.REVALUATE_FUNDING_FLAG).HasColumnName("REVALUATE_FUNDING_FLAG");
            this.Property(t => t.INCLUDE_GAINS_LOSSES_FLAG).HasColumnName("INCLUDE_GAINS_LOSSES_FLAG");
            this.Property(t => t.TARGET_START_DATE).HasColumnName("TARGET_START_DATE");
            this.Property(t => t.TARGET_FINISH_DATE).HasColumnName("TARGET_FINISH_DATE");
            this.Property(t => t.BASELINE_START_DATE).HasColumnName("BASELINE_START_DATE");
            this.Property(t => t.BASELINE_FINISH_DATE).HasColumnName("BASELINE_FINISH_DATE");
            this.Property(t => t.SCHEDULED_AS_OF_DATE).HasColumnName("SCHEDULED_AS_OF_DATE");
            this.Property(t => t.BASELINE_AS_OF_DATE).HasColumnName("BASELINE_AS_OF_DATE");
            this.Property(t => t.LABOR_DISC_REASON_CODE).HasColumnName("LABOR_DISC_REASON_CODE");
            this.Property(t => t.NON_LABOR_DISC_REASON_CODE).HasColumnName("NON_LABOR_DISC_REASON_CODE");
            this.Property(t => t.SECURITY_LEVEL).HasColumnName("SECURITY_LEVEL");
            this.Property(t => t.ACTUAL_AS_OF_DATE).HasColumnName("ACTUAL_AS_OF_DATE");
            this.Property(t => t.SCHEDULED_DURATION).HasColumnName("SCHEDULED_DURATION");
            this.Property(t => t.BASELINE_DURATION).HasColumnName("BASELINE_DURATION");
            this.Property(t => t.ACTUAL_DURATION).HasColumnName("ACTUAL_DURATION");
            this.Property(t => t.LONG_NAME).HasColumnName("LONG_NAME");
            this.Property(t => t.BTC_COST_BASE_REV_CODE).HasColumnName("BTC_COST_BASE_REV_CODE");
            this.Property(t => t.ASSET_ALLOCATION_METHOD).HasColumnName("ASSET_ALLOCATION_METHOD");
            this.Property(t => t.CAPITAL_EVENT_PROCESSING).HasColumnName("CAPITAL_EVENT_PROCESSING");
            this.Property(t => t.CINT_RATE_SCH_ID).HasColumnName("CINT_RATE_SCH_ID");
            this.Property(t => t.CINT_ELIGIBLE_FLAG).HasColumnName("CINT_ELIGIBLE_FLAG");
            this.Property(t => t.CINT_STOP_DATE).HasColumnName("CINT_STOP_DATE");
            this.Property(t => t.SYS_PROGRAM_FLAG).HasColumnName("SYS_PROGRAM_FLAG");
            this.Property(t => t.STRUCTURE_SHARING_CODE).HasColumnName("STRUCTURE_SHARING_CODE");
            this.Property(t => t.ENABLE_TOP_TASK_CUSTOMER_FLAG).HasColumnName("ENABLE_TOP_TASK_CUSTOMER_FLAG");
            this.Property(t => t.ENABLE_TOP_TASK_INV_MTH_FLAG).HasColumnName("ENABLE_TOP_TASK_INV_MTH_FLAG");
            this.Property(t => t.REVENUE_ACCRUAL_METHOD).HasColumnName("REVENUE_ACCRUAL_METHOD");
            this.Property(t => t.INVOICE_METHOD).HasColumnName("INVOICE_METHOD");
            this.Property(t => t.PROJFUNC_ATTR_FOR_AR_FLAG).HasColumnName("PROJFUNC_ATTR_FOR_AR_FLAG");
            this.Property(t => t.PJI_SOURCE_FLAG).HasColumnName("PJI_SOURCE_FLAG");
            this.Property(t => t.ALLOW_MULTI_PROGRAM_ROLLUP).HasColumnName("ALLOW_MULTI_PROGRAM_ROLLUP");
            this.Property(t => t.PROJ_REQ_RES_FORMAT_ID).HasColumnName("PROJ_REQ_RES_FORMAT_ID");
            this.Property(t => t.PROJ_ASGMT_RES_FORMAT_ID).HasColumnName("PROJ_ASGMT_RES_FORMAT_ID");
            this.Property(t => t.FUNDING_APPROVAL_STATUS_CODE).HasColumnName("FUNDING_APPROVAL_STATUS_CODE");
        }
    }
}

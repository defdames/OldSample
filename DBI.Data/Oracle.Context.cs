﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<SYS_USERS> SYS_USERS { get; set; }
        public DbSet<SYS_USER_INFORMATION> SYS_USER_INFORMATION { get; set; }
        public DbSet<SYS_USER_ACTIVITY> SYS_USER_ACTIVITY { get; set; }
        public DbSet<SYS_LOG> SYS_LOG { get; set; }
        public DbSet<DAILY_ACTIVITY_WEATHER> DAILY_ACTIVITY_WEATHER { get; set; }
        public DbSet<DAILY_ACTIVITY_EQUIPMENT> DAILY_ACTIVITY_EQUIPMENT { get; set; }
        public DbSet<INVENTORY_V> INVENTORY_V { get; set; }
        public DbSet<SUBINVENTORY_V> SUBINVENTORY_V { get; set; }
        public DbSet<DAILY_ACTIVITY_CHEMICAL_MIX> DAILY_ACTIVITY_CHEMICAL_MIX { get; set; }
        public DbSet<PROJECTS_V> PROJECTS_V { get; set; }
        public DbSet<CLASS_CODES_V> CLASS_CODES_V { get; set; }
        public DbSet<UNIT_OF_MEASURE_V> UNIT_OF_MEASURE_V { get; set; }
        public DbSet<SYS_ACTIVITY> SYS_ACTIVITY { get; set; }
        public DbSet<CROSSING_DATA_ENTRY> CROSSING_DATA_ENTRY { get; set; }
        public DbSet<PA_TASKS_V> PA_TASKS_V { get; set; }
        public DbSet<PA_LOCATIONS_V> PA_LOCATIONS_V { get; set; }
        public DbSet<EXPENDITURE_TYPE_V> EXPENDITURE_TYPE_V { get; set; }
        public DbSet<CROSSING_CONTACTS> CROSSING_CONTACTS { get; set; }
        public DbSet<XXPJ_PREV_WAGE_RATES> XXPJ_PREV_WAGE_RATES { get; set; }
        public DbSet<DAILY_ACTIVITY_HEADER> DAILY_ACTIVITY_HEADER { get; set; }
        public DbSet<DAILY_ACTIVITY_AUDIT> DAILY_ACTIVITY_AUDIT { get; set; }
        public DbSet<PA_PERIODS_ALL> PA_PERIODS_ALL { get; set; }
        public DbSet<MTL_TRANSACTION_INT_V> MTL_TRANSACTION_INT_V { get; set; }
        public DbSet<XXDBI_DAILY_ACTIVITY_HEADER_V> XXDBI_DAILY_ACTIVITY_HEADER_V { get; set; }
        public DbSet<XXDBI_LABOR_HEADER_V> XXDBI_LABOR_HEADER_V { get; set; }
        public DbSet<XXDBI_PER_DIEM_V> XXDBI_PER_DIEM_V { get; set; }
        public DbSet<XXDBI_TRUCK_EQUIP_USAGE_V> XXDBI_TRUCK_EQUIP_USAGE_V { get; set; }
        public DbSet<PA_TRANSACTION_INT_V> PA_TRANSACTION_INT_V { get; set; }
        public DbSet<DAILY_ACTIVITY_IMPORT> DAILY_ACTIVITY_IMPORT { get; set; }
        public DbSet<CUSTOMER_SURVEY_FIELDSETS> CUSTOMER_SURVEY_FIELDSETS { get; set; }
        public DbSet<CUSTOMER_SURVEY_FORMS_ANS> CUSTOMER_SURVEY_FORMS_ANS { get; set; }
        public DbSet<CUSTOMER_SURVEY_FORMS_COMP> CUSTOMER_SURVEY_FORMS_COMP { get; set; }
        public DbSet<CUSTOMER_SURVEY_QUES_TYPES> CUSTOMER_SURVEY_QUES_TYPES { get; set; }
        public DbSet<CUSTOMER_SURVEY_RELATION> CUSTOMER_SURVEY_RELATION { get; set; }
        public DbSet<CROSSING_INSPECTION> CROSSING_INSPECTION { get; set; }
        public DbSet<CUSTOMER_SURVEY_OPTIONS> CUSTOMER_SURVEY_OPTIONS { get; set; }
        public DbSet<DAILY_ACTIVITY_FOOTER> DAILY_ACTIVITY_FOOTER { get; set; }
        public DbSet<DAILY_ACTIVITY_STATUS> DAILY_ACTIVITY_STATUS { get; set; }
        public DbSet<SYS_GROUPS_PERMS> SYS_GROUPS_PERMS { get; set; }
        public DbSet<SYS_MODULES> SYS_MODULES { get; set; }
        public DbSet<SYS_MOBILE_DEVICES> SYS_MOBILE_DEVICES { get; set; }
        public DbSet<SYS_MOBILE_NOTIFICATIONS> SYS_MOBILE_NOTIFICATIONS { get; set; }
        public DbSet<SYS_USER_PERMS> SYS_USER_PERMS { get; set; }
        public DbSet<SYS_MENU> SYS_MENU { get; set; }
        public DbSet<CROSSING_INCIDENT> CROSSING_INCIDENT { get; set; }
        public DbSet<EMPLOYEES_V> EMPLOYEES_V { get; set; }
        public DbSet<SYS_GROUPS> SYS_GROUPS { get; set; }
        public DbSet<JOB_TITLE_V> JOB_TITLE_V { get; set; }
        public DbSet<CROSSING_RELATIONSHIP> CROSSING_RELATIONSHIP { get; set; }
        public DbSet<SYS_PERMISSIONS> SYS_PERMISSIONS { get; set; }
        public DbSet<DAILY_ACTIVITY_EMPLOYEE> DAILY_ACTIVITY_EMPLOYEE { get; set; }
        public DbSet<XXDBI_PAYROLL_AUDIT_V> XXDBI_PAYROLL_AUDIT_V { get; set; }
        public DbSet<HIERARCHY_ID_V> HIERARCHY_ID_V { get; set; }
        public DbSet<SYS_USER_ORGS> SYS_USER_ORGS { get; set; }
        public DbSet<GL_ACCOUNTS_V> GL_ACCOUNTS_V { get; set; }
        public DbSet<ORG_HIER_V> ORG_HIER_V { get; set; }
        public DbSet<DAILY_ACTIVITY_PRODUCTION> DAILY_ACTIVITY_PRODUCTION { get; set; }
        public DbSet<CROSSING_PROJECT> CROSSING_PROJECT { get; set; }
        public DbSet<CROSSING_RAILROAD> CROSSING_RAILROAD { get; set; }
        public DbSet<DAILY_ACTIVITY_INVENTORY> DAILY_ACTIVITY_INVENTORY { get; set; }
        public DbSet<JOB_COST_CLASSES> JOB_COST_CLASSES { get; set; }
        public DbSet<OVERHEAD_BUDGET_VERSION> OVERHEAD_BUDGET_VERSION { get; set; }
        public DbSet<BUD_BID_LINES> BUD_BID_LINES { get; set; }
        public DbSet<BUD_BID_LOCKED> BUD_BID_LOCKED { get; set; }
        public DbSet<BUD_BID_ORGS> BUD_BID_ORGS { get; set; }
        public DbSet<BUD_BID_STATUS> BUD_BID_STATUS { get; set; }
        public DbSet<SYS_MODULE_PROFILE_OPTIONS> SYS_MODULE_PROFILE_OPTIONS { get; set; }
        public DbSet<SYS_PROFILE_OPTIONS> SYS_PROFILE_OPTIONS { get; set; }
        public DbSet<SYS_USER_PROFILE_OPTIONS> SYS_USER_PROFILE_OPTIONS { get; set; }
        public DbSet<SYS_SECURITY_AUDIT> SYS_SECURITY_AUDIT { get; set; }
        public DbSet<JOB_COST_BALANCE_TYPE> JOB_COST_BALANCE_TYPE { get; set; }
        public DbSet<JOB_COST_DATE> JOB_COST_DATE { get; set; }
        public DbSet<JOB_COST_PROJECTS> JOB_COST_PROJECTS { get; set; }
        public DbSet<JOB_COST_ROLLUPS> JOB_COST_ROLLUPS { get; set; }
        public DbSet<JOB_COST_TASK> JOB_COST_TASK { get; set; }
        public DbSet<CROSSING_SERVICE_UNIT> CROSSING_SERVICE_UNIT { get; set; }
        public DbSet<CROSSING_SUB_DIVISION> CROSSING_SUB_DIVISION { get; set; }
        public DbSet<BUD_BID_ACTUAL_NUM> BUD_BID_ACTUAL_NUM { get; set; }
        public DbSet<BUD_BID_BUDGET_NUM> BUD_BID_BUDGET_NUM { get; set; }
        public DbSet<BUD_BID_DETAIL_SHEET> BUD_BID_DETAIL_SHEET { get; set; }
        public DbSet<BUD_BID_DETAIL_TASK> BUD_BID_DETAIL_TASK { get; set; }
        public DbSet<SYS_ORG_PROFILE_OPTIONS> SYS_ORG_PROFILE_OPTIONS { get; set; }
        public DbSet<CROSSING> CROSSINGS { get; set; }
        public DbSet<OVERHEAD_BUDGET_TYPE> OVERHEAD_BUDGET_TYPE { get; set; }
        public DbSet<TIME_CLOCK> TIME_CLOCK { get; set; }
        public DbSet<CROSSING_INVOICE> CROSSING_INVOICE { get; set; }
        public DbSet<CROSSING_SUPP_INVOICE> CROSSING_SUPP_INVOICE { get; set; }
        public DbSet<CUSTOMER_SURVEY_QUESTIONS> CUSTOMER_SURVEY_QUESTIONS { get; set; }
        public DbSet<BUD_BID_PROJECTS> BUD_BID_PROJECTS { get; set; }
        public DbSet<CROSSING_APPLICATION> CROSSING_APPLICATION { get; set; }
        public DbSet<CROSSING_SUPPLEMENTAL> CROSSING_SUPPLEMENTAL { get; set; }
        public DbSet<CUSTOMER_SURVEY_CAT> CUSTOMER_SURVEY_CAT { get; set; }
        public DbSet<CUSTOMER_SURVEY_FORMS> CUSTOMER_SURVEY_FORMS { get; set; }
        public DbSet<CUSTOMER_SURVEY_THRESH_AMT> CUSTOMER_SURVEY_THRESH_AMT { get; set; }
        public DbSet<OVERHEAD_GL_ACCOUNT> OVERHEAD_GL_ACCOUNT { get; set; }
        public DbSet<CUSTOMER_SURVEY_THRESHOLDS> CUSTOMER_SURVEY_THRESHOLDS { get; set; }
        public DbSet<BUD_BID_ADJUSTMENT> BUD_BID_ADJUSTMENT { get; set; }
        public DbSet<PA_ROLES_V> PA_ROLES_V { get; set; }
        public DbSet<JOB_COST> JOB_COST { get; set; }
        public DbSet<OVERHEAD_GL_RANGE> OVERHEAD_GL_RANGE { get; set; }
    }
}

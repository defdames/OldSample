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
        public DbSet<EMPLOYEES_V> EMPLOYEES_V { get; set; }
        public DbSet<DAILY_ACTIVITY_WEATHER> DAILY_ACTIVITY_WEATHER { get; set; }
        public DbSet<DAILY_ACTIVITY_EQUIPMENT> DAILY_ACTIVITY_EQUIPMENT { get; set; }
        public DbSet<INVENTORY_V> INVENTORY_V { get; set; }
        public DbSet<DAILY_ACTIVITY_HEADER> DAILY_ACTIVITY_HEADER { get; set; }
        public DbSet<SUBINVENTORY_V> SUBINVENTORY_V { get; set; }
        public DbSet<DAILY_ACTIVITY_CHEMICAL_MIX> DAILY_ACTIVITY_CHEMICAL_MIX { get; set; }
        public DbSet<PROJECTS_V> PROJECTS_V { get; set; }
        public DbSet<CLASS_CODES_V> CLASS_CODES_V { get; set; }
        public DbSet<UNIT_OF_MEASURE_V> UNIT_OF_MEASURE_V { get; set; }
        public DbSet<DAILY_ACTIVITY_STATUS> DAILY_ACTIVITY_STATUS { get; set; }
        public DbSet<SYS_ACTIVITY> SYS_ACTIVITY { get; set; }
        public DbSet<CROSSING_DATA_ENTRY> CROSSING_DATA_ENTRY { get; set; }
        public DbSet<CROSSING_SUB_DIVISION> CROSSING_SUB_DIVISION { get; set; }
        public DbSet<DAILY_ACTIVITY_PRODUCTION> DAILY_ACTIVITY_PRODUCTION { get; set; }
        public DbSet<DAILY_ACTIVITY_FOOTER> DAILY_ACTIVITY_FOOTER { get; set; }
        public DbSet<DAILY_ACTIVITY_INVENTORY> DAILY_ACTIVITY_INVENTORY { get; set; }
        public DbSet<CROSSING_CONTACTS> CROSSING_CONTACTS { get; set; }
        public DbSet<CROSSING> CROSSINGS { get; set; }
        public DbSet<PA_TASKS_V> PA_TASKS_V { get; set; }
        public DbSet<PA_ROLES_V> PA_ROLES_V { get; set; }
        public DbSet<DAILY_ACTIVITY_EMPLOYEE> DAILY_ACTIVITY_EMPLOYEE { get; set; }
        public DbSet<XXDBI_DAILY_ACTIVITY_HEADER> XXDBI_DAILY_ACTIVITY_HEADER { get; set; }
        public DbSet<XXDBI_LABOR_HEADER> XXDBI_LABOR_HEADER { get; set; }
        public DbSet<XXDBI_TRUCK_EQUIP_USAGE> XXDBI_TRUCK_EQUIP_USAGE { get; set; }
        public DbSet<PA_LOCATIONS_V> PA_LOCATIONS_V { get; set; }
        public DbSet<EXPENDITURE_TYPE_V> EXPENDITURE_TYPE_V { get; set; }
    }
}

using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using DBI.Data.Models.Mapping;

namespace DBI.Data.Models
{
    public partial class Entities : DbContext
    {
        static Entities()
        {
            Database.SetInitializer<Entities>(null);
        }

        public Entities()
            : base("Name=Entities")
        {
        }

        public DbSet<SYS_ACTIVITY> SYS_ACTIVITY { get; set; }
        public DbSet<SYS_LOG> SYS_LOG { get; set; }
        public DbSet<SYS_ROLES> SYS_ROLES { get; set; }
        public DbSet<SYS_USER_ACTIVITY> SYS_USER_ACTIVITY { get; set; }
        public DbSet<SYS_USER_PERMISSIONS> SYS_USER_PERMISSIONS { get; set; }
        public DbSet<SYS_USER_ROLES> SYS_USER_ROLES { get; set; }
        public DbSet<SYS_USERS> SYS_USERS { get; set; }
        public DbSet<PROJECTS_V> PROJECTS_V { get; set; }
        public DbSet<SYS_USER_INFORMATION> SYS_USER_INFORMATION { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new SYS_ACTIVITYMap());
            modelBuilder.Configurations.Add(new SYS_LOGMap());
            modelBuilder.Configurations.Add(new SYS_ROLESMap());
            modelBuilder.Configurations.Add(new SYS_USER_ACTIVITYMap());
            modelBuilder.Configurations.Add(new SYS_USER_PERMISSIONSMap());
            modelBuilder.Configurations.Add(new SYS_USER_ROLESMap());
            modelBuilder.Configurations.Add(new SYS_USERSMap());
            modelBuilder.Configurations.Add(new PROJECTS_VMap());
            modelBuilder.Configurations.Add(new SYS_USER_INFORMATIONMap());
        }
    }
}

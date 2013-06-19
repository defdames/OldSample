using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using DBI.Data.Models.Mapping;

namespace DBI.Data.Models
{

    /// <summary>
    /// This class creates the connection to Oracle for Entity Framework connections using the OracleDatabase connection string in the app.config. 
    /// </summary>
    public partial class OracleDatabase : DbContext
    {
        /// <summary>
        /// The connection to Oracle
        /// </summary>
        static OracleDatabase()
        {
            Database.SetInitializer<OracleDatabase>(null);
        }

        /// <summary>
        /// New connection
        /// </summary>
        public OracleDatabase()
            : base("Name=OracleDatabase")
        {
        }

        /// <summary>
        /// Property for the SecurityRole Table Entity Object
        /// </summary>
        public DbSet<SECURITYROLE> SECURITYROLEs { get; set; }

        /// <summary>
        /// Property for the User Table Entity Object
        /// </summary>
        public DbSet<SYS_USERS> SYS_USERS { get; set; }

        /// <summary>
        /// Adds the mapping to the Oracle tables for all Entity objects.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new SecurityRoleMap());
            modelBuilder.Configurations.Add(new SYS_USERS_MAP());
        }
    }
}

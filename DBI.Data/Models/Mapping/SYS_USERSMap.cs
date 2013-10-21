using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DBI.Data.Models.Mapping
{
    public class SYS_USERSMap : EntityTypeConfiguration<SYS_USERS>
    {
        public SYS_USERSMap()
        {
            // Primary Key
            this.HasKey(t => t.USER_ID);

            // Properties
            this.Property(t => t.USER_ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("SYS_USERS", "XXEMS");
            this.Property(t => t.USER_ID).HasColumnName("USER_ID");
            this.Property(t => t.LAST_ACTIVITY_DATE).HasColumnName("LAST_ACTIVITY_DATE");
        }
    }
}

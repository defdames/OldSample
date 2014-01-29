using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DBI.Data.Models.Mapping
{
    public class SYS_USER_ROLESMap : EntityTypeConfiguration<SYS_USER_ROLES>
    {
        public SYS_USER_ROLESMap()
        {
            // Primary Key
            this.HasKey(t => t.USER_ROLE_ID);

            // Properties
            this.Property(t => t.USER_ROLE_ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("SYS_USER_ROLES", "XXEMS");
            this.Property(t => t.USER_ROLE_ID).HasColumnName("USER_ROLE_ID");
            this.Property(t => t.USER_ID).HasColumnName("USER_ID");
            this.Property(t => t.ROLE_ID).HasColumnName("ROLE_ID");
        }
    }
}

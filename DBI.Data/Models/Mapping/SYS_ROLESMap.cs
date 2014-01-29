using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DBI.Data.Models.Mapping
{
    public class SYS_ROLESMap : EntityTypeConfiguration<SYS_ROLES>
    {
        public SYS_ROLESMap()
        {
            // Primary Key
            this.HasKey(t => t.ROLE_ID);

            // Properties
            this.Property(t => t.ROLE_ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.NAME)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.DESCRIPTION)
                .IsRequired()
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("SYS_ROLES", "XXEMS");
            this.Property(t => t.ROLE_ID).HasColumnName("ROLE_ID");
            this.Property(t => t.NAME).HasColumnName("NAME");
            this.Property(t => t.DESCRIPTION).HasColumnName("DESCRIPTION");
        }
    }
}

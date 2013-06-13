using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.CompilerServices;

namespace DBI.Data.Models.Mapping
{
    [CompilerGenerated]
    public class SecurityRoleMap : EntityTypeConfiguration<SECURITYROLE>
    {
        public SecurityRoleMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Properties
            this.Property(t => t.NAME)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.DESCRIPTION)
                .IsRequired()
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("SECURITYROLE", "XXEMS");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.NAME).HasColumnName("NAME");
            this.Property(t => t.DESCRIPTION).HasColumnName("DESCRIPTION");
        }
    }
}

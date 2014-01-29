using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DBI.Data.Models.Mapping
{
    public class SYS_ACTIVITYMap : EntityTypeConfiguration<SYS_ACTIVITY>
    {
        public SYS_ACTIVITYMap()
        {
            // Primary Key
            this.HasKey(t => t.ACTIVITY_ID);

            // Properties
            this.Property(t => t.ACTIVITY_ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.NAME)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.DESCRIPTION)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.CREATED_BY)
                .HasMaxLength(500);

            this.Property(t => t.LAST_UPDATED_BY)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("SYS_ACTIVITY", "XXEMS");
            this.Property(t => t.ACTIVITY_ID).HasColumnName("ACTIVITY_ID");
            this.Property(t => t.NAME).HasColumnName("NAME");
            this.Property(t => t.DESCRIPTION).HasColumnName("DESCRIPTION");
            this.Property(t => t.CREATED_DATE).HasColumnName("CREATED_DATE");
            this.Property(t => t.CREATED_BY).HasColumnName("CREATED_BY");
            this.Property(t => t.LAST_UPDATED).HasColumnName("LAST_UPDATED");
            this.Property(t => t.LAST_UPDATED_BY).HasColumnName("LAST_UPDATED_BY");
        }
    }
}

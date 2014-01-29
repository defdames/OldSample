using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DBI.Data.Models.Mapping
{
    public class SYS_USER_ACTIVITYMap : EntityTypeConfiguration<SYS_USER_ACTIVITY>
    {
        public SYS_USER_ACTIVITYMap()
        {
            // Primary Key
            this.HasKey(t => t.USER_ACTIVITY_ID);

            // Properties
            this.Property(t => t.USER_ACTIVITY_ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CREATED_BY)
                .HasMaxLength(500);

            this.Property(t => t.LAST_UPDATED_BY)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("SYS_USER_ACTIVITY", "XXEMS");
            this.Property(t => t.USER_ACTIVITY_ID).HasColumnName("USER_ACTIVITY_ID");
            this.Property(t => t.ACTIVITY_ID).HasColumnName("ACTIVITY_ID");
            this.Property(t => t.USER_ID).HasColumnName("USER_ID");
            this.Property(t => t.CREATED_DATE).HasColumnName("CREATED_DATE");
            this.Property(t => t.CREATED_BY).HasColumnName("CREATED_BY");
            this.Property(t => t.LAST_UPDATED).HasColumnName("LAST_UPDATED");
            this.Property(t => t.LAST_UPDATED_BY).HasColumnName("LAST_UPDATED_BY");
        }
    }
}

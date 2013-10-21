using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DBI.Data.Models.Mapping
{
    public class SYS_LOGMap : EntityTypeConfiguration<SYS_LOG>
    {
        public SYS_LOGMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.USER_CULTURE)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.GUID)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.MESSAGE)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.INNER_EXCEPTION)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.SOURCE)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.STACKTRACE)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.DEBUG)
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("SYS_LOG", "XXEMS");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.USER_ID).HasColumnName("USER_ID");
            this.Property(t => t.USER_CULTURE).HasColumnName("USER_CULTURE");
            this.Property(t => t.GUID).HasColumnName("GUID");
            this.Property(t => t.MESSAGE).HasColumnName("MESSAGE");
            this.Property(t => t.INNER_EXCEPTION).HasColumnName("INNER_EXCEPTION");
            this.Property(t => t.SOURCE).HasColumnName("SOURCE");
            this.Property(t => t.STACKTRACE).HasColumnName("STACKTRACE");
            this.Property(t => t.DEBUG).HasColumnName("DEBUG");
            this.Property(t => t.CREATED_DATE).HasColumnName("CREATED_DATE");
        }
    }
}

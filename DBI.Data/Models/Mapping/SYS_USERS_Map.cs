using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.CompilerServices;

namespace DBI.Data.Models.Mapping
{
    [CompilerGenerated]
    public class SYS_USERS_MAP : EntityTypeConfiguration<SYS_USERS>
    {
        public SYS_USERS_MAP()
        {
            // Primary Key
            this.HasKey(t => t.USER_ID);

            // Properties
            this.Property(t => t.USER_ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Properties
            this.Property(t => t.FND_USER_ID)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("SYS_USERS", "XXEMS");
            this.Property(t => t.USER_ID).HasColumnName("USER_ID");
            this.Property(t => t.FND_USER_ID).HasColumnName("FND_USER_ID");
        }
    }
}

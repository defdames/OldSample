using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DBI.Data.Models.Mapping
{
    public class SYS_USER_INFORMATIONMap : EntityTypeConfiguration<SYS_USER_INFORMATION>
    {
        public SYS_USER_INFORMATIONMap()
        {
            // Primary Key
            this.HasKey(t => new { t.USER_ID, t.USER_NAME, t.CURRENT_ORGANIZATION, t.CURRENT_ORG_ID, t.LOCATION_NAME });

            // Properties
            this.Property(t => t.USER_ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.USER_NAME)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.EMPLOYEE_NAME)
                .HasMaxLength(362);

            this.Property(t => t.EMPLOYEE_NUMBER)
                .HasMaxLength(30);

            this.Property(t => t.EMAIL_ADDRESS)
                .HasMaxLength(240);

            this.Property(t => t.JOB_NAME)
                .HasMaxLength(700);

            this.Property(t => t.CURRENT_ORGANIZATION)
                .IsRequired()
                .HasMaxLength(240);

            this.Property(t => t.CURRENT_ORG_ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ORACLE_ACCOUNT_STATUS)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.SUPERVISOR_NAME)
                .HasMaxLength(362);

            this.Property(t => t.SUPERVISOR_EMAIL_ADDRESS)
                .HasMaxLength(240);

            this.Property(t => t.SUPERVISOR_USER_NAME)
                .HasMaxLength(100);

            this.Property(t => t.LOCATION_NAME)
                .IsRequired()
                .HasMaxLength(60);

            // Table & Column Mappings
            this.ToTable("SYS_USER_INFORMATION", "XXEMS");
            this.Property(t => t.USER_ID).HasColumnName("USER_ID");
            this.Property(t => t.USER_NAME).HasColumnName("USER_NAME");
            this.Property(t => t.EMPLOYEE_NAME).HasColumnName("EMPLOYEE_NAME");
            this.Property(t => t.EMPLOYEE_NUMBER).HasColumnName("EMPLOYEE_NUMBER");
            this.Property(t => t.EMAIL_ADDRESS).HasColumnName("EMAIL_ADDRESS");
            this.Property(t => t.JOB_NAME).HasColumnName("JOB_NAME");
            this.Property(t => t.CURRENT_ORGANIZATION).HasColumnName("CURRENT_ORGANIZATION");
            this.Property(t => t.CURRENT_ORG_ID).HasColumnName("CURRENT_ORG_ID");
            this.Property(t => t.ORACLE_ACCOUNT_STATUS).HasColumnName("ORACLE_ACCOUNT_STATUS");
            this.Property(t => t.SUPERVISOR_NAME).HasColumnName("SUPERVISOR_NAME");
            this.Property(t => t.SUPERVISOR_ID).HasColumnName("SUPERVISOR_ID");
            this.Property(t => t.SUPERVISOR_EMAIL_ADDRESS).HasColumnName("SUPERVISOR_EMAIL_ADDRESS");
            this.Property(t => t.SUPERVISOR_USER_NAME).HasColumnName("SUPERVISOR_USER_NAME");
            this.Property(t => t.LOCATION_NAME).HasColumnName("LOCATION_NAME");
        }
    }
}

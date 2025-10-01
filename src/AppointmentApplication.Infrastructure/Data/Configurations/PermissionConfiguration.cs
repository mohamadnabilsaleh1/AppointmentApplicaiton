using AppointmentApplication.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("permissions");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}

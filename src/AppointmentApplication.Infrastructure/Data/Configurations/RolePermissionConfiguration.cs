using AppointmentApplication.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.ToTable("role_permissions");

            builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });

            builder.HasOne<Role>()
                .WithMany()
                .HasForeignKey(rp => rp.RoleId);

            builder.HasOne<Permission>()
                .WithMany()
                .HasForeignKey(rp => rp.PermissionId);
        }
    }
}

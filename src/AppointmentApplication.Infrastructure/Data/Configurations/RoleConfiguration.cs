using AppointmentApplication.Domain.Users;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("roles");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.HasMany(role => role.Users)
                .WithMany(user => user.Roles);

            builder.HasData(
                    Role.Patient,
                    Role.Admin,
                    Role.Doctor,
                    Role.HealthCareFacility);

            // A Role can have many Permissions (via RolePermission)
            // builder.HasMany(r => r.Permissions)
            //        .WithMany()
            //        .UsingEntity<RolePermission>(
            //             j => j.HasOne<Permission>()
            //                   .WithMany()
            //                   .HasForeignKey(rp => rp.PermissionId),
            //             j => j.HasOne<Role>()
            //                   .WithMany()
            //                   .HasForeignKey(rp => rp.RoleId),
            //             j =>
            //             {
            //                 j.ToTable("role_permissions");
            //                 j.HasKey(rp => new { rp.RoleId, rp.PermissionId });
            //             });
        }
    }
}

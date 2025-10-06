using AppointmentApplication.Domain.Users;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.HasKey(user => user.Id);

            builder.Property(user => user.FirstName)
                .HasMaxLength(200);

            builder.Property(user => user.LastName)
                .HasMaxLength(200);

            builder.Property(user => user.Email)
                .HasMaxLength(400);

            builder.HasIndex(user => user.Email).IsUnique();
            builder.HasIndex(user => user.IdentityId).IsUnique();
            builder.HasMany(u => u.HealthCareFacilities)
                        .WithOne()
                        .HasForeignKey(h => h.UserId)
                        .OnDelete(DeleteBehavior.Restrict);
            // 🔑 Many-to-Many User <-> Role
            // builder.HasMany(u => u.Roles)
            //        .WithMany(r => r.Users)
            //        .UsingEntity<Dictionary<string, object>>(
            //             "user_roles", // join table name
            //             j => j.HasOne<Role>()
            //                   .WithMany()
            //                   .HasForeignKey("RoleId")
            //                   .OnDelete(DeleteBehavior.Restrict),
            //             j => j.HasOne<User>()
            //                   .WithMany()
            //                   .HasForeignKey("UserId")
            //                   .OnDelete(DeleteBehavior.Restrict),
            //             j =>
            //             {
            //                 j.HasKey("UserId", "RoleId");
            //             });
        }
    }
}

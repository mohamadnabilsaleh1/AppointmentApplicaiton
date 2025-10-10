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
                .WithOne(h => h.User) // This tells EF about the navigation property
                .HasForeignKey(h => h.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.Patients)
                .WithOne(p => p.User) // This tells EF about the navigation property
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

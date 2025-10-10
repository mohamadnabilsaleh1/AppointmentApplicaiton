using System;
using Microsoft.EntityFrameworkCore;
using AppointmentApplication.Domain.Citizens;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Enums;

namespace AppointmentApplication.Infrastructure.Data
{
    public class CountryUsersDbContext : DbContext, ICountryUsersDbContext
    {
        public CountryUsersDbContext(DbContextOptions<CountryUsersDbContext> options)
            : base(options)
        {
        }

        public DbSet<Citizen> Citizens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Citizen>(entity =>
            {
                entity.ToTable("Citizens");
                entity.Property(u => u.NationalId).IsRequired();
                entity.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.MiddleName).HasMaxLength(100);
                entity.Property(u => u.LastName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.PhoneNumber).IsRequired().HasMaxLength(20);
                entity.Property(u => u.BirthDate).IsRequired();
                entity.Property(u => u.Gender).IsRequired();

                // Seed 50 citizens with static GUIDs
                entity.HasData(
    new Citizen(Guid.Parse("11111111-1111-1111-1111-111111111111"), 1000000001, "Ahmed", "Mohammed", "Ali", "0984306816", new DateOnly(1990, 5, 12), Gender.Male),
    new Citizen(Guid.Parse("22222222-2222-2222-2222-222222222222"), 1000000002, "Fatima", "Hassan", "Yousef", "0984306817", new DateOnly(1988, 3, 22), Gender.Female),
    new Citizen(Guid.Parse("33333333-3333-3333-3333-333333333333"), 1000000003, "Omar", "Khaled", "Mahmoud", "0984306818", new DateOnly(1995, 7, 15), Gender.Male),
    new Citizen(Guid.Parse("44444444-4444-4444-4444-444444444444"), 1000000004, "Layla", "Sami", "Abdel", "0984306819", new DateOnly(1992, 11, 9), Gender.Female),
    new Citizen(Guid.Parse("55555555-5555-5555-5555-555555555555"), 1000000005, "Youssef", "Adel", "Hussein", "0984306820", new DateOnly(1985, 1, 30), Gender.Male),
    new Citizen(Guid.Parse("66666666-6666-6666-6666-666666666666"), 1000000006, "Mariam", "Tarek", "Saeed", "0984306821", new DateOnly(1991, 6, 18), Gender.Female),
    new Citizen(Guid.Parse("77777777-7777-7777-7777-777777777777"), 1000000007, "Ali", "Mostafa", "Nabil", "0984306822", new DateOnly(1989, 12, 5), Gender.Male),
    new Citizen(Guid.Parse("88888888-8888-8888-8888-888888888888"), 1000000008, "Sara", "Omar", "Fahmy", "0984306823", new DateOnly(1994, 8, 2), Gender.Female),
    new Citizen(Guid.Parse("99999999-9999-9999-9999-999999999999"), 1000000009, "Hassan", "Ibrahim", "Kamal", "0984306824", new DateOnly(1993, 2, 25), Gender.Male),
    new Citizen(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), 1000000010, "Noor", "Yahya", "Salah", "0984306825", new DateOnly(1990, 4, 10), Gender.Female)
);
            });
        }
    }
}

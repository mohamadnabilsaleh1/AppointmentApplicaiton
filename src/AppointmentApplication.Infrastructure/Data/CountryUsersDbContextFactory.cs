using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AppointmentApplication.Infrastructure.Data
{
    public class CountryUsersDbContextFactory : IDesignTimeDbContextFactory<CountryUsersDbContext>
    {
        public CountryUsersDbContext CreateDbContext(string[] args)
        {
            // Load appsettings.json from the API project (adjust path if needed)
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../AppointmentApplication.API"))
                .AddJsonFile("appsettings.json")
                .Build();

            // Use connection string named "CountryConnection"
            string? connectionString = configuration.GetConnectionString("CountryConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'CountryConnection' not found.");
            }

            var optionsBuilder = new DbContextOptionsBuilder<CountryUsersDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new CountryUsersDbContext(optionsBuilder.Options);
        }
    }
}

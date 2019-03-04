using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HeatConsumptionOrganization.Models
{
    class Context : DbContext
    {
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<TypeOfProduct> TypeOfProducts { get; set; }
        public DbSet<ManufacturedProduct> ManufacturedProducts { get; set; }
        public DbSet<HeatConsumption> HeatConsumptions { get; set; }
        public DbSet<HeatConsumptionRate> HeatConsumptionRates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            string connectionString = config.GetConnectionString("SQLConnection");
            var options = optionsBuilder
                .UseSqlServer(connectionString)
                .Options;
        }
    }
}

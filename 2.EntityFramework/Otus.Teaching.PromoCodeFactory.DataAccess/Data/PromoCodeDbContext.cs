using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Data
{
    public class PromoCodeDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Preference> Preferences { get; set; }

        public DbSet<PromoCode> PromoCodes { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public PromoCodeDbContext(DbContextOptions<PromoCodeDbContext> options) : base(options)
        {
            // Database.EnsureDeleted();
            var newDatabase = Database.EnsureCreated();

            if (newDatabase)
            {
                Roles.AddRange(FakeDataFactory.Roles);
                Preferences.AddRange(FakeDataFactory.Preferences);
                SaveChanges();

                var employees = FakeDataFactory.Employees;
                employees[0].Role = Roles.FirstOrDefault(x => x.Name == "Admin");
                employees[1].Role = Roles.FirstOrDefault(x => x.Name == "PartnerManager");
                Employees.AddRange(employees);
                SaveChanges();

                var partnerManager = Employees.FirstOrDefault(e => e.Role.Name == "PartnerManager");
                var theatePreference = Preferences.FirstOrDefault(x => x.Name == "Театр");
                var familyPreference = Preferences.FirstOrDefault(x => x.Name == "Семья");

                var promoCodes = FakeDataFactory.PromoCodes;
                promoCodes[0].PartnerManager = partnerManager;
                promoCodes[0].Preference = theatePreference;
                promoCodes[1].PartnerManager = partnerManager;
                promoCodes[1].Preference = familyPreference;
                PromoCodes.AddRange(promoCodes);
                SaveChanges();

                var customers = FakeDataFactory.Customers;
                customers[0].Preferences = new List<Preference> { theatePreference, familyPreference };
                customers[0].PromoCodes = promoCodes;
                customers[1].Preferences = new List<Preference> { theatePreference, familyPreference };
                customers[1].PromoCodes = promoCodes;

                Customers.AddRange(customers);
                SaveChanges();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(builder =>
                {
                    builder.Property(employee => employee.FirstName)
                        .HasMaxLength(250);

                    builder.Property(employee => employee.LastName)
                        .HasMaxLength(250);

                    builder.Property(employee => employee.FullName)
                        .HasMaxLength(250);

                    builder.Property(employee => employee.Email)
                        .HasMaxLength(250);
                }
            );

            modelBuilder.Entity<Customer>(builder =>
                {
                    builder.Property(employee => employee.FirstName)
                        .HasMaxLength(250);

                    builder.Property(employee => employee.LastName)
                        .HasMaxLength(250);

                    builder.Property(employee => employee.FullName)
                        .HasMaxLength(250);

                    builder.Property(employee => employee.Email)
                        .HasMaxLength(250);
                }
            );

            modelBuilder.Entity<Preference>(builder =>
                {
                    builder.Property(employee => employee.Name)
                        .HasMaxLength(250);
                }
            );

            modelBuilder.Entity<PromoCode>(builder =>
                {
                    builder.Property(employee => employee.Code)
                        .HasMaxLength(250);

                    builder.Property(employee => employee.PartnerName)
                        .HasMaxLength(250);

                    builder.Property(employee => employee.ServiceInfo)
                        .HasMaxLength(250);
                }
            );

            modelBuilder.Entity<Role>(builder =>
                {
                    builder.Property(employee => employee.Name)
                        .HasMaxLength(100);

                    builder.Property(employee => employee.Description)
                        .HasMaxLength(100);
                }
            );
        }
    }
}
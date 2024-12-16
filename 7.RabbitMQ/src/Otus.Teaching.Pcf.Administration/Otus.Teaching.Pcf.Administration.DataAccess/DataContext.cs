﻿using Microsoft.EntityFrameworkCore;
using Otus.Teaching.Pcf.Administration.Core.Domain.Administration;
using Otus.Teaching.Pcf.Administration.DataAccess.Data;

namespace Otus.Teaching.Pcf.Administration.DataAccess
{
    public class DataContext
        : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        
        public DbSet<Employee> Employees { get; set; }

        public DataContext()
        {
            
        }
        
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
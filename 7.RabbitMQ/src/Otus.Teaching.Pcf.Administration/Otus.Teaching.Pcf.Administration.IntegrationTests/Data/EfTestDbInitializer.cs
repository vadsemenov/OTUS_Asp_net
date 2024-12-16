﻿using Otus.Teaching.Pcf.Administration.DataAccess;
using Otus.Teaching.Pcf.Administration.DataAccess.Data;

namespace Otus.Teaching.Pcf.Administration.IntegrationTests.Data
{
    public class EfTestDbInitializer
        : IDbInitializer
    {
        private readonly DataContext _dataContext;

        public EfTestDbInitializer(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        
        public void InitializeDb()
        {
            _dataContext.Database.EnsureDeleted();
            _dataContext.Database.EnsureCreated();
            
            _dataContext.AddRange(TestDataFactory.Employees);
            _dataContext.SaveChanges();
        }

        public void CleanDb()
        {
            _dataContext.Database.EnsureDeleted();
        }
    }
}
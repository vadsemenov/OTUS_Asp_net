using MongoDB.Driver;
using Pcf.Administration.Core.Domain.Administration;

namespace Pcf.Administration.DataAccess.Data;

public class MongoDbInitializer : IDbInitializer
{
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<Role> _roleCollection;
    private readonly IMongoCollection<Employee> _employeeCollection;

    public MongoDbInitializer(IMongoDatabase database)
    {
        _database = database;
        _roleCollection = database.GetCollection<Role>(nameof(Role) + "sCollection");
        _employeeCollection = database.GetCollection<Employee>(nameof(Employee) + "sCollection");
    }

    public void InitializeDb()
    {
        _roleCollection.InsertMany(FakeDataFactory.Roles);

        var roles = _roleCollection
            .Find(_ => true)
            .ToList();

        _employeeCollection.InsertMany(FakeDataFactory.Employees);
    }
}
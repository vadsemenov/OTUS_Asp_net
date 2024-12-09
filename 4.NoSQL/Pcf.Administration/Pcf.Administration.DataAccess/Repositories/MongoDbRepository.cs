using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using Pcf.Administration.Core.Abstractions.Repositories;
using Pcf.Administration.Core.Domain;

namespace Pcf.Administration.DataAccess.Repositories;

public class MongoDbRepository<T> :
    IMongoRepository<T>
    where T : MongoBaseEntity
{
    private readonly IMongoCollection<T> _collection;

    public MongoDbRepository(IMongoDatabase database)
    {
        // using var client = new MongoClient(connectionString);
        // var database = client.GetDatabase(databaseName);
        _collection = database.GetCollection<T>(typeof(T).Name + "sCollection");
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public async Task<T> GetByIdAsync(string id)
    {
        return await _collection.Find(e => e.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> GetRangeByIdsAsync(List<string> ids)
    {
        return await _collection.Find(e => ids.Contains(e.Id)).ToListAsync();
    }

    public async Task<T> GetFirstWhere(Expression<Func<T, bool>> predicate)
    {
        return await _collection.Find(predicate).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate)
    {
        return await _collection.Find(predicate).ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _collection.InsertOneAsync(entity);
    }

    public async Task UpdateAsync(T entity)
    {
        var filter = Builders<T>.Filter.Eq(e => e.Id, entity.Id);
        await _collection.ReplaceOneAsync(filter, entity);
    }

    public async Task DeleteAsync(T entity)
    {
        var filter = Builders<T>.Filter.Eq(e => e.Id, entity.Id);
        await _collection.DeleteOneAsync(filter);
    }
}
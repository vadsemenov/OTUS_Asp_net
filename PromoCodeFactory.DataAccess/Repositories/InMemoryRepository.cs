using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
namespace PromoCodeFactory.DataAccess.Repositories
{
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected IEnumerable<T> Data { get; set; }

        public InMemoryRepository(IEnumerable<T> data)
        {
            Data = data;
        }

        public Task<bool> CreateAsync(T entity)
        {
            Data = Data.Append(entity);

            return Task.FromResult(true);
        }

        public Task<bool> UpdateAsync(T entity)
        {
            var databaseEntity = Data.FirstOrDefault(e => e.Id == entity.Id);

            if (databaseEntity == null)
            {
                return Task.FromResult(false);
            }

            Data = Data.Where(e => e.Id != entity.Id).Append(entity);

            return Task.FromResult(true);
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            var databaseEntity = Data.FirstOrDefault(e => e.Id == id);

            if (databaseEntity == null)
            {
                return Task.FromResult(false);
            }

            Data = Data.Where(e => e.Id != id);

            return Task.FromResult(true);
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(Data);
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
        }
    }
}
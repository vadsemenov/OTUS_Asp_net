using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories;

public class RolesRepository : IRepository<Role>
{
    private readonly PromoCodeDbContext _promoCodeDbContext;

    public RolesRepository(PromoCodeDbContext promoCodeDbContext)
    {
        _promoCodeDbContext = promoCodeDbContext;
    }

    public async Task<IEnumerable<Role>> GetAllAsync()
    {
        return await _promoCodeDbContext.Roles.ToListAsync();
    }

    public async Task<Role> GetByIdAsync(Guid id)
    {
        return await _promoCodeDbContext.Roles.FirstOrDefaultAsync(c => c.Id == id);
    }

    public Task AddAsync(Role entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Role entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Role entity)
    {
        throw new NotImplementedException();
    }
}
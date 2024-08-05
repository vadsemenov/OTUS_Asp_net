using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories;

public class PromoCodesRepository : IRepository<PromoCode>
{
    private readonly PromoCodeDbContext _promoCodeDbContext;

    public PromoCodesRepository(PromoCodeDbContext promoCodeDbContext)
    {
        _promoCodeDbContext = promoCodeDbContext;
    }

    public async Task<IEnumerable<PromoCode>> GetAllAsync()
    {
        return await _promoCodeDbContext.PromoCodes.ToListAsync();
    }

    public async Task<PromoCode> GetByIdAsync(Guid id)
    {
        return await _promoCodeDbContext.PromoCodes.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddAsync(PromoCode entity)
    {
        await _promoCodeDbContext.PromoCodes.AddAsync(entity);

        await _promoCodeDbContext.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(PromoCode promoCode)
    {
        if (promoCode == null)
        {
            return await Task.FromResult(false);
        }

        _promoCodeDbContext.PromoCodes.Remove(promoCode);

        await _promoCodeDbContext.SaveChangesAsync();

        return await Task.FromResult(true);
    }

    public Task UpdateAsync(PromoCode entity)
    {
        throw new NotImplementedException();
    }
}
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Castle.Core.Resource;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories;

public class PreferencesRepository : IRepository<Preference>
{
    private readonly PromoCodeDbContext _promoCodeDbContext;

    public PreferencesRepository(PromoCodeDbContext promoCodeDbContext)
    {
        _promoCodeDbContext = promoCodeDbContext;
    }

    public async Task<IEnumerable<Preference>> GetAllAsync()
    {
        return await _promoCodeDbContext.Preferences.ToListAsync();
    }

    public async Task<Preference> GetByIdAsync(Guid id)
    {
        return await _promoCodeDbContext.Preferences.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddAsync(Preference preference)
    {
        await _promoCodeDbContext.Preferences.AddAsync(preference);

        await _promoCodeDbContext.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(Preference preference)
    {
        if (preference == null)
        {
            return await Task.FromResult(false);
        }

        _promoCodeDbContext.Preferences.Remove(preference);

        await _promoCodeDbContext.SaveChangesAsync();

        return await Task.FromResult(true);
    }

    public Task UpdateAsync(Preference entity)
    {
        throw new NotImplementedException();
    }
}
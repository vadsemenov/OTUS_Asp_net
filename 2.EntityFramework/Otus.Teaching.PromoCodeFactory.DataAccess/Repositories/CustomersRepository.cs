using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess.Data;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories;

public class CustomersRepository : IRepository<Customer>
{
    private readonly PromoCodeDbContext _promoCodeDbContext;

    public CustomersRepository(PromoCodeDbContext promoCodeDbContext)
    {
        _promoCodeDbContext = promoCodeDbContext;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _promoCodeDbContext.Customers
            .Include(c=>c.Preferences)
            .Include(c => c.PromoCodes)
            .ToListAsync();
    }

    public async Task<Customer> GetByIdAsync(Guid id)
    {
        var customers = _promoCodeDbContext.Customers.ToList();


        return await _promoCodeDbContext.Customers
            .Include(c => c.PromoCodes)
            .Include(c => c.Preferences)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddAsync(Customer entity)
    {
        await _promoCodeDbContext.Customers.AddAsync(entity);

        await _promoCodeDbContext.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(Customer customer)
    {
        if (customer == null)
        {
            return await Task.FromResult(false);
        }

        _promoCodeDbContext.Customers.Remove(customer);

        await _promoCodeDbContext.SaveChangesAsync();

        return await Task.FromResult(true);
    }

    public async Task UpdateAsync(Customer entity)
    {
        _promoCodeDbContext.Customers.Update(entity);

        await _promoCodeDbContext.SaveChangesAsync();
    }
}
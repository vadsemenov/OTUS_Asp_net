using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.DataAccess.Data;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories;

public class EmployeesRepository : IRepository<Employee>
{
    private readonly PromoCodeDbContext _promoCodeDbContext;

    public EmployeesRepository(PromoCodeDbContext promoCodeDbContext)
    {
        _promoCodeDbContext = promoCodeDbContext;
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await _promoCodeDbContext.Employees.ToListAsync();
    }

    public async Task<Employee> GetByIdAsync(Guid id)
    {
        return await _promoCodeDbContext.Employees
            .Include(e=>e.Role)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public Task AddAsync(Employee entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Employee entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Employee entity)
    {
        throw new NotImplementedException();
    }
}
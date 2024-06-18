using System.Linq;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Map;

public static class EmployeeMapper
{
    public static Employee ToModel(this EmployeeRequest employeeRequest)
    {
        return new Employee
        {
            Id = employeeRequest.Id,
            FirstName = employeeRequest.FirstName,
            LastName = employeeRequest.LastName,
            Email = employeeRequest.Email,
            Roles = employeeRequest.Roles.Select(r => r.ToModel()).ToList(),
            AppliedPromocodesCount = employeeRequest.AppliedPromocodesCount
        };
    }
    public static EmployeeResponse ToDto(this Employee employee)
    {
        return new EmployeeResponse
        {
            Id = employee.Id,
            FullName = employee.FullName,
            Email = employee.Email,
            Roles = employee.Roles.Select(r => r.ToDtoResponse()).ToList(),
            AppliedPromocodesCount = employee.AppliedPromocodesCount
        };
    }
}
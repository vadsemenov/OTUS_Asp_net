using System.Linq;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Mapping;

public static class MapService
{
    public static Customer MapToModel(CreateOrEditCustomerRequest customerRequest)
    {
        return new Customer
        {
            FirstName = customerRequest.FirstName,
            LastName = customerRequest.LastName,
            Email = customerRequest.Email
        };
    }
    public static PromoCode MapToModel(this GivePromoCodeRequest request)
    {
        return new PromoCode
        {
            Code = request.PromoCode,
            ServiceInfo = request.ServiceInfo,
            PartnerName = request.PartnerName
        };
    }

    public static CustomerShortResponse MapToShortDto(Customer customer)
    {
        return new CustomerShortResponse
        {
            Id = customer.Id,
            Email = customer.Email,
            FirstName = customer.FirstName,
            LastName = customer.LastName
        };
    }

    public static CustomerResponse MapToDto(this Customer customer)
    {
        return new CustomerResponse
        {
            Id = customer.Id,
            Email = customer.Email,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            PromoCodes = customer.PromoCodes.Select(MapToShortDto).ToList()
        };
    }

    public static EmployeeResponse MapToDto(this Employee employee)
    {
        return new EmployeeResponse()
        {
            Id = employee.Id,
            Email = employee.Email,
            Role = new RoleItemResponse()
            {
                Id = employee.Role.Id,
                Name = employee.Role.Name,
                Description = employee.Role.Description
            },
            FullName = employee.FullName,
            AppliedPromocodesCount = employee.AppliedPromocodesCount
        };
    }

    public static PreferenceResponse MapToDto(this Preference preference)
    {
        return new PreferenceResponse
        {
            Id = preference.Id,
            Name = preference.Name,
            Customers = preference.Customers.Select(MapToShortDto).ToList()
        };
    }

    public static PromoCodeShortResponse MapToShortDto(this PromoCode promoCode)
    {
        return new PromoCodeShortResponse
        {
            Id = promoCode.Id,
            BeginDate = promoCode.BeginDate.ToShortDateString(),
            EndDate = promoCode.EndDate.ToShortDateString(),
            PartnerName = promoCode.PartnerName,
            Code = promoCode.Code,
            ServiceInfo = promoCode.ServiceInfo
        };
    }
}
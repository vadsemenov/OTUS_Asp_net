using System;
using System.Collections.Generic;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace PromoCodeFactory.UnitTests.WebHost.Controllers.Partners;

public static class PartnerFactory
{
    public static Partner CreatePartner(
        bool isActive = false,
        int numberIssuedPromoCodes = 0,
        DateTime? cancelDate = null)
    {
        return new Partner()
        {
            Id = Guid.Parse("0da65561-cf56-4942-bff2-22f50cf70d43"),
            Name = "Рыба твоей мечты",
            IsActive = isActive,
            NumberIssuedPromoCodes = numberIssuedPromoCodes,
            PartnerLimits = new List<PartnerPromoCodeLimit>()
            {
                new PartnerPromoCodeLimit()
                {
                    Id = Guid.Parse("0691bb24-5fd9-4a52-a11c-34bb8bc9364e"),
                    CreateDate = new DateTime(2020,05,3),
                    EndDate = new DateTime(2020,10,15),
                    CancelDate = cancelDate,
                    Limit = 1000
                }
            }
        };
    }
}
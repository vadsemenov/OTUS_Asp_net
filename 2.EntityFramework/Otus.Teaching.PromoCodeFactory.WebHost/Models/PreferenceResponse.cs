using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Collections.Generic;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Models;

public class PreferenceResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public ICollection<CustomerShortResponse> Customers { get; set; } 
}
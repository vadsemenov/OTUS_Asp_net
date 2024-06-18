using System.Collections.Generic;
using System;

namespace PromoCodeFactory.WebHost.Models;

public class EmployeeRequest
{
    public Guid Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public List<RoleItemRequest> Roles { get; set; }

    public int AppliedPromocodesCount { get; set; }
}
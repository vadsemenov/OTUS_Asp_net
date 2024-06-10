using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Map;

public static class RoleItemMapper
{
    public static Role ToModel(this RoleItemRequest roleItemRequest)
    {
        return new Role
        {
            Id = roleItemRequest.Id,
            Name = roleItemRequest.Name,
            Description = roleItemRequest.Description,
        };
    }
  
    public static RoleItemResponse ToDtoResponse(this Role role)
    {
        return new RoleItemResponse
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description
        };
    }
    public static RoleItemRequest ToDtoRequest(this Role role)
    {
        return new RoleItemRequest
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description
        };
    }
}
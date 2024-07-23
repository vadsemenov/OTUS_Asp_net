using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Mapping;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers;

/// <summary>
/// Предпочтения
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class PreferencesController : ControllerBase
{
    private readonly IRepository<Preference> _preferenceRepository;

    public PreferencesController(IRepository<Preference> preferenceRepository)
    {
        _preferenceRepository = preferenceRepository;
    }

    /// <summary>
    /// Получить все предпочтения
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<List<PreferenceResponse>>> GetPreferencesAsync()
    {
        var preferences = await _preferenceRepository.GetAllAsync();

        var preferenceDto = preferences.Select(MapService.MapToDto).ToList();

        return Ok(preferenceDto);
    }
}


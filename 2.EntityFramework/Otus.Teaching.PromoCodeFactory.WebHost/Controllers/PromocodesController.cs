using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Mapping;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Промокоды
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PromocodesController
        : ControllerBase
    {
        private readonly IRepository<PromoCode> _promoCodeRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Preference> _preferencesRepository;

        public PromocodesController(IRepository<PromoCode> promoCodeRepository,
            IRepository<Customer> customerRepository,
            IRepository<Preference> preferencesRepository)
        {
            _promoCodeRepository = promoCodeRepository;
            _customerRepository = customerRepository;
            _preferencesRepository = preferencesRepository;
        }

        /// <summary>
        /// Получить все промокоды
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<PromoCodeShortResponse>>> GetPromocodesAsync()
        {
            //TODO: Получить все промокоды 
            var preferences = await _promoCodeRepository.GetAllAsync();

            var preferencesDto = preferences.Select(MapService.MapToShortDto).ToList();

            return Ok(preferencesDto);
        }

        /// <summary>
        /// Создать промокод и выдать его клиентам с указанным предпочтением
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
        {
            //TODO: Создать промокод и выдать его клиентам с указанным предпочтением
            var customers = await _customerRepository.GetAllAsync();

            var promoCode = request.MapToModel();

            var preference = (await _preferencesRepository.GetAllAsync())
                .FirstOrDefault(p => p.Name == request.Preference);

            if (preference == null)
            {
                return NotFound();
            }

            promoCode.Preference = preference;
            promoCode.BeginDate = DateTime.Now;
            promoCode.EndDate = DateTime.Now + TimeSpan.FromDays(2);

            var customer = customers.FirstOrDefault(c => c.Preferences.Any(p => p.Id == preference.Id));

            if (customer == null)
            {
                return NotFound();
            }

            await _promoCodeRepository.AddAsync(promoCode);

            customer.PromoCodes.Add(promoCode);
            await _customerRepository.UpdateAsync(customer);

            return Ok();
        }
    }
}
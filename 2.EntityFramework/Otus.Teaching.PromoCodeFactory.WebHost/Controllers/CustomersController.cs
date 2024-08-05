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
    /// Клиенты
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController
        : ControllerBase
    {
        private readonly IRepository<Customer> _customersRepository;
        private readonly IRepository<Preference> _preferencesRepository;
        private readonly IRepository<PromoCode> _promoCodesRepository;


        public CustomersController(IRepository<Customer> customersRepository,
            IRepository<Preference> preferencesRepository,
            IRepository<PromoCode> promoCodesRepository)
        {
            _customersRepository = customersRepository;
            _preferencesRepository = preferencesRepository;
            _promoCodesRepository = promoCodesRepository;
        }

        //Реализовать CRUD операции для CustomersController через репозиторий EF,
        //нужно добавить новый Generic класс EfRepository.
        //Получение списка, получение одного клиента, создание/редактирование и удаление,
        //при удалении также нужно удалить ранее выданные промокоды клиента.
        //Методы должны иметь xml комментарии для описания в Swagger.
        //CustomerResponse также должен возвращать список предпочтений клиента с той же моделью PrefernceResponse

        /// <summary>
        /// Получение списка всех клиентов
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet]
        public async Task<ActionResult<List<CustomerShortResponse>>> GetCustomersAsync()
        {
            var customers = await _customersRepository.GetAllAsync();

            return Ok(customers.Select(MapService.MapToShortDto).ToList());
        }

        /// <summary>
        /// Получение клиента по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            //TODO: Добавить получение клиента вместе с выданными ему промомкодами
            var customer = await _customersRepository.GetByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            var customerDto = customer.MapToDto();

            return Ok(customerDto);
        }

        /// <summary>
        /// Добавление нового клиента
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            //TODO: Добавить создание нового клиента вместе с его предпочтениями
            var preferences = new List<Preference>();

            foreach (var preferenceId in request.PreferenceIds)
            {
                preferences.Add(await _preferencesRepository.GetByIdAsync(preferenceId));
            }

            var customer = MapService.MapToModel(request);
            customer.Preferences = preferences;

            await _customersRepository.AddAsync(customer);

            return Ok();
        }

        /// <summary>
        /// Обновление клиента
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            //TODO: Обновить данные клиента вместе с его предпочтениями
            var customer = await _customersRepository.GetByIdAsync(id);

            if (customer == null)
            {
                NotFound();
            }

            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.Email = request.Email;
            customer.Preferences = new List<Preference>();

            await _customersRepository.UpdateAsync(customer);

            var preferences = new List<Preference>();

            foreach (var preferenceId in request.PreferenceIds)
            {
                preferences.Add(await _preferencesRepository.GetByIdAsync(preferenceId));
            }

            customer.Preferences = preferences
                .Where(p => p != null)
                .ToList();

            await _customersRepository.UpdateAsync(customer);

            return Ok();
        }

        /// <summary>
        /// Удаление клиента
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            //TODO: Удаление клиента вместе с выданными ему промокодами
            var customer = await _customersRepository.GetByIdAsync(id);

            if (customer == null)
            {
                NotFound();
            }

            var promoCode = customer?.PromoCodes;

            if (promoCode != null)
            {
                foreach (var preference in promoCode)
                {
                    await _promoCodesRepository.DeleteAsync(preference);
                }
            }

            await _customersRepository.DeleteAsync(customer);

            return Ok();
        }
    }
}
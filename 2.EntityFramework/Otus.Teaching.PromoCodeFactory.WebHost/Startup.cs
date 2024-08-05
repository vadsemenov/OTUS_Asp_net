using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess.Data;
using Otus.Teaching.PromoCodeFactory.DataAccess.Repositories;

namespace Otus.Teaching.PromoCodeFactory.WebHost
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<PromoCodeDbContext>(op =>
                op.UseLazyLoadingProxies()
                    .UseSqlite(_configuration.GetConnectionString("SqlLite")));

            services.AddScoped<IRepository<Customer>, CustomersRepository>();
            services.AddScoped<IRepository<Role>, RolesRepository>();
            services.AddScoped<IRepository<PromoCode>, PromoCodesRepository>();
            services.AddScoped<IRepository<Preference>, PreferencesRepository>();
            services.AddScoped<IRepository<Employee>, EmployeesRepository>();

            // services.AddScoped(typeof(IRepository<Employee>), (x) => 
            //     new InMemoryRepository<Employee>(FakeDataFactory.Employees));
            // services.AddScoped(typeof(IRepository<Role>), (x) => 
            //     new InMemoryRepository<Role>(FakeDataFactory.Roles));
            // services.AddScoped(typeof(IRepository<Preference>), (x) => 
            //     new InMemoryRepository<Preference>(FakeDataFactory.Preferences));
            // services.AddScoped(typeof(IRepository<Customer>), (x) => 
            //     new InMemoryRepository<Customer>(FakeDataFactory.Customers));

            services.AddOpenApiDocument(options =>
            {
                options.Title = "PromoCode Factory API Doc";
                options.Version = "1.0";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseOpenApi();
            app.UseSwaggerUi3(x =>
            {
                x.DocExpansion = "list";
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
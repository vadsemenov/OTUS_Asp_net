using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Pcf.Administration.Core.Abstractions.Repositories;
using Pcf.Administration.DataAccess.Data;
using Pcf.Administration.DataAccess.Repositories;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Pcf.Administration.WebHost
{
    // Run Mongo:
    // Выполнить в папке где неходиться docker compose:
    // docker-compose up --build --force-recreate --renew-anon-volumes
    // version: '3.8'

    // Чтобы просмотреть базу данных в докере кликнуть на порт напротив Monge express:
    // Mongo express password:
    // User: admin
    // Password: pass

    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddMvcOptions(x=> 
                x.SuppressAsyncSuffixInActionNames = false);
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            // services.AddScoped<IDbInitializer, EfDbInitializer>();
            services.AddScoped<IDbInitializer, MongoDbInitializer>();
            // services.AddDbContext<DataContext>(x =>
            // {
            //     //x.UseSqlite("Filename=PromocodeFactoryAdministrationDb.sqlite");
            //     x.UseNpgsql(Configuration.GetConnectionString("PromocodeFactoryAdministrationDb"));
            //     x.UseSnakeCaseNamingConvention();
            //     x.UseLazyLoadingProxies();
            // });

            // Привязка настроек из appsettings.json
            services.Configure<MongoDbSettings>(Configuration.GetSection("MongoDB"));

            // Регистрация MongoDB-клиента
            services.AddSingleton<IMongoClient>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                return new MongoClient(settings.ConnectionString);
            });

            // Регистрация MongoDB-базы данных
            services.AddScoped<IMongoDatabase>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase(settings.DatabaseName);
            });

            services.AddScoped(typeof(IMongoRepository<>), typeof(MongoDbRepository<>));
            
            services.AddOpenApiDocument(options =>
            {
                options.Title = "PromoCode Factory Administration API Doc";
                options.Version = "1.0";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbInitializer dbInitializer)
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
            app.UseSwaggerUi(x =>
            {
                x.DocExpansion = "list";
            });
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            dbInitializer.InitializeDb();
        }
    }
}
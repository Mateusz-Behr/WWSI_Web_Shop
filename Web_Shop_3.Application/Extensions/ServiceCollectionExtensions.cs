using HashidsNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sieve.Models;
using Sieve.Services;
using Web_Shop_3.Application.CustomQueries;
using Web_Shop_3.Application.Mappings.PropertiesMappings;
using Web_Shop_3.Application.Services;
using Web_Shop_3.Application.Services.Interfaces;

namespace Web_Shop_3.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SieveOptions>(sieveOptions =>
            {
                configuration.GetSection("Sieve").Bind(sieveOptions);
            });

            services.AddSingleton<IHashids>(_ => new Hashids("Nkazmu4RgFA8Ye6nP5vQjcBJ3xCTKb79", 12));
            services
                .AddScoped<ISieveCustomSortMethods, SieveCustomSortMethods>()
                .AddScoped<ISieveCustomFilterMethods, SieveCustomFilterMethods>()
                .AddScoped<ISieveProcessor, ApplicationSieveProcessor>();

            services
                .AddScoped(typeof(ICustomerService), typeof(CustomerService))
                .AddScoped(typeof(ICategoryService), typeof(CategoryService))
                .AddScoped(typeof(IProductService), typeof(ProductService));
        }
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Web_Shop_3.Persistence.MySQL.Extensions;
using Web_Shop_3.Persistence.UOW.Interfaces;
using Web_Shop_3.Persistence.UOW;
using Web_Shop_3.Persistence.Repositories;
using Web_Shop_3.Persistence.Repositories.Interfaces;


namespace Web_Shop_3.Persistence.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMySQLDbContext(configuration);

            //services.AddScoped(typeof(ICustomerRepository), typeof(CustomerRepository));
            //services.AddScoped(typeof(ICategoryRepository), typeof(CategoryRepository));

            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
        }
    }
}

using Web_Shop_3.Persistence.MySQL.Model;

namespace Web_Shop_3.Persistence.Repositories.Interfaces
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<bool> EmailExistsAsync(string email);
        Task<bool> IsEmailEditAllowedAsync(string email, ulong id);
        Task<Customer?> GetByEmailAsync(string email);
    }
}

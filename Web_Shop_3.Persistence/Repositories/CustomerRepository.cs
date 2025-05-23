using Microsoft.EntityFrameworkCore;
using Web_Shop_3.Persistence.Repositories.Interfaces;
using Web_Shop_3.Persistence.MySQL.Context;
using Web_Shop_3.Persistence.MySQL.Model;

namespace Web_Shop_3.Persistence.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(WwsishopContext context) : base(context) { }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await Entities.AnyAsync(e => e.Email == email);
        }

        public async Task<bool> IsEmailEditAllowedAsync(string email, ulong id)
        {
            return !await Entities.AnyAsync(e => e.Email == email && e.IdCustomer != id);
        }

        public async Task<Customer?> GetByEmailAsync(string email)
        {
            return await Entities.FirstOrDefaultAsync(e => e.Email == email);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Web_Shop_3.Persistence.Repositories.Interfaces;
using Web_Shop_3.Persistence.MySQL.Context;
using Web_Shop_3.Persistence.MySQL.Model;

namespace Web_Shop_3.Persistence.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(WwsishopContext context) : base(context) { }

    }
}

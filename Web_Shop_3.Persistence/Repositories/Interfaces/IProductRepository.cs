using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Shop_3.Persistence.MySQL.Model;

namespace Web_Shop_3.Persistence.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<bool> ExistsByNameOrSkuAsync(string name, string sku, ulong? excludeId = null);

    }
}

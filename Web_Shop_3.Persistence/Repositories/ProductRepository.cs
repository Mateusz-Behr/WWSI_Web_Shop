using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Shop_3.Persistence.MySQL.Context;
using Web_Shop_3.Persistence.MySQL.Model;
using Web_Shop_3.Persistence.Repositories.Interfaces;

namespace Web_Shop_3.Persistence.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(WwsishopContext context) : base(context) { }

        public async Task<bool> ExistsByNameOrSkuAsync(string name, string sku, ulong? excludeId = null)
        {
            return await Entities.AnyAsync(p => (p.Name == name || p.Sku == sku) && (excludeId == null || p.IdProduct != excludeId));
        }
    }
}

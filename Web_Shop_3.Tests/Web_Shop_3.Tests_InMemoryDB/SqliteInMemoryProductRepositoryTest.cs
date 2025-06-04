using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Shop_3.Persistence.MySQL.Model;
using Web_Shop_3.Persistence.Repositories;

namespace Web_Shop_3.Tests_InMemoryDB
{
    public class SqliteInMemoryProductRepositoryTest : IDisposable
    {
        private readonly SqliteDatabaseFixture _databaseFixture;
        public SqliteInMemoryProductRepositoryTest()
        {
            _databaseFixture = new SqliteDatabaseFixture();
        }
        public void Dispose()
        {
            _databaseFixture.Dispose();
        }

        [Fact]
        public async Task ExistsByNameOrSkuAsync_ProductExistsWithNameOrSku_ReturnsTrue()
        {
            using var context = _databaseFixture.CreateContext();

            await context.Products.AddAsync(new Product
            {
                Name = "ProduktTest",
                Description = "DescriptionTest",
                Price = 1.00M,
                Sku = "SKUTest"
            });
            await context.SaveChangesAsync();

            var repo = new ProductRepository(context);

            var exists = await repo.ExistsByNameOrSkuAsync("ProduktTest", "SKUTest");
            Assert.True(exists);
        }

        [Fact]
        public async Task ExistsByNameOrSkuAsync_ProductNotExistsWithNameOrSku_ReturnsFalse()
        {
            using var context = _databaseFixture.CreateContext();
            var repo = new ProductRepository(context);

            var exists = await repo.ExistsByNameOrSkuAsync("Nieistniejąca nazwa", "Nieistniejący SKU");
            Assert.False(exists);
        }
    }
}

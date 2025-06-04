using Microsoft.Extensions.Logging;
using Moq;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Web_Shop_3.Application.CustomQueries;
using Web_Shop_3.Application.DTOs.ProductDTOs;
using Web_Shop_3.Application.Mappings.PropertiesMappings;
using Web_Shop_3.Application.Services;
using Web_Shop_3.Persistence.MySQL.Model;
using Web_Shop_3.Persistence.UOW;
using Web_Shop_3.Tests.Common.Sieve;

namespace Web_Shop_3.Tests_InMemoryDB
{
    public class SqliteInMemoryProductServiceTest : IDisposable
    {
        private readonly SqliteDatabaseFixture _databaseFixture;

        private readonly Mock<ILogger<Product>> _loggerMock;

        private readonly SieveProcessor _processor;
        private readonly SieveOptionsAccessor _optionsAccessor;

        public SqliteInMemoryProductServiceTest()
        {
            _databaseFixture = new SqliteDatabaseFixture();

            _loggerMock = new Mock<ILogger<Product>>();

            _optionsAccessor = new SieveOptionsAccessor();

            _processor = new ApplicationSieveProcessor(_optionsAccessor,
                new SieveCustomSortMethods(),
                new SieveCustomFilterMethods());
        }
        public void Dispose()
        {
            _databaseFixture.Dispose();
        }

        [Fact]
        public async Task ProductService_CreateNewProductAsync_ReturnsTrue()
        {
            using var context = _databaseFixture.CreateContext();

            var unitOfWork = new UnitOfWork(context);

            var productService = new ProductService(_loggerMock.Object, _processor, _optionsAccessor, unitOfWork);

            var newProductDTO = new AddUpdateProductDTO
            {
                Name = "TestProduct",
                Description = "TestDescription",
                Price = 100,
                Sku = "TESTSKU001",
            };

            var result = await productService.CreateNewProductAsync(newProductDTO);

            Assert.True(result.IsSuccess);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.entity);
            Assert.Equal("TESTSKU001", result.entity.Sku);
        }
    }
}

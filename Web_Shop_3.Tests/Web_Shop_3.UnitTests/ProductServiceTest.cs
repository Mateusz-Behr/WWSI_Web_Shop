using Microsoft.Extensions.Logging;
using Moq;
using Web_Shop_3.Application.CustomQueries;
using Web_Shop_3.Application.DTOs.ProductDTOs;
using Web_Shop_3.Application.Mappings.PropertiesMappings;
using Web_Shop_3.Application.Services;
using Web_Shop_3.Persistence.MySQL.Model;
using Web_Shop_3.Persistence.Repositories.Interfaces;
using Web_Shop_3.Persistence.UOW.Interfaces;
using Web_Shop_3.Tests.Common.Sieve;
using BC = BCrypt.Net.BCrypt;

namespace Web_Shop_3.UnitTests
{
    public class ProductServiceTest
    {
        private readonly Mock<ILogger<Product>> _loggerMock;

        private readonly Mock<ApplicationSieveProcessor> _processorMock;
        private readonly Mock<SieveOptionsAccessor> _optionsAccessorMock;

        public ProductServiceTest()
        {
            _loggerMock = new Mock<ILogger<Product>>();

            _optionsAccessorMock = new Mock<SieveOptionsAccessor>();

            _processorMock = new Mock<ApplicationSieveProcessor>(_optionsAccessorMock.Object,
                new Mock<SieveCustomSortMethods>().Object,
                new Mock<SieveCustomFilterMethods>().Object);
        }

        [Theory]
        [InlineData(false)]
        public async Task ProductService_CreateNewProductAsync_ReturnsTrue(bool nameSkuExists)
        {
            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(m => m.ExistsByNameOrSkuAsync(It.IsAny<string>(), It.IsAny<string>(), null)).ReturnsAsync(() => nameSkuExists);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.ProductRepository).Returns(() => productRepositoryMock.Object);
            unitOfWorkMock.Setup(m => m.Repository<Product>()).Returns(() => productRepositoryMock.Object);
            unitOfWorkMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(0));

            var productService = new ProductService(_loggerMock.Object, _processorMock.Object, _optionsAccessorMock.Object, unitOfWorkMock.Object);

            var addUpdateProductDTO = new AddUpdateProductDTO()
            {
                Name = "TestName",
                Description = "TestDescription",
                Price = 5,
                Sku = "AAA_BBB_ABC01"
            };

            var verifyResult = await productService.CreateNewProductAsync(addUpdateProductDTO);

            Assert.True(verifyResult.IsSuccess);
            Assert.Equal(System.Net.HttpStatusCode.OK, verifyResult.StatusCode);
        }

        [Theory]
        [InlineData(true)]
        public async Task ProductService_CreateNewProductAsync_ReturnsFalse(bool nameSkuExists)
        {
            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(m => m.ExistsByNameOrSkuAsync(It.IsAny<string>(), It.IsAny<string>(), null)).ReturnsAsync(() => nameSkuExists);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.ProductRepository).Returns(() => productRepositoryMock.Object);
            unitOfWorkMock.Setup(m => m.Repository<Product>()).Returns(() => productRepositoryMock.Object);
            unitOfWorkMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(0));

            var productService = new ProductService(_loggerMock.Object, _processorMock.Object, _optionsAccessorMock.Object, unitOfWorkMock.Object);

            var addUpdateProductDTO = new AddUpdateProductDTO()
            {
                Name = "TestName",
                Description = "TestDescription",
                Price = 5,
                Sku = "AAA_BBB_ABC01"
            };

            var verifyResult = await productService.CreateNewProductAsync(addUpdateProductDTO);

            Assert.False(verifyResult.IsSuccess);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, verifyResult.StatusCode);
        }


    }
}

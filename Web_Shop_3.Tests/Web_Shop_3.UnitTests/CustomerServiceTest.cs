using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Shop_3.Application.CustomQueries;
using Web_Shop_3.Application.DTOs.CustomerDTOs;
using Web_Shop_3.Application.Mappings.PropertiesMappings;
using Web_Shop_3.Application.Services;
using Web_Shop_3.Persistence.MySQL.Model;
using Web_Shop_3.Persistence.Repositories.Interfaces;
using Web_Shop_3.Persistence.UOW.Interfaces;
using Web_Shop_3.Tests.Common.Sieve;
using BC = BCrypt.Net.BCrypt;

namespace Web_Shop_3.UnitTests
{
    public class CustomerServiceTest
    {
        private readonly Mock<ILogger<Customer>> _loggerMock;

        private readonly Mock<ApplicationSieveProcessor> _processorMock;
        private readonly Mock<SieveOptionsAccessor> _optionsAccessorMock;

        public CustomerServiceTest()
        {
            _loggerMock = new Mock<ILogger<Customer>>();

            _optionsAccessorMock = new Mock<SieveOptionsAccessor>();

            _processorMock = new Mock<ApplicationSieveProcessor>(_optionsAccessorMock.Object,
                new Mock<SieveCustomSortMethods>().Object,
                new Mock<SieveCustomFilterMethods>().Object);
        }

        [Theory]
        [InlineData(false)]
        public async Task CustomerService_CreateNewCustomerAsync_ReturnsTrue(bool emailExists)
        {
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            customerRepositoryMock.Setup(m => m.EmailExistsAsync(It.IsAny<string>())).Returns((string email) => Task.FromResult(emailExists));

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.CustomerRepository).Returns(() => customerRepositoryMock.Object);
            unitOfWorkMock.Setup(m => m.Repository<Customer>()).Returns(() => customerRepositoryMock.Object);
            unitOfWorkMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(0));

            var customerService = new CustomerService(_loggerMock.Object, _processorMock.Object, _optionsAccessorMock.Object, unitOfWorkMock.Object);

            var addUpdateCustomerDTO = new AddUpdateCustomerDTO()
            {
                Name = "TestName",
                Surname = "TestSurname",
                Password = "TestPassword",
                Email = "test@domain.com"
            };

            var verifyResult = await customerService.CreateNewCustomerAsync(addUpdateCustomerDTO);

            Assert.True(verifyResult.IsSuccess);
            Assert.Equal(System.Net.HttpStatusCode.OK, verifyResult.StatusCode);
        }

        [Theory]
        [InlineData(true)]
        public async Task CustomerService_CreateNewCustomerAsync_ReturnsFalse(bool emailExists)
        {
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            customerRepositoryMock.Setup(m => m.EmailExistsAsync(It.IsAny<string>())).Returns((string email) => Task.FromResult(emailExists));

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.CustomerRepository).Returns(() => customerRepositoryMock.Object);
            unitOfWorkMock.Setup(m => m.Repository<Customer>()).Returns(() => customerRepositoryMock.Object);
            unitOfWorkMock.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(0));

            var customerService = new CustomerService(_loggerMock.Object, _processorMock.Object, _optionsAccessorMock.Object, unitOfWorkMock.Object);

            var addUpdateCustomerDTO = new AddUpdateCustomerDTO()
            {
                Name = "Test",
                Surname = "Test",
                Password = "Test",
                Email = "test@domain.com"
            };

            var verifyResult = await customerService.CreateNewCustomerAsync(addUpdateCustomerDTO);

            Assert.False(verifyResult.IsSuccess);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, verifyResult.StatusCode);
        }

        [Fact]
        public async Task CustomerService_VerifyPasswordByEmailTest_ReturnsTrue()
        {
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            customerRepositoryMock.Setup(m => m.GetByEmailAsync(It.IsAny<string>())).Returns((string email) => Task.FromResult(new Customer { IdCustomer = 2, Name = "Jan", Surname = "Kowalski", Email = "jan.kowalski@o2.pl", PasswordHash = BC.HashPassword("Test222") }));

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.CustomerRepository).Returns(() => customerRepositoryMock.Object);

            var customerService = new CustomerService(_loggerMock.Object, _processorMock.Object, _optionsAccessorMock.Object, unitOfWorkMock.Object);

            var verifyResult = await customerService.VerifyPasswordByEmailAsync("jan.kowalski@o2.pl", "Test222");

            Assert.True(verifyResult.IsSuccess);
            Assert.Equal(System.Net.HttpStatusCode.OK, verifyResult.StatusCode);
            Assert.Equal("jan.kowalski@o2.pl", verifyResult.entity!.Email);
        }

        [Fact]
        public async Task CustomerService_VerifyPasswordByEmailTest_ReturnsFalse()
        {
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            customerRepositoryMock.Setup(m => m.GetByEmailAsync(It.IsAny<string>())).Returns((string email) => Task.FromResult(new Customer { IdCustomer = 2, Name = "Jan", Surname = "Kowalski", Email = "jan.kowalski@o2.pl", PasswordHash = BC.HashPassword("Test222") }));

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(m => m.CustomerRepository).Returns(() => customerRepositoryMock.Object);

            var customerService = new CustomerService(_loggerMock.Object, _processorMock.Object, _optionsAccessorMock.Object, unitOfWorkMock.Object);

            var verifyResult = await customerService.VerifyPasswordByEmailAsync("jan.kowalski@o2.pl", "Test211");

            Assert.False(verifyResult.IsSuccess);
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, verifyResult.StatusCode);
        }
    }
}

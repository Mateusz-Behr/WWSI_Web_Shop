using HashidsNet;
using Microsoft.Extensions.Logging;
using Moq;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Shop_3.Application.CustomQueries;
using Web_Shop_3.Application.DTOs.CustomerDTOs;
using Web_Shop_3.Application.Mappings;
using Web_Shop_3.Application.Mappings.PropertiesMappings;
using Web_Shop_3.Application.Services;
using Web_Shop_3.Persistence.MySQL.Model;
using Web_Shop_3.Persistence.UOW;
using Web_Shop_3.Tests.Common.Sieve;

namespace Web_Shop_3.Tests_InMemoryDB
{
    public class SqliteInMemoryCustomerServiceTest : IDisposable
    {
        private readonly SqliteDatabaseFixture _databaseFixture;

        private readonly Mock<ILogger<Customer>> _loggerMock;

        private readonly SieveProcessor _processor;
        private readonly SieveOptionsAccessor _optionsAccessor;

        public SqliteInMemoryCustomerServiceTest()
        {
            _databaseFixture = new SqliteDatabaseFixture();

            _loggerMock = new Mock<ILogger<Customer>>();

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
        public async Task CustomerService_CreateNewCustomerAsync_ReturnsTrue()
        {
            {
                using var context = _databaseFixture.CreateContext();

                var unitOfWork = new UnitOfWork(context);

                var customerService = new CustomerService(_loggerMock.Object, _processor, _optionsAccessor, unitOfWork);

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
                Assert.Equal("test@domain.com", verifyResult.entity!.Email);
            }
        }

        [Fact]
        public async Task CustomerService_SearchAsync_ReturnsTrue()
        {
            using var context = _databaseFixture.CreateContext();

            await context.Customers.AddAsync(new Customer
            {
                Name = "Tomasz",
                Surname = "Test",
                Email = "mateusz@test.com",
                BirthDate = new DateOnly(1990, 1, 1),
                PasswordHash = "somehashedvalue"
            });
            await context.SaveChangesAsync();

            var unitOfWork = new UnitOfWork(context);

            var customerService = new CustomerService(_loggerMock.Object, _processor, _optionsAccessor, unitOfWork);

            var hashIds = new Hashids(salt: "your-salt", minHashLength: 10);

            var model = new SieveModel
            {
                Filters = "Name@=Tomasz"
            };

            var searchResult = await customerService.SearchAsync(model, resultEntity => DomainToDtoMapper.MapGetSingleCustomerDTO(resultEntity, hashIds));

            Assert.True(searchResult.IsSuccess);
            Assert.Equal(1, searchResult.entityList!.TotalItemCount);
        }
    }
}

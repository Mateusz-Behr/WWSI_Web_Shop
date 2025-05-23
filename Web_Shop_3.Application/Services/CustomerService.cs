using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using System.Net;
using Web_Shop_3.Application.DTOs.CustomerDTOs;
using Web_Shop_3.Application.Mappings;
using Web_Shop_3.Application.Services.Interfaces;
using Web_Shop_3.Persistence.MySQL.Model;
using Web_Shop_3.Persistence.UOW.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace Web_Shop_3.Application.Services
{
    public class CustomerService : BaseService<Customer>, ICustomerService
    {
        public CustomerService(ILogger<Customer> logger,
                               ISieveProcessor sieveProcessor,
                               IOptions<SieveOptions> sieveOptions,
                               IUnitOfWork unitOfWork)
            : base(logger, sieveProcessor, sieveOptions, unitOfWork)
        {

        }

        public async Task<(bool IsSuccess, Customer? entity, HttpStatusCode StatusCode, string ErrorMessage)> CreateNewCustomerAsync(AddUpdateCustomerDTO dto)
        {
            try
            {
                if (await _unitOfWork.CustomerRepository.EmailExistsAsync(dto.Email))
                {
                    return (false, default(Customer), HttpStatusCode.BadRequest, "Email: " + dto.Email + " already registered.");
                }

                var newEntity = dto.MapCustomer();
                //newEntity.CreatedAt = DateTime.UtcNow;
                //newEntity.UpdatedAt = newEntity.CreatedAt;

                var result = await AddAndSaveAsync(newEntity);
                return (true, result.entity, HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return LogError(ex.Message);
            }
        }

        public async Task<(bool IsSuccess, Customer? entity, HttpStatusCode StatusCode, string ErrorMessage)> UpdateExistingCustomerAsync(AddUpdateCustomerDTO dto, ulong id)
        {
            try
            {
                var existingEntityResult = await WithoutTracking().GetByIdAsync(id);

                if (!existingEntityResult.IsSuccess)
                {
                    return existingEntityResult;
                }

                if (!await _unitOfWork.CustomerRepository.IsEmailEditAllowedAsync(dto.Email, id))
                {
                    return (false, default(Customer), HttpStatusCode.BadRequest, "Email: " + dto.Email + " already registered.");
                }

                var domainEntity = dto.MapCustomer();

                domainEntity.IdCustomer = id;
                if (!dto.IsPasswordUpdate)
                {
                    domainEntity.PasswordHash = existingEntityResult!.entity!.PasswordHash;
                }

                //domainEntity.CreatedAt = existingEntity.CreatedAt;
                //domainEntity.UpdatedAt = DateTime.UtcNow;
                //domainCustomer.UpdatedAt = DateTime.UtcNow.ConvertFromUtc(TimeZones.CentralEuropeanTimeZone);
                return await UpdateAndSaveAsync(domainEntity, id);
            }
            catch (Exception ex)
            {
                return LogError(ex.Message);
            }
        }

        public async Task<(bool IsSuccess, Customer? entity, HttpStatusCode StatusCode, string ErrorMessage)> VerifyPasswordByEmailAsync(string email, string password)
        {
            try
            {
                var existingEntity = await _unitOfWork.CustomerRepository.GetByEmailAsync(email);

                if (existingEntity == null || !BC.Verify(password, existingEntity.PasswordHash))
                {
                    return (false, default(Customer), HttpStatusCode.Unauthorized, "Invalid email or password.");
                }

                return (true, existingEntity, HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return LogError(ex.Message);
            }
        }
    }
}

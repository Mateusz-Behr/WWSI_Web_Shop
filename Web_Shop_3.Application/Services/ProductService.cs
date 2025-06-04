using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using System.Net;
using Web_Shop_3.Application.DTOs.ProductDTOs;
using Web_Shop_3.Application.Mappings;
using Web_Shop_3.Application.Services.Interfaces;
using Web_Shop_3.Persistence.MySQL.Model;
using Web_Shop_3.Persistence.UOW.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace Web_Shop_3.Application.Services
{
    public class ProductService : BaseService<Product>, IProductService
    {
        public ProductService(ILogger<Product> logger,
                               ISieveProcessor sieveProcessor,
                               IOptions<SieveOptions> sieveOptions,
                               IUnitOfWork unitOfWork)
            : base(logger, sieveProcessor, sieveOptions, unitOfWork)
        {

        }

        public async Task<(bool IsSuccess, Product? entity, HttpStatusCode StatusCode, string ErrorMessage)> CreateNewProductAsync(AddUpdateProductDTO dto)
        {
            try
            {
                if (await _unitOfWork.ProductRepository.ExistsByNameOrSkuAsync(dto.Name, dto.Sku))
                {
                    return (false, default(Product), HttpStatusCode.BadRequest, $"Product with name '{dto.Name}' or SKU '{dto.Sku}' already exists.");
                }

                var newEntity = dto.MapProduct();

                var result = await AddAndSaveAsync(newEntity);
                return (true, result.entity, HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return LogError(ex.Message);
            }
        }

        public async Task<(bool IsSuccess, Product? entity, HttpStatusCode StatusCode, string ErrorMessage)> UpdateExistingProductAsync(AddUpdateProductDTO dto, ulong id)
        {
            try
            {
                var existingEntityResult = await WithoutTracking().GetByIdAsync(id);

                if (!existingEntityResult.IsSuccess)
                {
                    return existingEntityResult;
                }

                if (!await _unitOfWork.ProductRepository.ExistsByNameOrSkuAsync(dto.Name, dto.Sku))
                {
                    return (false, default(Product), HttpStatusCode.BadRequest, $"Product with name '{dto.Name}' or SKU '{dto.Sku}' already exists.");
                }

                var domainEntity = dto.MapProduct();


                return await UpdateAndSaveAsync(domainEntity, id);
            }
            catch (Exception ex)
            {
                return LogError(ex.Message);
            }
        }
    }
}

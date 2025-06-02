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
                var existingResult = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
                if (existingResult == null)
                {
                    return (false, null, HttpStatusCode.NotFound, "Product not found");
                }

                if (await _unitOfWork.ProductRepository.ExistsByNameOrSkuAsync(dto.Name, dto.Sku, id))
                {
                    return (false, null, HttpStatusCode.BadRequest, "Name or SKU already exists for another product");
                }

                var product = existingResult;

                product.Name = dto.Name;
                product.Description = dto.Description;
                product.Price = dto.Price;
                product.Sku = dto.Sku;

                if (dto.CategoryIds != null)
                {
                    if (!dto.CategoryIds.Any())
                    {
                        product.IdCategories = new List<Category>();
                    }
                    else
                    {
                        var categories = new List<Category>();
                        foreach (var catId in dto.CategoryIds)
                        {
                            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(catId);
                            if (category == null)
                            {
                                return (false, null, HttpStatusCode.BadRequest, $"Category with ID {catId} does not exist");
                            }
                            categories.Add(category);
                        }
                        product.IdCategories = categories;
                    }
                }
                return await UpdateAndSaveAsync(product, id);
            }
            catch (Exception ex)
            {
                return LogError(ex.Message);
            }
        }
    }
}

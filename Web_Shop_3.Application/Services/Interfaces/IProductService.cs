using System.Net;
using Web_Shop_3.Application.DTOs.ProductDTOs;
using Web_Shop_3.Persistence.MySQL.Model;

namespace Web_Shop_3.Application.Services.Interfaces
{
    public interface IProductService : IBaseService<Product>
    {
        Task<(bool IsSuccess, Product? entity, HttpStatusCode StatusCode, string ErrorMessage)> CreateNewProductAsync(AddUpdateProductDTO dto);
        Task<(bool IsSuccess, Product? entity, HttpStatusCode StatusCode, string ErrorMessage)> UpdateExistingProductAsync(AddUpdateProductDTO dto, ulong id);

        //Task<bool> ProductExistsByNameOrSkuAsync(string name, string sku);
        //Task<bool> CategoryExistsAsync(ulong categoryId);
        //Task<(bool isExists, ulong? categoryId)> FindCategoryByNameAsync(string categoryName);
    }
}

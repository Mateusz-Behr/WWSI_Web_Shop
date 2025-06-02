using HashidsNet;
using Web_Shop_3.Application.DTOs;
using Web_Shop_3.Application.DTOs.CategoryDTOs;
using Web_Shop_3.Application.DTOs.CustomerDTOs;
using Web_Shop_3.Application.DTOs.ProductDTOs;
using Web_Shop_3.Application.Utils;
using Web_Shop_3.Persistence.MySQL.Model;

namespace Web_Shop_3.Application.Mappings
{
    public static class DomainToDtoMapper
    {
        public static GetSingleCustomerDTO MapGetSingleCustomerDTO(this Customer domainCustomer, IHashids hashIds)
        {
            if (domainCustomer == null)
                throw new ArgumentNullException(nameof(domainCustomer));

            GetSingleCustomerDTO getSingleCustomerDTO = new()
            {
                hashIdCustomer = domainCustomer.IdCustomer.EncodeHashid(hashIds),
                Name = domainCustomer.Name,
                Surname = domainCustomer.Surname,
                Email = domainCustomer.Email,
                BirthDate = domainCustomer.BirthDate,
            };

            return getSingleCustomerDTO;
        }

        public static GetSingleCategoryDTO MapGetSingleCategoryDTO(this Category domainCategory)
        {
            if (domainCategory == null)
                throw new ArgumentNullException(nameof(domainCategory));

            GetSingleCategoryDTO getSingleCategoryDTO = new()
            {
                IdCategory = domainCategory.IdCategory,
                Name = domainCategory.Name,
                Description = domainCategory.Description,
            };

            return getSingleCategoryDTO;
        }

        public static GetSingleProductDTO MapGetSingleProductDTO(this Product domainProduct)
        {
            if (domainProduct == null)
                throw new ArgumentNullException(nameof(domainProduct));

            GetSingleProductDTO getSingleProductDTO = new()
            {
                IdProduct = domainProduct.IdProduct,
                Name = domainProduct.Name,
                Description = domainProduct.Description,
                Price = domainProduct.Price,
                Sku = domainProduct.Sku,
            };

            return getSingleProductDTO;
        }

        public static AddUpdateProductDTO MapToAddUpdateDTO(this Product domainProduct)
        {
            if (domainProduct == null)
                throw new ArgumentNullException(nameof(domainProduct));

            return new AddUpdateProductDTO
            {
                Name = domainProduct.Name,
                Description = domainProduct.Description,
                Price = domainProduct.Price,
                Sku = domainProduct.Sku,
                CategoryIds = domainProduct.IdCategories?.Select(c => (ulong)c.IdCategory).ToList()
            };
        }
    }
}

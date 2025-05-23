
using Web_Shop_3.Application.DTOs;
using Web_Shop_3.Application.DTOs.CustomerDTOs;
using Web_Shop_3.Persistence.MySQL.Model;

namespace Web_Shop_3.Application.Mappings
{
    public static class DomainToDtoMapper
    {
        public static GetSingleCustomerDTO MapGetSingleCustomerDTO(this Customer domainCustomer)
        {
            if (domainCustomer == null)
                throw new ArgumentNullException(nameof(domainCustomer));

            GetSingleCustomerDTO getSingleCustomerDTO = new()
            {
                IdCustomer = domainCustomer.IdCustomer,
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
    }
}

using System.Net;
using Web_Shop_3.Application.DTOs.CustomerDTOs;
using Web_Shop_3.Persistence.MySQL.Model;

namespace Web_Shop_3.Application.Services.Interfaces
{
    public interface ICustomerService : IBaseService<Customer>
    {
        Task<(bool IsSuccess, Customer? entity, HttpStatusCode StatusCode, string ErrorMessage)> CreateNewCustomerAsync(AddUpdateCustomerDTO dto);
        Task<(bool IsSuccess, Customer? entity, HttpStatusCode StatusCode, string ErrorMessage)> UpdateExistingCustomerAsync(AddUpdateCustomerDTO dto, ulong id);
        Task<(bool IsSuccess, Customer? entity, HttpStatusCode StatusCode, string ErrorMessage)> VerifyPasswordByEmailAsync(string email, string password);
    }
}

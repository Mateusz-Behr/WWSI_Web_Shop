﻿using Web_Shop_3.Application.DTOs.CustomerDTOs;
using Web_Shop_3.Persistence.MySQL.Model;
using BC = BCrypt.Net.BCrypt;

namespace Web_Shop_3.Application.Mappings
{
    public static class DtoToDomainMapper
    {
        public static Customer MapCustomer(this AddUpdateCustomerDTO dtoCustomer)
        {
            if (dtoCustomer == null)
                throw new ArgumentNullException(nameof(dtoCustomer));

            Customer domainCustomer = new()
            {
                Name = dtoCustomer.Name,
                Surname = dtoCustomer.Surname,
                Email = dtoCustomer.Email,
                BirthDate = dtoCustomer.BirthDate,
                PasswordHash = BC.HashPassword(dtoCustomer.Password)
            };

            return domainCustomer;
        }
    }
}

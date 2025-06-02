using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_Shop_3.Application.DTOs.CustomerDTOs
{
    public class GetSingleCustomerDTO
    {
        public string hashIdCustomer { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public string Email { get; set; } = null!;

        public DateOnly? BirthDate { get; set; }
    }
}

﻿using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Shop_3.Persistence.MySQL.Model;

namespace Web_Shop_3.Application.Mappings.PropertiesMappings
{
    public class SieveConfigurationForCustomer : ISieveConfiguration
    {
        public void Configure(SievePropertyMapper mapper)
        {
            mapper.Property<Customer>(p => p.Name)
                .CanSort()
                .CanFilter();

            mapper.Property<Customer>(p => p.Surname)
                 .CanSort()
                 .CanFilter();

            mapper.Property<Customer>(p => p.Email)
                 .CanSort()
                 .CanFilter();

            mapper.Property<Customer>(p => p.BirthDate)
                 .CanSort()
                 .CanFilter();
        }
    }
}

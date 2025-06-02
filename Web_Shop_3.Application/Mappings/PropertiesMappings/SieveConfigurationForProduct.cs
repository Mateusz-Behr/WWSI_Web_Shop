using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Shop_3.Persistence.MySQL.Model;

namespace Web_Shop_3.Application.Mappings.PropertiesMappings
{
    public class SieveConfigurationForProduct : ISieveConfiguration
    {
        public void Configure(SievePropertyMapper mapper)
        {
            mapper.Property<Product>(p => p.Name)
                .CanSort()
                .CanFilter();

            mapper.Property<Product>(p => p.Description)
                 .CanSort()
                 .CanFilter();

            mapper.Property<Product>(p => p.Price)
                 .CanSort()
                 .CanFilter();

            mapper.Property<Product>(p => p.Sku)
                 .CanSort()
                 .CanFilter();
        }
    }
}

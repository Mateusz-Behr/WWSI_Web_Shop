using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Shop_3.Persistence.MySQL.Model;

namespace Web_Shop_3.Application.Mappings.PropertiesMappings
{
    public class SieveConfigurationForCategory : ISieveConfiguration
    {
        public void Configure(SievePropertyMapper mapper)
        {
            mapper.Property<Category>(p => p.Name)
                .CanSort()
                .CanFilter();

            mapper.Property<Category>(p => p.Description)
                 .CanSort()
                 .CanFilter();

        }
    }
}

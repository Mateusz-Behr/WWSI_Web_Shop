using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using System.Net;
using Web_Shop_3.Application.DTOs;
using Web_Shop_3.Application.Extensions;
using Web_Shop_3.Application.Helpers.PagedList;
using Web_Shop_3.Application.Mappings;
using Web_Shop_3.Application.Services.Interfaces;
using Web_Shop_3.Persistence.MySQL.Model;
using Web_Shop_3.Persistence.UOW.Interfaces;

namespace Web_Shop_3.Application.Services
{
    public class CategoryService : BaseService<Category>, ICategoryService
    {
        public CategoryService(ILogger<Category> logger,
                               ISieveProcessor sieveProcessor,
                               IOptions<SieveOptions> sieveOptions,
                               IUnitOfWork unitOfWork)
            : base(logger, sieveProcessor, sieveOptions, unitOfWork)
        {

        }
    }
}

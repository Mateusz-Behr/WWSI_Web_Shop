using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Web_Shop_3.Persistence.Repositories.Interfaces;
using Web_Shop_3.Persistence.MySQL.Context;
using Web_Shop_3.Persistence.UOW.Interfaces;
using Web_Shop_3.Application.Services.Interfaces;
using Web_Shop_3.Application.Mappings;
using Sieve.Models;
using Web_Shop_3.Application.Helpers.PagedList;
using Web_Shop_3.Application.DTOs.CategoryDTOs;
using HashidsNet;

namespace Web_Shop_3.RestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ILogger<CategoryController> logger,
                                  IHashids hashIds,
                                  ICategoryService CategoryService) : base(hashIds)
        {
            _categoryService = CategoryService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "GetCategoryById")]
        public async Task<ActionResult<GetSingleCategoryDTO>> GetCategory(uint id)
        {
            var result = await _categoryService.GetByIdAsync(id);

            if (!result.IsSuccess)
            {
                return Problem(statusCode: (int)result.StatusCode, title: "Read error.", detail: result.ErrorMessage);
            }

            return StatusCode((int)result.StatusCode, result.entity!.MapGetSingleCategoryDTO());  // "entity!" wykrzyknik to null forgivin operator - jak jesteśmy pewni, że tu nie będzie nulla (bo znamy implementację) to można tego użyć; ale trzeba być OSTROŻNYM, żeby nie używać tego tak o
        }

        [HttpGet("list")]
        [SwaggerOperation(OperationId = "GetCategories")]
        public async Task<ActionResult<IPagedList<GetSingleCategoryDTO>>> GetCategories([FromQuery] SieveModel paginationParams)
        {
            var result = await _categoryService.SearchAsync(paginationParams, 
                                                            resultEntity => DomainToDtoMapper.MapGetSingleCategoryDTO(resultEntity));

            if (!result.IsSuccess)
            {
                return Problem(statusCode: (int)result.StatusCode, title: "Read error.", detail: result.ErrorMessage);
            }

            return Ok(result.entityList);
        }
    }
}

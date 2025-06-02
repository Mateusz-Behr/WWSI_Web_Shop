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
using Web_Shop_3.Application.DTOs.ProductDTOs;

namespace Web_Shop_3.RestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _ProductService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger, IProductService ProductService)
        {
            _ProductService = ProductService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "GetProductById")]
        public async Task<ActionResult<GetSingleProductDTO>> GetProduct(ulong id)
        {
            var result = await _ProductService.GetByIdAsync(id);

            if (!result.IsSuccess)
            {
                return Problem(statusCode: (int)result.StatusCode, title: "Read error.", detail: result.ErrorMessage);
            }

            return StatusCode((int)result.StatusCode, result.entity!.MapGetSingleProductDTO());  // "entity!" wykrzyknik to null forgivin operator - jak jesteśmy pewni, że tu nie będzie nulla (bo znamy implementację) to można tego użyć; ale trzeba być OSTROŻNYM, żeby nie używać tego tak o
        }

        [HttpGet("list")]
        [SwaggerOperation(OperationId = "GetProducts")]
        public async Task<ActionResult<IPagedList<GetSingleProductDTO>>> GetProducts([FromQuery] SieveModel paginationParams)
        {
            var result = await _ProductService.SearchAsync(paginationParams, resultEntity => DomainToDtoMapper.MapGetSingleProductDTO(resultEntity));

            if (!result.IsSuccess)
            {
                return Problem(statusCode: (int)result.StatusCode, title: "Read error.", detail: result.ErrorMessage);
            }

            return Ok(result.entityList);
        }

        [HttpPost("add")]
        [SwaggerOperation(OperationId = "AddProduct")]
        public async Task<ActionResult<GetSingleProductDTO>> AddProduct([FromBody] AddUpdateProductDTO dto)
        {
            var result = await _ProductService.CreateNewProductAsync(dto);

            if (!result.IsSuccess)
            {
                return Problem(statusCode: (int)result.StatusCode, title: "Add error.", detail: result.ErrorMessage);
            }

            return CreatedAtAction(nameof(GetProduct), new { id = result.entity!.IdProduct }, result.entity.MapGetSingleProductDTO());
        }

        [HttpPut("update/{id}")]
        [SwaggerOperation(OperationId = "UpdateProduct")]
        public async Task<ActionResult<GetSingleProductDTO>> UpdateProduct(ulong id, [FromBody] AddUpdateProductDTO dto)
        {
            var result = await _ProductService.UpdateExistingProductAsync(dto, id);

            if (!result.IsSuccess)
            {
                return Problem(statusCode: (int)result.StatusCode, title: "Update error.", detail: result.ErrorMessage);
            }

            return StatusCode((int)result.StatusCode, result.entity!.MapGetSingleProductDTO());
        }


        [HttpDelete("{id}")]
        [SwaggerOperation(OperationId = "DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(ulong id)
        {
            var result = await _ProductService.DeleteAndSaveAsync(id);

            if (!result.IsSuccess)
            {
                return Problem(statusCode: (int)result.StatusCode, title: "Delete error.", detail: result.ErrorMessage);
            }

            return NoContent();
        }
    }
}

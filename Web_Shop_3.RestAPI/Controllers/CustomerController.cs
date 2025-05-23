﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Web_Shop_3.Persistence.Repositories.Interfaces;
using Web_Shop_3.Persistence.MySQL.Context;
using Web_Shop_3.Persistence.UOW.Interfaces;
using Web_Shop_3.Application.Services.Interfaces;
using Web_Shop_3.Application.DTOs;
using Web_Shop_3.Application.Mappings;
using Sieve.Models;
using Web_Shop_3.Application.Helpers.PagedList;
using Web_Shop_3.Application.DTOs.CustomerDTOs;

namespace Web_Shop_3.RestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ILogger<CustomerController> logger, ICustomerService customerService)
        {
            _customerService = customerService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "GetCustomerById")]
        public async Task<ActionResult<GetSingleCustomerDTO>> GetCustomer(ulong id)
        {
            var result = await _customerService.GetByIdAsync(id);

            if (!result.IsSuccess)
            {
                return Problem(statusCode: (int)result.StatusCode, title: "Read error.", detail: result.ErrorMessage);
            }

            return StatusCode((int)result.StatusCode, result.entity!.MapGetSingleCustomerDTO());  // "entity!" wykrzyknik to null forgivin operator - jak jesteśmy pewni, że tu nie będzie nulla (bo znamy implementację) to można tego użyć; ale trzeba być OSTROŻNYM, żeby nie używać tego tak o
        }

        [HttpGet("list")]
        [SwaggerOperation(OperationId = "GetCustomers")]
        public async Task<ActionResult<IPagedList<GetSingleCustomerDTO>>> GetCustomers([FromQuery] SieveModel paginationParams)
        {
            var result = await _customerService.SearchAsync(paginationParams, resultEntity => DomainToDtoMapper.MapGetSingleCustomerDTO(resultEntity));

            if (!result.IsSuccess)
            {
                return Problem(statusCode: (int)result.StatusCode, title: "Read error.", detail: result.ErrorMessage);
            }

            return Ok(result.entityList);
        }

        [HttpPost("add")]
        [SwaggerOperation(OperationId = "AddCustomer")]
        public async Task<ActionResult<GetSingleCustomerDTO>> AddCustomer([FromBody] AddUpdateCustomerDTO dto)
        {
            var result = await _customerService.CreateNewCustomerAsync(dto);

            if (!result.IsSuccess)
            {
                return Problem(statusCode: (int)result.StatusCode, title: "Add error.", detail: result.ErrorMessage);
            }

            return CreatedAtAction(nameof(GetCustomer), new { id = result.entity!.IdCustomer }, result.entity.MapGetSingleCustomerDTO());
        }

        [HttpPut("update/{id}")]
        [SwaggerOperation(OperationId = "UpdateCustomer")]
        public async Task<ActionResult<GetSingleCustomerDTO>> UpdateCustomer(ulong id, [FromBody] AddUpdateCustomerDTO dto)
        {
            var result = await _customerService.UpdateExistingCustomerAsync(dto, id);

            if (!result.IsSuccess)
            {
                return Problem(statusCode: (int)result.StatusCode, title: "Update error.", detail: result.ErrorMessage);
            }

            return StatusCode((int)result.StatusCode, result.entity!.MapGetSingleCustomerDTO());
        }

        [HttpGet("verifyPassword/{email}/{password}")]
        [SwaggerOperation(OperationId = "VerifyPasswordByEmail")]
        public async Task<ActionResult<GetSingleCustomerDTO>> VerifyPasswordByEmail(string email, string password)
        {
            var result = await _customerService.VerifyPasswordByEmailAsync(email, password);

            if (!result.IsSuccess)
            {
                return Problem(statusCode: (int)result.StatusCode, title: "Read error.", detail: result.ErrorMessage);
            }

            return StatusCode((int)result.StatusCode, result.entity!.MapGetSingleCustomerDTO());
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(OperationId = "DeleteCustomer")]
        public async Task<IActionResult> DeleteCustomer(ulong id)
        {
            var result = await _customerService.DeleteAndSaveAsync(id);

            if (!result.IsSuccess)
            {
                return Problem(statusCode: (int)result.StatusCode, title: "Delete error.", detail: result.ErrorMessage);
            }

            return NoContent();
        }
    }
}

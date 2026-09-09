using Ecommerce.Application.DTOs.TypeDtos;
using Ecommerce.Application.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeController : ControllerBase
    {
        private readonly ITypeService _typeService;

        public TypeController(ITypeService typeService)
        {
            _typeService = typeService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateType([FromForm]TypeSendDto dto)
        {
            var result = await _typeService.CreateTypeAsync(dto);
            return (result.Success)? Ok(result):BadRequest(result); 
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteType([FromQuery] int typeId)
        {
            var result = await _typeService.DeleteTypeAsync(typeId);
            return (result.Success) ? Ok(result) : BadRequest(result);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetTypeById([FromQuery] int typeId)
        {
            var result = await _typeService.GetTypeByTypeIdAsync(typeId);
            return (result.Success) ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTypes()
        {
            var result = await _typeService.GetAllTypesAsync();
            return (result.Success) ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateType([FromQuery] int typeId, [FromForm] TypeSendDto dto)
        {
            var result = await _typeService.UpdateTypeByTypeIdAsync(typeId, dto);
            return (result.Success) ? Ok(result) : BadRequest(result);
        }
    }
}

using Ecommerce.Application.DTOs.BrandDtos;
using Ecommerce.Application.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;

namespace Ecommerce.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;
        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateProductBrand([FromForm] BrandSendDto dto)
        {
            var result = await _brandService.CreateProductBrandAsync(dto);
            return (result.Success) ? Ok(result) : BadRequest(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBrandById([FromQuery] int brandId)
        {
            var result = await _brandService.DeleteBrandByIdAsync(brandId);
            return (result.Success) ? Ok(result) : BadRequest(result);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetBrandByBrandId([FromQuery] int brandId) {
            var result = await _brandService.GetBrandByBrandIdAsync(brandId);
            return (result.Success) ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBrands()
        {
            var result = await _brandService.GetAllBrandsAsync();
            return (result.Success) ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBrand([FromQuery] int brandId, [FromForm] BrandSendDto dto)
        {
            var result = await _brandService.UpdateBrandByIdAsync(brandId, dto);
            return (result.Success) ? Ok(result) : BadRequest(result);
        }
    } 
}

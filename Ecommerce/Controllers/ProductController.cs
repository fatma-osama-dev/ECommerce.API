using Ecommerce.Application.DTOs.ProductDtos;
using Ecommerce.Application.Helpers;
using Ecommerce.Application.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public  async Task<IActionResult> CreateProduct([FromForm] ProductSendDto dto){
            var result = await _productService.CreateProductAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }  

        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery]  ProductSpecParams productSpecParams)
        {
            var result = await _productService.GetAllProductsAsync(productSpecParams);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetProductById([FromQuery] int productId)
        {
            var result = await _productService.GetProductByIdAsync(productId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromQuery] int id, [FromForm] ProductUpdateDto dto)
        {
            var result = await _productService.UpdateProductAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProductByProductId([FromQuery] int productId)
        {
            var result = await _productService.DeleteProductAsync(productId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}

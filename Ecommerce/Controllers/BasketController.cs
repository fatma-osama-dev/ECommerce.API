using Ecommerce.Application.DTOs.Basket;
using Ecommerce.Application.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBasket([FromQuery] string basketId)
        {
            var result = await _basketService.DeleteCustomerBasketByBasketIdAsync(basketId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("by BasketId")]
        public async Task<IActionResult> GetBasket([FromQuery] string basketId)
        {
            var result = await _basketService.GetCustomerBasketByBasketIdAsync(basketId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBasket([FromBody] CustomerBasketDto basket)
        {
            var result = await _basketService.UpdateCustomerBasketAsync(basket);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}

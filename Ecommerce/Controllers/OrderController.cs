using Ecommerce.Application.DTOs.Order;
using Ecommerce.Application.Response;
using Ecommerce.Application.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost]
        public async Task<ActionResult<BaseResponse<OrderGetDto>>> CreateOrder([FromBody] OrderSendDto orderDto)
        {

            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(buyerEmail))
                return BadRequest(new BaseResponse<OrderGetDto>(false, "User email not found in token validation!"));

            var result = await _orderService.CreateOrderAsync(buyerEmail, orderDto);

            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse<IReadOnlyList<OrderGetDto>>>> GetOrdersForUser()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var result = await _orderService.GetOrdersForUserAsync(buyerEmail!);

            if (!result.Success) return Ok(result);
            return NotFound(result);
           
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<OrderGetDto>>> GetOrderById([FromRoute] int id)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var result = await _orderService.GetOrderByIdAsync(id, buyerEmail!);

            if (result.Success) return BadRequest(result);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("delivery-methods")]
        public async Task<ActionResult<BaseResponse<IReadOnlyList<DeliveryMethodDto>>>> GetDeliveryMethods()
        {
            var result = await _orderService.GetDeliveryMethodsAsync();

            if (result.Success) return Ok(result);
            return BadRequest(result);
        }





    }
}

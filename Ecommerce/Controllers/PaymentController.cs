using Ecommerce.Application.DTOs.Basket;
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
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;

        }
        [HttpPost]
        public async Task<ActionResult<BaseResponse<CustomerBasketDto>>> CreateOrUpdatePaymentIntent()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized( new BaseResponse<CustomerBasketDto>( false, "User is not authenticated."));
            }

            var result = await _paymentService.CreateOrUpdatePaymentIntentAsync(userId);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
       
       

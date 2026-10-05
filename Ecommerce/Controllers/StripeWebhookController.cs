using Ecommerce.Application.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Ecommerce.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeWebhookController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IPaymentService _paymentService;
        public StripeWebhookController(IConfiguration configuration, IPaymentService paymentService)
        {
            _configuration = configuration;
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<IActionResult> HandleWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
         

            var webhookSecret = _configuration["StripeSettings:WebhookSecret"];
         
            var stripeSignature = Request.Headers["Stripe-Signature"];

            Event stripeEvent;

            try
            {
                stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, webhookSecret);
            }
            catch (StripeException)
            {
                return BadRequest();
            }

            if (stripeEvent.Type == "payment_intent.succeeded")
            {
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
               

                if (paymentIntent == null)
                {
                    return BadRequest();
                }

                if (!paymentIntent.Metadata.TryGetValue("BasketId", out var basketId))
                {
                    return BadRequest("BasketId was not found in PaymentIntent metadata.");
                }

                var result = await _paymentService.HandlePaymentSucceededAsync(paymentIntent.Id, basketId);

                if (!result.Success)
                {
                    return BadRequest(result);
                }
            }

            return Ok();
        }
    }
}
    
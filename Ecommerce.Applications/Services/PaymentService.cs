using AutoMapper;
using Ecommerce.Application.DTOs.Basket;
using Ecommerce.Application.DTOs.Order;
using Ecommerce.Application.Response;
using Ecommerce.Application.ServiceInterfaces;
using Ecommerce.Application.Services;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Ecommerce.Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services
{

    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IGenericRepository<Domain.Entities.Product> _productRepo;
        private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;


        public PaymentService(IBasketRepository basketRepo, IGenericRepository<Domain.Entities.Product> productRepo, IGenericRepository<DeliveryMethod> deliveryMethodRepo, IConfiguration configuration, IMapper mapper, UserManager<AppUser> userManager, IOrderRepository    orderRepo)
        {
            _basketRepo = basketRepo;
            _productRepo = productRepo;
            _deliveryMethodRepo = deliveryMethodRepo;
            _orderRepo = orderRepo;
            _configuration = configuration;
            _mapper = mapper;
            _userManager = userManager;

        }


        public async Task<BaseResponse<CustomerBasketDto>> CreateOrUpdatePaymentIntentAsync(string userId)
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];


                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return new BaseResponse<CustomerBasketDto>(false, "User not found.");
                }


                if (string.IsNullOrEmpty(user.BasketId))
                {
                    return new BaseResponse<CustomerBasketDto>(false, "User does not have a basket.");
                }


                var basket = await _basketRepo.GetCustomerBasketByBasketIdAsync(user.BasketId);

                if (basket == null)
                {
                    return new BaseResponse<CustomerBasketDto>(false, "Basket not found.");
                }

                if (!basket.Basket.Any())
                {
                    return new BaseResponse<CustomerBasketDto>(false, "Basket is empty.");
                }


                if (!basket.DeliveryMethodId.HasValue)
                {
                    return new BaseResponse<CustomerBasketDto>(false, "Delivery method has not been selected.");
                }

                var deliveryMethod =
                    await _deliveryMethodRepo.GetByIdAsync(basket.DeliveryMethodId.Value);

                if (deliveryMethod == null)
                {
                    return new BaseResponse<CustomerBasketDto>(false, "Invalid delivery method.");
                }

                var shippingPrice = deliveryMethod.Cost;


                foreach (var item in basket.Basket)
                {
                    var product = await _productRepo.GetByIdAsync(item.Id);

                    if (product == null)
                    {
                        return new BaseResponse<CustomerBasketDto>(false, $"Product with id {item.Id} was not found.");
                    }

                    item.Price = product.Price;
                    item.ProductName = product.Name;
                    item.PictureUrl = product.PictureUrl;
                }


                var subtotal = basket.Basket.Sum(x => x.Price * x.Quantity);

                var total = subtotal + shippingPrice;

                var totalAmount = (long)(total * 100);


                var service = new PaymentIntentService();

                PaymentIntent intent;

                if (string.IsNullOrEmpty(basket.PaymentIntentId))
                {
                    var options = new PaymentIntentCreateOptions
                    {
                        Amount = totalAmount,
                        Currency = "usd",
                        PaymentMethodTypes = new List<string> { "card" },
                        Metadata = new Dictionary<string, string>
                            {
                                { "BasketId", basket.Id }
                            }
                    };

                    intent = await service.CreateAsync(options);

                    basket.PaymentIntentId = intent.Id;
                    basket.ClientSecret = intent.ClientSecret;
                }
                else
                {
                    var options = new PaymentIntentUpdateOptions
                    {
                        Amount = totalAmount,

                        Metadata = new Dictionary<string, string>
                                    {
                                        { "BasketId", basket.Id }
                                    }
                    };

                    intent = await service.UpdateAsync(basket.PaymentIntentId, options);
                }

                await _basketRepo.UpdateCustomerBasketAsync(basket);


                var mappedBasket = _mapper.Map<CustomerBasketDto>(basket);

                return new BaseResponse<CustomerBasketDto>(true, "Payment intent initialized successfully.", mappedBasket);
            }
            catch (Exception ex)
            {
                return new BaseResponse<CustomerBasketDto>(false, "Payment initialization failed.", ex);
            }
        }

        public async Task<BaseResponse<bool>> HandlePaymentSucceededAsync(string paymentIntentId, string basketId)
        {
            try
            {
                var order = await _orderRepo.GetOrderByPaymentIntentIdAsync(paymentIntentId);

                if (order == null)
                {
                    return new BaseResponse<bool>(false, "Order associated with this payment was not found.");
                 
                }

                if (order.Status != OrderStatus.PaymentReceived)
                {
                    order.Status = OrderStatus.PaymentReceived;

                    var result = await _orderRepo.SaveChangesAsync();

                    if (result <= 0)
                    {
                        return new BaseResponse<bool>(false, "Failed to update order payment status.");
                    }
                }

                await _basketRepo.DeleteCustomerBasketByBasketIdAsync(basketId);

                return new BaseResponse<bool>(true, "Payment succeeded , order updated and basket deleted.");
                   
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>(false, "An error occurred while processing successful payment.", ex);
             
            }
        }
    }
}

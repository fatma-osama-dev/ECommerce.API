using AutoMapper;
using Ecommerce.Application.DTOs.Order;
using Ecommerce.Application.Response;
using Ecommerce.Application.ServiceInterfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Ecommerce.Domain.RepositoryInterfaces;
using Ecommerce.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
        private readonly IBasketRepository _basketRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public OrderService(IOrderRepository orderRepo, IGenericRepository<DeliveryMethod> deliveryMethodRepo, IBasketRepository basketRepo, IMapper mapper, UserManager<AppUser> userManager)
        {
            _orderRepo = orderRepo;
            _deliveryMethodRepo = deliveryMethodRepo;
            _basketRepo = basketRepo;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<BaseResponse<OrderGetDto>> CreateOrderAsync(string userId, OrderSendDto orderDto)
     
        {
            try
            {

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return new BaseResponse<OrderGetDto>(false, "User not found.");

                }


                if (string.IsNullOrEmpty(user.BasketId))
                {
                    return new BaseResponse<OrderGetDto>(false, "User does not have a basket.");
                }

                var basketId = user.BasketId;


                var basket = await _basketRepo.GetCustomerBasketByBasketIdAsync(basketId);


                if (basket == null)
                {
                    return new BaseResponse<OrderGetDto>(false, "Basket not found or expired.");

                }

                if (!basket.Basket.Any())
                {
                    return new BaseResponse<OrderGetDto>(false, "Cannot create an order from an empty basket.");

                }



                if (!basket.DeliveryMethodId.HasValue)
                {
                    return new BaseResponse<OrderGetDto>(false, "Delivery method has not been selected.");
                }

                var deliveryMethod = await _deliveryMethodRepo.GetByIdAsync(basket.DeliveryMethodId.Value);
               
                        
                if (deliveryMethod == null)
                {
                    return new BaseResponse<OrderGetDto>(false, "Selected delivery method is invalid or does not exist.");  
                }


                var items = new List<OrderItem>();

                foreach (var basketItem in basket.Basket)
                {
                    var orderItem = new OrderItem
                    {
                        ProductId = basketItem.Id,
                        ProductName = basketItem.ProductName,
                        PictureUrl = basketItem.PictureUrl,
                        Price = basketItem.Price,
                        Quantity = basketItem.Quantity
                    };

                    items.Add(orderItem);
                }
                if (string.IsNullOrEmpty(basket.PaymentIntentId))
                {
                    return new BaseResponse<OrderGetDto>(false, "Payment has not been initialized for this basket.");
                  
                }

                var shippingAddress = _mapper.Map<OrderAddressDto, OrderAddress>(orderDto.ShipToAddress);

                var order = new Order
                {
                    BuyerEmail = user.Email!,
                    OrderDate = DateTime.UtcNow,
                    Status = OrderStatus.Pending,
                    ShipToAddress = shippingAddress,
                    DeliveryMethodId = deliveryMethod.Id,
                    DeliveryMethod = deliveryMethod,
                    OrderItems = items,
                    PaymentIntentId = basket.PaymentIntentId,
                };


                await _orderRepo.AddAsync(order);

                var result = await _orderRepo.SaveChangesAsync();

                if (result <= 0)
                {
                    return new BaseResponse<OrderGetDto>(false, "Failed to create order inside database.");

                }

                var mappedOrder = _mapper.Map<Order, OrderGetDto>(order);

                return new BaseResponse<OrderGetDto>(true, "Order created successfully.", mappedOrder);
             }
            
            catch (Exception ex)
            {
                return new BaseResponse<OrderGetDto>(false, "An unexpected error occurred while processing your order.", ex);
            }
        }

        public async Task<BaseResponse<IReadOnlyList<DeliveryMethodDto>>> GetDeliveryMethodsAsync()
        {
            try
            {
                var deliveryMethods = await _deliveryMethodRepo.GetAllAsync();

                if (deliveryMethods == null || !deliveryMethods.Any())
                {
                    return new BaseResponse<IReadOnlyList<DeliveryMethodDto>>(false, "No delivery methods found in the system.");
                }

                var methods = _mapper.Map<IReadOnlyCollection<DeliveryMethod>, IReadOnlyList<DeliveryMethodDto>>(deliveryMethods);

                return new BaseResponse<IReadOnlyList<DeliveryMethodDto>>(true, "Delivery methods retrieved successfully.", methods);

            }
            catch (Exception ex)
            {
                return new BaseResponse<IReadOnlyList<DeliveryMethodDto>>(false, "An unexpected error occurred while retrieving delivery methods.", ex);
            }
        }


        public async Task<BaseResponse<OrderGetDto>> GetOrderByIdAsync(int orderId, string buyerEmail)
        {
            try
            {
                var order = await _orderRepo.GetOrderByIdAsync(orderId, buyerEmail);

                if (order == null)
                {
                    return new BaseResponse<OrderGetDto>(false, "Order not found.");
                }

                var mappedOrder = _mapper.Map<Order, OrderGetDto>(order);

                return new BaseResponse<OrderGetDto>(true, "Order retrieved successfully.", mappedOrder);
               
            }

            catch (Exception ex)
            {
                return new BaseResponse<OrderGetDto>(false, "An unexpected error occurred while retrieving the order.", ex);
                  
            }
        }

        public async Task<BaseResponse<IReadOnlyList<OrderGetDto>>> GetOrdersForUserAsync(string buyerEmail)
        {
            try
            {
                var orders = await _orderRepo.GetOrdersForUserAsync(buyerEmail);

                if (orders == null || !orders.Any())
                {
                    return new BaseResponse<IReadOnlyList<OrderGetDto>>(false, "No orders found for the user.");

                }

                var mappedOrders = _mapper.Map<IReadOnlyCollection<Order>, IReadOnlyList<OrderGetDto>>(orders);

                return new BaseResponse<IReadOnlyList<OrderGetDto>>(true, "Orders retrieved successfully.", mappedOrders);

            }
            catch (Exception ex)
            {
                return new BaseResponse<IReadOnlyList<OrderGetDto>>(false, "An unexpected error occurred while retrieving orders for the user.", ex);

            } }


        public async Task<BaseResponse<bool>> HandlePaymentSucceededAsync(string paymentIntentId)
        {
            try
            {
                var order = await _orderRepo.GetOrderByPaymentIntentIdAsync(paymentIntentId);

                if (order == null)
                {
                    return new BaseResponse<bool>(false, "Order associated with this payment was not found.");

                }


                if (order.Status == OrderStatus.PaymentReceived)
                {
                    return new BaseResponse<bool>(true, "Payment was already processed.");
                }

                order.Status = OrderStatus.PaymentReceived;

                var result = await _orderRepo.SaveChangesAsync();

                if (result <= 0)
                {
                    return new BaseResponse<bool>(false, "Failed to update order payment status.");
                }

                return new BaseResponse<bool>(true, "Order payment status updated successfully.");

            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>(false, "An error occurred while updating order payment status.", ex);
            }
        }

        
    }
}
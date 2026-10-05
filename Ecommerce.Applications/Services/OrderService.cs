using AutoMapper;
using Ecommerce.Application.DTOs.Order;
using Ecommerce.Application.Response;
using Ecommerce.Application.ServiceInterfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.RepositoryInterfaces;
using Ecommerce.Infrastructure.Repositories;
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
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IBasketRepository _basketRepo;
        private readonly IMapper _mapper;

        public OrderService(
         IOrderRepository orderRepo,
         IGenericRepository<DeliveryMethod> deliveryMethodRepo,
         IGenericRepository<Product> productRepo,
         IBasketRepository basketRepo,
         IMapper mapper)
        {
            _orderRepo = orderRepo;
            _deliveryMethodRepo = deliveryMethodRepo;
            _productRepo = productRepo;
            _basketRepo = basketRepo;
            _mapper = mapper;
        }
        public async Task<BaseResponse<OrderGetDto>> CreateOrderAsync(string buyerEmail, OrderSendDto orderDto)
        {
            try
            {
            
                var basket = await _basketRepo.GetCustomerBasketByBasketIdAsync(orderDto.BasketId);
                if (basket == null)
                    return new BaseResponse<OrderGetDto>(false, "Basket not found or expired!");

                var items = new List<OrderItem>();
                foreach (var basketItem in basket.Basket)
                {
                   
                    var productItem = await _productRepo.GetByIdAsync(basketItem.Id);
                    if (productItem != null)
                    {
                        var orderItem = new OrderItem
                        {
                            ProductId = productItem.Id,
                            ProductName = productItem.Name,
                            PictureUrl = productItem.PictureUrl,
                            Price = productItem.Price,
                            Quantity = basketItem.Quantity
                        };
                        items.Add(orderItem);
                    }
                }

           
                var deliveryMethod = await _deliveryMethodRepo.GetByIdAsync(orderDto.DeliveryMethodId);
                if (deliveryMethod == null)
                    return new BaseResponse<OrderGetDto>(false, "Invalid Delivery Method!");

              
                var shippingAddress = _mapper.Map<OrderAddressDto, OrderAddress>(orderDto.ShipToAddress);

              
                var order = new Order
                {
                    BuyerEmail = buyerEmail,
                    ShipToAddress = shippingAddress,
                    DeliveryMethod = deliveryMethod,
                    OrderItems = items
                };

                
                await _orderRepo.AddAsync(order);
                var result = await _orderRepo.SaveChangesAsync();

                if (result<=0)
                    return new BaseResponse<OrderGetDto>(false, "Failed to create order inside database!");

                await _basketRepo.DeleteCustomerBasketByBasketIdAsync(orderDto.BasketId);

               
                var mappedOrder = _mapper.Map<Order, OrderGetDto>(order);

                return new BaseResponse<OrderGetDto>(true, "Order created successfully.", mappedOrder);
            }
            catch (Exception ex)
            {
              
                return new BaseResponse<OrderGetDto>(false, $"An unexpected error occurred while processing your order", ex);
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
            try { 
                var order = await _orderRepo.GetOrderByIdAsync(orderId, buyerEmail);
                if (order == null)
                {
                    return new BaseResponse<OrderGetDto>(false, "Order not found.");
                }
                var mappedOrder = _mapper.Map<Order, OrderGetDto>(order);
                return new BaseResponse<OrderGetDto>(true, "Order retrieved successfully.", mappedOrder);
            }
            catch (Exception ex) {
                return new BaseResponse<OrderGetDto>(false, "An unexpected error occurred while retrieving the order.", ex);
            }
        }

        public async Task<BaseResponse<IReadOnlyList<OrderGetDto>>> GetOrdersForUserAsync(string buyerEmail)
        {
            try {
                var orders = await _orderRepo.GetOrdersForUserAsync(buyerEmail);
                if(orders == null || !orders.Any())
                {
                    return new BaseResponse<IReadOnlyList<OrderGetDto>>(false, "No orders found for the user.");
                }
                var mappedOrders = _mapper.Map<IReadOnlyCollection<Order>, IReadOnlyList<OrderGetDto>>(orders); 
                return new BaseResponse<IReadOnlyList<OrderGetDto>>(true, "Orders retrieved successfully.", mappedOrders);
            }
            catch (Exception ex) { 
                return new BaseResponse<IReadOnlyList<OrderGetDto>>(false, "An unexpected error occurred while retrieving orders for the user.", ex);
            }
        }
    }
}

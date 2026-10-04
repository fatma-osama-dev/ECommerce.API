using Ecommerce.Application.DTOs.Order;
using Ecommerce.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.ServiceInterfaces
{
   
        public interface IOrderService
        {   Task<BaseResponse<OrderGetDto>> CreateOrderAsync(string buyerEmail, OrderSendDto orderDto);
            Task<BaseResponse<IReadOnlyList<OrderGetDto>>> GetOrdersForUserAsync(string buyerEmail);
            Task<BaseResponse<OrderGetDto>> GetOrderByIdAsync(int orderId, string buyerEmail);
            Task<BaseResponse<IReadOnlyList<DeliveryMethodDto>>> GetDeliveryMethodsAsync(); 
        
    }
}

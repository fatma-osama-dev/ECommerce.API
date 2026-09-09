using Ecommerce.Application.DTOs.Basket;
using Ecommerce.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.ServiceInterfaces
{
    public interface IBasketService
    {
      Task<BaseResponse<CustomerBasketDto>> GetCustomerBasketByBasketIdAsync(string basketId);
       Task<BaseResponse<CustomerBasketDto>> UpdateCustomerBasketAsync(CustomerBasketDto? basket);
       Task<BaseResponse<bool>> DeleteCustomerBasketByBasketIdAsync(string basketId);   
    }
}

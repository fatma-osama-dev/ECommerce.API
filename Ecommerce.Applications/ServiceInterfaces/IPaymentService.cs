using Ecommerce.Application.DTOs.Basket;
using Ecommerce.Application.Response;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.ServiceInterfaces
{
    public interface IPaymentService
    {
        Task<BaseResponse<CustomerBasketDto>> CreateOrUpdatePaymentIntentAsync(string userId);
        Task<BaseResponse<bool>> HandlePaymentSucceededAsync(string paymentIntentId, string basketId);

    }
}

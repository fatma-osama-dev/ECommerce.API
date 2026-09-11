using Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.RepositoryInterfaces
{
    public interface IOrderRepository
    {
       Task<Order?> GetOrderByIdAsync(int orderId);
       Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);
       
    }
}

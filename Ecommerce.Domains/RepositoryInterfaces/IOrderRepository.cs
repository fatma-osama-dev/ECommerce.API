using Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.RepositoryInterfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
       Task<Order?> GetOrderByIdAsync(int orderId, string buyerEmail);
       Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);
       
    }
}

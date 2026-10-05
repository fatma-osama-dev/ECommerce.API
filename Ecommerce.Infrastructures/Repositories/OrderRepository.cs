using Ecommerce.Domain.Entities;
using Ecommerce.Domain.RepositoryInterfaces;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repositories
{
    public class OrderRepository : GenericRepository<Order> , IOrderRepository
    {
        
        public OrderRepository(EcommerceDbContext context) : base(context)
        {
          
        }


        public async Task<Order?> GetOrderByIdAsync(int orderId, string buyerEmail)
        {
            return await _context.Orders.Include(o => o.OrderItems)
                                        .Include(o => o.DeliveryMethod)
                                        .FirstOrDefaultAsync(o => o.Id == orderId && o.BuyerEmail == buyerEmail);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.DeliveryMethod)
                .Where(o => o.BuyerEmail == buyerEmail)
                .OrderByDescending(o => o.OrderDate) 
                .ToListAsync();

        }
    }
}

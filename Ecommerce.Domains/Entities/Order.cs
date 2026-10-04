using Ecommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities
{
    public class Order : BaseEntity<int>
    {
       
        public string BuyerEmail { get; set; } = null!;
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public OrderAddress ShipToAddress { get; set; } = null!;
        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
        public int DeliveryMethodId { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; } = null!;
        public decimal GetTotal()
        {
            decimal subTotal = 0;
            foreach (var item in OrderItems)
            {
                subTotal += item.Price * item.Quantity;
            }
            if (DeliveryMethod != null)
            {
                subTotal += DeliveryMethod.Cost;
            }
            return subTotal;
        }
    }
}

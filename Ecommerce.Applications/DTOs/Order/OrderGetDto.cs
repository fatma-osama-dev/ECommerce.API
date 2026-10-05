using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Order
{
    public class OrderGetDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = null!; 
        public OrderAddressDto ShipToAddress { get; set; } = null!;
        public ICollection<OrderItemDto> OrderItems { get; set; } = new HashSet<OrderItemDto>();
        public string DeliveryMethodName { get; set; } = null!;
        public decimal ShippingPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}

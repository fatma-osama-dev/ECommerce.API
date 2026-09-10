using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities
{
    public class OrderItem:BaseEntity<int>
    {
        public int ProductId { get; set; } 
        public string ProductName { get; set; } = null!;
        public string PictureUrl { get; set; } = null!;
        public Decimal Price { get; set; }

        public int Quantity { get; set; } = 1;
    }
}

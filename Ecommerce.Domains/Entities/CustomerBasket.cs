using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities
{
    public class CustomerBasket : BaseEntity<string>
    {
        public ICollection<BasketItem> Basket { get; set; } = new HashSet<BasketItem>();
       
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal TotalPrice { get { return Basket.Sum(b => b.Price * b.Quantity) ; } }

    }
}

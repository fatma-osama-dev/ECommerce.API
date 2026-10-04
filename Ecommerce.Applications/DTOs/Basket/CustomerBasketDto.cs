using Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Basket
{
    public class CustomerBasketDto
    {
        public string id { get; set; } = null!;
        public ICollection<BasketItemDto?> Basket { get; set; } = new HashSet<BasketItemDto>();
        public string? PaymentIntentId { get; set; }

        public string? ClientSecret { get; set; }

        public int? DeliveryMethodId { get; set; }
        public decimal TotalPrice { get { return Basket?.Sum(x => x.Price * (x.Quantity ?? 0)) ?? 0; } }

    }
}

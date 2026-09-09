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
        public ICollection<BasketItemDto> Basket { get; set; } = new HashSet<BasketItemDto>();
        public decimal TotalPrice { get; }
    }
}

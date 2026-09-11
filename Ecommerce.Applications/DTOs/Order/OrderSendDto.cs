using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Order
{
    public class OrderSendDto
    {
        [Required] public string BasketId { get; set; } = null!;
        [Required] public int DeliveryMethodId { get; set; }
        [Required] public OrderAddressDto ShipToAddress { get; set; } = null!;
    }
}

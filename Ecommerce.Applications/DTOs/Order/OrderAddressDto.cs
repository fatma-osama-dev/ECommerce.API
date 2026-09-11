using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.Order
{
    public class OrderAddressDto
    {
        [Required] public string FirstName { get; set; } = null!;
        [Required] public string LastName { get; set; } = null!;
        [Required] public string Street { get; set; } = null!;
        [Required] public string City { get; set; } = null!;
        [Required] public string State { get; set; } = null!;
        [Required] public string ZipCode { get; set; } = null!;
    }
}

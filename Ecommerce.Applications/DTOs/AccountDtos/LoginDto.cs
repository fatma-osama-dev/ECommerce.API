using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.AccountDtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email address is required!")]
        [EmailAddress(ErrorMessage = "Invalid email address format!")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; } = null!;
    }
}

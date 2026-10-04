using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs.AccountDtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Display name is required!")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Display name must be between 3 and 50 characters!")]
        public string DisplayName { get; set; } = null!;

        [Required(ErrorMessage = "Email address is required!")]
        [EmailAddress(ErrorMessage = "Invalid email address format!")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required!")]
        [RegularExpression(@"(?=^.{6,10}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{""':;?/>.<,])(?!.*\s).*$",
            ErrorMessage = "Password must be 6-10 characters, and contain at least 1 uppercase, 1 lowercase, 1 number, and 1 special character!")]
        public string Password { get; set; } = null!;
    }
}

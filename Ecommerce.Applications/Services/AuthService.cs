using Ecommerce.Application.DTOs.AccountDtos;
using Ecommerce.Application.Response;
using Ecommerce.Application.ServiceInterfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services
{
    public class AuthService : IAuthService
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        public AuthService(UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }
        public async Task<BaseResponse<UserDto>> RegisterAsync(RegisterDto registerDto)
        {
            try
            {

                var userExists = await _userManager.FindByEmailAsync(registerDto.Email);
                if (userExists != null)
                {
                    return new BaseResponse<UserDto>(false, "Email address is already in use!");
                }
                var user = new AppUser
                {
                    DisplayName = registerDto.DisplayName,
                    Email = registerDto.Email,
                    UserName = registerDto.Email
                };
                var result = await _userManager.CreateAsync(user, registerDto.Password);
                if (!result.Succeeded)
                {

                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return new BaseResponse<UserDto>(false, $"Registration failed: {errors}");
                }
                var userDto = new UserDto
                {
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                };
                return new BaseResponse<UserDto>(true, "User registered successfully.", userDto);
            }

            catch (Exception ex)
            {
                return new BaseResponse<UserDto>(false, $"An error occurred during registration .", ex);
            }
        }

        public async Task<BaseResponse<UserDto>> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var userExists = await _userManager.FindByEmailAsync(loginDto.Email);
                if (userExists == null)
                {
                    return new BaseResponse<UserDto>(false, "Unauthorized! Invalid email or password.");
                }
                var result = await _userManager.CheckPasswordAsync(userExists, loginDto.Password);
                if (!result)
                {
                    return new BaseResponse<UserDto>(false, "Unauthorized! Invalid email or password.");
                }
                var userDto = new UserDto
                {
                    DisplayName = userExists.DisplayName,
                    Email = userExists.Email!,
                    Token = _tokenService.CreateToken(userExists)
                };
                return new BaseResponse<UserDto>(true, "Login successful.", userDto);
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserDto>(false, $"An error occurred during login.", ex);
            }


        }
    }
}

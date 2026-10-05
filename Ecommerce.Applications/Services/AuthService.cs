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
    namespace Ecommerce.Application.Services
    {
        public class AuthService : IAuthService
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly ITokenService _tokenService;
            private readonly IBasketService _basketService;

            public AuthService(UserManager<AppUser> userManager, ITokenService tokenService, IBasketService basketService)
            {
                _userManager = userManager;
                _tokenService = tokenService;
                _basketService = basketService;
            }

            public async Task<BaseResponse<UserDto>> RegisterAsync(
                RegisterDto registerDto)
            {
                try
                {
                    
                    var userExists =
                        await _userManager.FindByEmailAsync(registerDto.Email);

                    if (userExists != null)
                    {
                        return new BaseResponse<UserDto>(false, "Email address is already in use!");
                      
                    }

              
                    var user = new AppUser
                    {
                        DisplayName = registerDto.DisplayName,
                        Email = registerDto.Email,
                        UserName = registerDto.Email,
                        BasketId = null
                    };

                    var result =
                        await _userManager.CreateAsync(user, registerDto.Password);
                       

                    if (!result.Succeeded)
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        
                        return new BaseResponse<UserDto>(false, $"Registration failed: {errors}");
                     
                    }

                 
                    var basketResult = await _basketService.MergeOrAttachBasketAsync(user.BasketId, registerDto.BasketId);
                  

                    if (!basketResult.Success)
                    {
                        return new BaseResponse<UserDto>(false, basketResult.Message);
                    
                    }

                    user.BasketId = basketResult.Data?.id;

                   
                    var updateUserResult = await _userManager.UpdateAsync(user);
                   

                    if (!updateUserResult.Succeeded)
                    {
                        var errors = string.Join(", ",updateUserResult.Errors.Select(e => e.Description));
                       

                        return new BaseResponse<UserDto>(false, $"Failed to update user basket: {errors}");
                    }

            
                    var userDto = new UserDto
                    {
                        DisplayName = user.DisplayName,
                        Email = user.Email!,
                        BasketId = user.BasketId,
                        Token = _tokenService.CreateToken(user)
                    };

                    return new BaseResponse<UserDto>(true, "User registered successfully.", userDto);
                }
                catch (Exception ex)
                {
                    return new BaseResponse<UserDto>(false, "An error occurred during registration.", ex);
                }
            }


            public async Task<BaseResponse<UserDto>> LoginAsync( LoginDto loginDto)
            {
                try
                {
                 
                    var userExists =await _userManager.FindByEmailAsync(loginDto.Email);

                    if (userExists == null)
                    {
                        return new BaseResponse<UserDto>( false,  "Unauthorized! Invalid email or password.");
                    }

                    var passwordCorrect = await _userManager.CheckPasswordAsync( userExists, loginDto.Password);

                    if (!passwordCorrect)
                    {
                        return new BaseResponse<UserDto>(false, "Unauthorized! Invalid email or password.");
                    }

                   
                    var basketResult = await _basketService.MergeOrAttachBasketAsync(userExists.BasketId, loginDto.BasketId);

                    if (!basketResult.Success)
                    {
                        return new BaseResponse<UserDto>(false,basketResult.Message);
                    }

                    
                    userExists.BasketId = basketResult.Data?.id;

         
                    var updateUserResult = await _userManager.UpdateAsync(userExists);

                    if (!updateUserResult.Succeeded)
                    {
                        var errors = string.Join( ", ", updateUserResult.Errors.Select(e => e.Description));

                        return new BaseResponse<UserDto>( false, $"Failed to update user basket: {errors}");
                    }

  
                    var userDto = new UserDto
                    {
                        DisplayName = userExists.DisplayName,
                        Email = userExists.Email!,
                        BasketId = userExists.BasketId,
                        Token = _tokenService.CreateToken(userExists)
                    };

                    return new BaseResponse<UserDto>(true,"Login successful.", userDto);
                }
                catch (Exception ex)
                {
                    return new BaseResponse<UserDto>(false,"An error occurred during login.",ex);
                }
            }
        }
    }
}

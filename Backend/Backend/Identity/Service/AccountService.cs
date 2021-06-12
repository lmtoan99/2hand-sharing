using Application.DTOs.Account;
using Application.DTOs.Email;
using Application.Enums;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Contexts;
using Application.Interfaces.Repositories;
using Application.Interfaces.Service;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using Domain.Settings;
using Identity.Helpers;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Service
{
    class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly JWTSettings _jwtSettings;
        private readonly IUserRepositoryAsync _userRepository;
        private readonly string _host = "http://localhost:4200";
        private readonly IMapper _mapper;
        private readonly IImageRepository _imageRepository;
        //private readonly 
        public AccountService(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JWTSettings> jwtSettings,
            SignInManager<ApplicationUser> signInManager,
            IEmailService emailService,
            IUserRepositoryAsync userRepository,
            IMapper mapper,
            IImageRepository imageRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
            _signInManager = signInManager;
            this._emailService = emailService;
            this._userRepository = userRepository;
            _mapper = mapper;
            _imageRepository = imageRepository;
        }

        public async Task<Response<AuthenticateResponse>> AuthenticateAsync(AuthenticateRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new ApiException($"Wrong username or password");
            }
            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                throw new ApiException($"Wrong username or password");
            }
            if (!user.EmailConfirmed)
            {
                throw new ApiException($"Account Not Confirmed for '{request.Email}'.");
            }
            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);
            AuthenticateResponse response = new AuthenticateResponse();
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            response.Roles = rolesList.ToList().FirstOrDefault();
            response.IsVerified = user.EmailConfirmed;
            response.UserInfo = _mapper.Map<UserInfoDTO>(await _userRepository.GetUserInfoByUserId(user.Id));
            response.UserInfo.Email = request.Email;
            response.UserInfo.AvatarUrl = _imageRepository.GenerateV4SignedReadUrl(response.UserInfo.AvatarUrl);
            return new Response<AuthenticateResponse>(response, $"Authenticated {user.UserName}");
        }



        public async Task<Response<string>> RegisterAsync(RegisterRequest request)
        {
            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail != null)
            {
                throw new ApiException($"Email {request.Email } is already registered.");
            }

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                User userCreate = new User
                {
                    AccountId = user.Id,
                    FullName = request.FullName,
                    Dob = request.Dob,
                    PhoneNumber = request.PhoneNumber
                };
                _ = _userRepository.AddAsync(userCreate);

                await _userManager.AddToRoleAsync(user, Roles.Basic.ToString());
                var verificationUri = await SendVerificationEmail(user);
                //TODO: Attach Email Service here and configure it via appsettings
                await _emailService.SendAsync(new EmailRequest() {To = user.Email, Body = $"<div style='text-align: center;'>Tài khoản của bạn đã được đăng kí trên trang web {_host}. Hãy xác nhận tài khoản của bạn bằng cách click vào URL sau<br><a href='{verificationUri}'>CONFIRM ACCOUNT</a></div>", Subject = "Confirm Registration" });
                return new Response<string>(user.Id, message: $"User Registered. Please confirm your account by checking your email");
            }
            else
            {
                throw new ApiException($"{result.Errors.FirstOrDefault().Description}");
            }
        }

        private async Task<JwtSecurityToken> GenerateJWToken(ApplicationUser account)
        {
            var userClaims = await _userManager.GetClaimsAsync(account);
            var roles = await _userManager.GetRolesAsync(account);
            var user = await _userRepository.GetUserByAccountId(account.Id);
            string ipAddress = IpHelper.GetIpAddress();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, account.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, account.Email),
                new Claim("uid", user.Id.ToString()),
                new Claim("ip", ipAddress),
                new Claim("role",roles.FirstOrDefault().ToString())
            }
            .Union(userClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        private async Task<string> SendVerificationEmail(ApplicationUser user)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "auth/register/confirm";
            var _enpointUri = new Uri($"{_host}/{route}");
            var verificationUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "userid", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
            //Email Service Call Here
            return verificationUri;
        }

        public async Task<Response<string>> ConfirmEmailAsync(ConfirmEmailRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            request.Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
            var result = await _userManager.ConfirmEmailAsync(user, request.Code);
            if (result.Succeeded)
            {
                return new Response<string>(user.Id, message: $"Account Confirmed for {user.Email}");
            }
            else
            {
                throw new ApiException($"An error occured while confirming {user.Email}.");
            }
        }

        private RefreshToken GenerateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }

        public async Task ForgotPassword(ForgotPasswordRequest model)
        {
            var account = await _userManager.FindByEmailAsync(model.Email);
            // always return ok response to prevent email enumeration
            if (account == null) throw new ApiException("Email is not registered");

            var code = await _userManager.GeneratePasswordResetTokenAsync(account);
            var route = "auth/forgot-password/reset";
            var _enpointUri = new Uri($"{_host}/{route}?userid={account.Id}&code={code}");
            var emailRequest = new EmailRequest()
            {
                Body = $"<div style='text-align: center;'>Email phản hồi với yêu cầu đặt lại mật khẩu của bạn. Mời đặt lại mật khẩu qua URL:<br><a href='{ _enpointUri }'>RESET PASSWORD</a></div>",
                To = model.Email,
                Subject = "Reset Password",
            };
            await _emailService.SendAsync(emailRequest);
        }

        public async Task<Response<string>> ResetPassword(ResetPasswordRequest model)
        {
            var account = await _userManager.FindByIdAsync(model.UserId);
            if (account == null) throw new ApiException($"No Account Registered with {model.UserId}.");
            var result = await _userManager.ResetPasswordAsync(account, model.Token, model.Password);
            if (result.Succeeded)
            {
                return new Response<string>(model.UserId, message: $"Password Resetted.");
            }
            else
            {
                throw new ApiException($"Error occured while reseting the password.");
            }
        }

        public async Task<string> GetEmailById(string accountId)
        {
            var user = await _userManager.FindByIdAsync(accountId);
            return user.Email;
        }

        public async Task<string> GetAcccountIdByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                return null;
            }
            return user.Id;
        }
    }
}

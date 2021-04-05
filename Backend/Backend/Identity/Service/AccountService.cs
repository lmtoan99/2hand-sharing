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
        /*private readonly IApplicationDbContext _dbContext;
        private readonly DbSet<Account> _account;
        private readonly IMapper _mapper;
        private readonly JWTSettings _jwtSettings;
        private static Random random = new Random();
        private readonly IEmailService _emailService;
        public AccountService(IApplicationDbContext dbContext, IMapper mapper,IEmailService emailService, IOptions<JWTSettings> jwtSettings)
        {
            this._dbContext = dbContext;
            _account = dbContext.Accounts;
            _mapper = mapper;
            _emailService = emailService;
            _jwtSettings = jwtSettings.Value;
        }
        
        public async Task<Response<RegisterResponse>> RegisterAsync(RegisterRequest request)
        {
            var userWithSameUserName = await _account.Where(a => a.Username.Equals(request.Username)).FirstOrDefaultAsync();
            if (userWithSameUserName != null)
            {
                throw new ApiException($"Username '{request.Username}' is already taken.");
            }

            var userWithSameEmail = await _account.Where(a => a.Email.Equals(request.Email)).FirstOrDefaultAsync();
            if (userWithSameEmail != null)
            {
                throw new ApiException($"Email {request.Email } is already registered.");
            }

            request.Password = HashPassword(request.Password);
            var accountCreate = _mapper.Map<Account>(request);
            await _account.AddAsync(accountCreate);
            await _dbContext.SaveChangesAsync();
            var registerResponse = _mapper.Map<RegisterResponse>(accountCreate);
            registerResponse.JWToken = new JwtSecurityTokenHandler().WriteToken(GenerateJWToken(accountCreate));

            return new Response<RegisterResponse>(registerResponse);
        }

        public async Task<Response<AuthenticateResponse>> AuthenticateAsync(AuthenticateRequest request)
        {
            var user = await _account.Where(a => request.Username.Equals(a.Username)).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new ApiException($"Wrong Username or Password.");
            }

            var pwdHash = HashPassword(request.Password);
            if (!user.Password.Equals(pwdHash))
            {
                throw new ApiException($"Wrong Username or Password.");
            }

            JwtSecurityToken jwtSecurityToken = GenerateJWToken(user);
            AuthenticateResponse response = _mapper.Map<AuthenticateResponse>(user);
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            response.Expiration = jwtSecurityToken.ValidTo;
            return new Response<AuthenticateResponse>(response, $"Authenticated {user.Username}");
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private JwtSecurityToken GenerateJWToken(Account account)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Create a List of Claims, Keep claims name short    
            var permClaims = new List<Claim>();
            permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            permClaims.Add(new Claim("userid", account.Id.ToString()));
            permClaims.Add(new Claim("username", account.Username));

            //Create Security Token object by giving required parameters    
            var token = new JwtSecurityToken(_jwtSettings.Issuer, //Issure    
                            _jwtSettings.Audience,  //Audience    
                            permClaims,
                            expires: DateTime.Now.AddMinutes(_jwtSettings.DurationInMinutes),
                            signingCredentials: credentials);
            return token;
        }

        private string RandomString()
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, 4)
              .Select(s => s[random.Next(4)]).ToArray());
        }

        public async Task ForgotPassword(ForgotPasswordRequest request, string origin)
        {           
            var userWithSameEmail = _account.Where(a => a.Email.Equals(request.Email)).FirstOrDefault();

            // always return ok response to prevent email enumeration
            if (userWithSameEmail == null) return;

            var code = RandomString();
            //var route = "account/reset-password/";
            //var _enpointUri = new Uri(string.Concat($"{origin}/", route));
            var emailRequest = new EmailRequest()
            {
                Body = $"You reset token is - {code}",
                To = request.Email,
                Subject = "Reset Password",
            };
            await _emailService.SendAsync(emailRequest);
        }

        public Task<Response<string>> ResetPassword(ResetPasswordRequest model)
        {
            throw new NotImplementedException();
        }

        //private bool CheckResetPassword(string email, string token, string password)
        //{

        //}

        //public async Task<Response<string>> ResetPassword(ResetPasswordRequest request)
        //{
        //    var userWithSameEmail = await _account.Where(a => a.Email.Equals(request.Email)).FirstOrDefaultAsync();
        //    if (userWithSameEmail == null) throw new ApiException($"No Accounts Registered with {request.Email}.");
        //    //var result = await _userManager.ResetPasswordAsync(account, model.Token, model.Password);
        //    //if (result.Succeeded)
        //    //{
        //    //    return new Response<string>(model.Email, message: $"Password Resetted.");
        //    //}
        //    //else
        //    //{
        //    //    throw new ApiException($"Error occured while reseting the password.");
        //    //}



        //    request.Password = HashPassword(request.Password);
        //    var accountCreate = _mapper.Map<Account>(request);
        //    await _account.AddAsync(accountCreate);
        //    await _dbContext.SaveChangesAsync();
        //    var registerResponse = _mapper.Map<RegisterResponse>(accountCreate);
        //    registerResponse.JWToken = new JwtSecurityTokenHandler().WriteToken(GenerateJWToken(accountCreate));

        //    return new Response<RegisterResponse>(registerResponse);
        //}*/

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly JWTSettings _jwtSettings;
        private readonly IDateTimeService _dateTimeService;
        private readonly IUserRepositoryAsync _userRepository;
        //private readonly 
        public AccountService(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JWTSettings> jwtSettings,
            IDateTimeService dateTimeService,
            SignInManager<ApplicationUser> signInManager,
            IEmailService emailService,
            IUserRepositoryAsync userRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
            _dateTimeService = dateTimeService;
            _signInManager = signInManager;
            this._emailService = emailService;
            this._userRepository = userRepository;
        }

        public async Task<Response<AuthenticateResponse>> AuthenticateAsync(AuthenticateRequest request, string ipAddress)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new ApiException($"Wrong username or password");
            }
            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
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
            var refreshToken = GenerateRefreshToken(ipAddress);
            response.RefreshToken = refreshToken.Token;
            return new Response<AuthenticateResponse>(response, $"Authenticated {user.UserName}");
        }

        public async Task<Response<string>> RegisterAsync(RegisterRequest request, string origin)
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
                    PhoneNumber = request.PhoneNumber,
                    Address = request.Address
                };
                _ = _userRepository.AddAsync(userCreate);

                await _userManager.AddToRoleAsync(user, Roles.Basic.ToString());
                var verificationUri = await SendVerificationEmail(user, origin);
                //TODO: Attach Email Service here and configure it via appsettings
                await _emailService.SendAsync(new EmailRequest() {To = user.Email, Body = $"Please confirm your account by visiting this URL {verificationUri}", Subject = "Confirm Registration" });
                return new Response<string>(user.Id, message: $"User Registered. Please confirm your account by visiting this URL {verificationUri}");
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

        private async Task<string> SendVerificationEmail(ApplicationUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "Identity/confirm-email/";
            var _enpointUri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
            //Email Service Call Here
            return verificationUri;
        }

        public async Task<Response<string>> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return new Response<string>(user.Id, message: $"Account Confirmed for {user.Email}. You can now use the /Identity/authenticate endpoint.");
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

        public async Task ForgotPassword(ForgotPasswordRequest model, string origin)
        {
            var account = await _userManager.FindByEmailAsync(model.Email);

            // always return ok response to prevent email enumeration
            if (account == null) return;

            var code = await _userManager.GeneratePasswordResetTokenAsync(account);
            var route = "Identity/reset-password/";
            var _enpointUri = new Uri($"{origin}/{route}&code={code}");
            var emailRequest = new EmailRequest()
            {
                Body = $"Email response for your reset password request. Please visit this URL to reset password ${_enpointUri} or provide this code: {code} for your mobile application.",
                To = model.Email,
                Subject = "Reset Password",
            };
            await _emailService.SendAsync(emailRequest);
        }

        public async Task<Response<string>> ResetPassword(ResetPasswordRequest model)
        {
            var account = await _userManager.FindByEmailAsync(model.Email);
            if (account == null) throw new ApiException($"No Accounts Registered with {model.Email}.");
            var result = await _userManager.ResetPasswordAsync(account, model.Token, model.Password);
            if (result.Succeeded)
            {
                return new Response<string>(model.Email, message: $"Password Resetted.");
            }
            else
            {
                throw new ApiException($"Error occured while reseting the password.");
            }
        }
    }
}

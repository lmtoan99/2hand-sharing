using Application.DTOs.Account;
using Application.Exceptions;
using Application.Interfaces.Contexts;
using Application.Interfaces.Service;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using Domain.Settings;
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
        private readonly IApplicationDbContext _dbContext;
        private readonly DbSet<Account> _account;
        private readonly IMapper _mapper;
        private readonly JWTSettings _jwtSettings;

        public AccountService(IApplicationDbContext dbContext, IMapper mapper, IOptions<JWTSettings> jwtSettings)
        {
            this._dbContext = dbContext;
            _account = dbContext.Accounts;
            _mapper = mapper;
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
    }
}

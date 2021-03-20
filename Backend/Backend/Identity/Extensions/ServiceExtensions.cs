using Application.Interfaces.Service;
using Identity.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Domain.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Application.Wrappers;
using Application.Interfaces.Repositories;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Identity.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region Services
            services.AddTransient<IAccountService, AccountService>();
            #endregion

            services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));
            #region Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["JWTSettings:Issuer"],
                        ValidAudience = configuration["JWTSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = c =>
                        {
                            c.NoResult();
                            c.Response.StatusCode = 500;
                            c.Response.ContentType = "text/plain";
                            return c.Response.WriteAsync(c.Exception.ToString());
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(new Response<string>("You are not Authorized"));
                            return context.Response.WriteAsync(result);
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = 403;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(new Response<string>("You are not authorized to access this resource"));
                            return context.Response.WriteAsync(result);
                        },
                    };
                });
            #endregion
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Domain.Repositories;
using API.Infrastructure.Context;
using API.Infrastructure.Repositories;
using API.Services;
using API.Services.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using API.Services.Mappers;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IAdminService, AdminService>();
        }
        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration conf)
        {
            string key = conf.GetSection("JWT").GetValue<string>("Key") ?? "xxx123";

            services.AddAuthentication(option => {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option => {
                option.TokenValidationParameters = new TokenValidationParameters{
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });

            services.AddAuthorization();
        }

        public static void ConfigureRepositories(this IServiceCollection services)
            => services.AddScoped<IRepositoryManager, RepositoryManager>();

        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(VehicleProfile));
            services.AddAutoMapper(typeof(AdminProfile));
        }

        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration config)
            => services.AddDbContext<DataContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
    }
} 
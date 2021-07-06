using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using v0528.Data;
using v0528.Helpers;
using v0528.Interface;
using v0528.Services;

namespace v0528.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config) 
        {
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<LogUserActivity>();
            services.AddScoped<ILikesRespository, LikesRepostiory>();
            services.AddScoped<IMessageRepository, MessageRepository>();


            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            var connectString = config.GetConnectionString("DbCon");
            services.AddDbContext<DataContext>(options => options.UseSqlServer(connectString));
            return services;
        }
    }
}

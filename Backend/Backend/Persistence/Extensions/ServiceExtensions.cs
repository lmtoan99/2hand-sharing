using Application.Interfaces;
using Application.Interfaces.Contexts;
using Application.Interfaces.Repositories;
using Domain.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using Persistence.Repositories;
using Persistence.Repositories.EntityRepositories;

namespace Persistence.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseLazyLoadingProxies().UseMySql(
                   configuration.GetConnectionString("DefaultConnection"),
                   b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            #region Repositories
            services.AddScoped(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddScoped<IUserRepositoryAsync, UserRepositoryAsync>();
            services.AddScoped<ICategoryRepositoryAsync, CategoryRepositoryAsync>();
            services.AddScoped<IItemRepositoryAsync, ItemRepositoryAsync>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IItemImageRelationshipRepositoryAsync, ItemImageRelationshipRepositoryAsync>();
            services.AddScoped<IAddressRepositoryAsync, AddressRepositoryAsync>();
            services.AddScoped<IReceiveItemInformationRepositoryAsync, ReceveItemInformationRepositoryAsync>();
            services.AddScoped<IGroupRepositoryAsync, GroupRepositoryAsync>();
            services.AddScoped<IGroupAdminDetailRepositoryAsync, GroupAdminDetailRepositoryAsync>();
            services.AddScoped<IMessageRepositoryAsync, MessageRepositoryAsync>();
            services.AddScoped<IFirebaseTokenRepositoryAsync, FirebaseTokenRepositoryAsync>();
           services.AddScoped<IGroupMemberDetailRepositoryAsync, GroupMemberDetailRepositoryAsync>();

            services.AddScoped<INotificationRepositoryAsync, NotificationRepositoryAsync>();
            #endregion

            #region ApplicationContext
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            #endregion
        }
    }
}

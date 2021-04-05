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
            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddTransient<IUserRepositoryAsync, UserRepositoryAsync>();
            services.AddTransient<ICategoryRepositoryAsync, CategoryRepositoryAsync>();
            services.AddTransient<IItemRepositoryAsync, ItemRepositoryAsync>();
            services.AddTransient<IImageRepository, ImageRepository>();
            services.AddTransient<IItemImageRelationshipRepositoryAsync, ItemImageRelationshipRepositoryAsync>();
            #endregion

            #region ApplicationContext
            services.AddTransient<IApplicationDbContext, ApplicationDbContext>();
            #endregion
        }
    }
}

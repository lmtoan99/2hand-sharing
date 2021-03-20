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
            services.AddTransient<IAccountRepositoryAsync, AccountRepositoryAsync>();
            services.AddTransient<ICategoryRepositoryAsync, CategoryRepositoryAsync>();
            #endregion

            #region Repositories
            services.AddTransient<IApplicationDbContext, ApplicationDbContext>();
            #endregion
        }
    }
}

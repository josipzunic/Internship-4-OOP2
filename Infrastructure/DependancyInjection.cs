using Domain.Persistence.Common;
using Domain.Persistence.Companies;
using Domain.Persistence.User;
using Infrastructure.Common;
using Infrastructure.Database.Configurations;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Company;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

namespace Infrastructure;


public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddDbContexts(services, configuration);
            AddRepositories(services);
            AddUnitOfWorks(services);
            AddCustomServices(services, configuration);

            return services;
        }

        private static void AddDbContexts(IServiceCollection services, IConfiguration configuration)
        {
            var usersConn = configuration.GetConnectionString("usersDatabase");
            if (string.IsNullOrEmpty(usersConn))
                throw new ArgumentNullException(nameof(usersConn));

            var companiesConn = configuration.GetConnectionString("companiesDatabase");
            if (string.IsNullOrEmpty(companiesConn))
                throw new ArgumentNullException(nameof(companiesConn));
            
            services.AddDbContext<UsersDbContext>(options =>
                options.UseNpgsql(usersConn));

            services.AddDbContext<CompaniesDbContext>(options =>
                options.UseNpgsql(companiesConn));
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
        }

        private static void AddUnitOfWorks(IServiceCollection services)
        {
            services.AddScoped<IUserUnitOfWork, UserUnitOfWork>();
            services.AddScoped<ICompanyUnitOfWork, CompanyUnitOfWork>();
            services.AddScoped<IUnitOfWork, UnitOfWork>(); 
        }

        private static void AddCustomServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDapperManager>(_ =>
                new DapperManager(configuration.GetConnectionString("usersDatabase")));

            services.AddMemoryCache();
            services.AddScoped<ICacheServices, CacheService>();
            services.AddHttpClient<IRetrieveExternalUsers, RetrieveExternalUsers>();
        }
    }
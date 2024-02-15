using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Persistence.Context;
using Persistence.DbConfigs;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.Reflection;

namespace Persistence
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            // Connectionstring
            var connectionString = configuration.GetSection(nameof(DbConfig)).Get<DbConfig>()?.ConnectionString;
            var serverVersion = new MySqlServerVersion(ServerVersion.AutoDetect(connectionString));

            // TODO: Unit of work

            services.AddDbContext<ApplicationDbContext>(
               dbContextOptions => dbContextOptions
                    .UseMySql(connectionString, serverVersion, mySqlOptions =>
                    {
                        mySqlOptions.SchemaBehavior(MySqlSchemaBehavior.Ignore);
                    })
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableDetailedErrors())
                .AddTransient<ApplicationDbSeeder>();
                
            return services;
        }

        public static void AddPersistenceDependencies(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using SD_Server.Application;
using SD_Server.Domain.Base;
using SD_Server.Infra.Data;
using SD_Server.Infra.Data.Context;
using SD_Server.Infra.Data.DbMigrator;
using SD_Server.Infra.Data.UnitOfWork;
using System.Reflection;

namespace SD_Api_Extensions
{
    /// <summary>
    /// Extensões para registrar serviços na aplicação.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registra o DbContext
        /// e os repositórios (via reflexão) usando a connection string lida de AppSettings.
        /// </summary>
        /// <param name="services">O container de DI.</param>
        /// <exception cref="Exception">Lançada se a seção AppSettings não estiver configurada.</exception>
        public static void AddDbServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionStringSql = Environment.GetEnvironmentVariable("Connection_Sql")
                ?? configuration["AppSettings:ConnectionString"]
                ?? configuration.GetConnectionString("SqlServer");

            if (string.IsNullOrEmpty(connectionStringSql))
                throw new InvalidOperationException("Connection String não definida");

            services.AddDbContext<SdServerDbContext>(options =>
            {
                options.UseSqlServer(connectionStringSql, sqlOptions =>
                {
                    sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "dbo");
                });
            });

            services.AddScoped<DatabaseMigrator>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            AddRepositories(services);
        }


        /// <summary>
        /// Registra o AutoMapper escaneando o assembly de Application
        /// e o assembly do tipo <typeparamref name="TAssembly"/> informado pelo chamador.
        /// </summary>
        /// <typeparam name="TAssembly">Tipo usado como referência para o assembly da API chamadora.</typeparam>
        /// <param name="services">O container de DI.</param>
        public static void AddMapper<TAssembly>(this IServiceCollection services)
        {
            var assemblies = new List<Assembly>();

            var applicationAssembly = Assembly.GetAssembly(typeof(ApplicationModule));
            if (applicationAssembly != null)
                assemblies.Add(applicationAssembly);

            var callerAssembly = Assembly.GetAssembly(typeof(TAssembly));
            if (callerAssembly != null)
                assemblies.Add(callerAssembly);

            services.AddAutoMapper(cfg => cfg.ShouldMapMethod = _ => false, assemblies);
        }

        public static void AddStaticFiles(this IServiceCollection services)
        {
            services.Configure<StaticFileOptions>(options =>
            {
                options.FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "avatars"));
                options.RequestPath = "/avatars";
            });
        }

        /// <summary>
        /// Registra os repositórios automaticamente, escaneando o assembly que contém o marker InfraDataModule.
        /// </summary>
        /// <param name="services">O container de DI.</param>
        private static void AddRepositories(IServiceCollection services)
        {
            var repositoryAssembly = typeof(InfraDataModule).Assembly;
            var registrations =
                from type in repositoryAssembly.GetExportedTypes()
                where type.Name.EndsWith("Repository")
                from service in type.GetInterfaces()
                select new { service, implementation = type };

            foreach (var reg in registrations)
                services.AddTransient(reg.service, reg.implementation);
        }
    }
}

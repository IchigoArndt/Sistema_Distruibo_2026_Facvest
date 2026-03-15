using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using SD_Server.Application;
using System.Reflection;

namespace SD_Api_Extensions
{
    /// <summary>
    /// Extensões para registrar serviços na aplicação.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
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
    }
}

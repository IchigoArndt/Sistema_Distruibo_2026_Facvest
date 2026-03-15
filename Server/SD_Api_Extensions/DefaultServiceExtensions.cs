using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace SD_Api_Extensions
{
    public static class DefaultServiceExtensions
    {
        /// <summary>
        /// Registra os serviços padrão utilizados pelas APIs.
        /// </summary>
        /// <typeparam name="TAssembly">
        /// Tipo usado como referência para escanear os assemblies, especialmente para a configuração do AutoMapper.
        /// </typeparam>
        /// <param name="services">
        /// Coleção de serviços utilizada para a injeção de dependências.
        /// </param>
        /// <param name="container">
        /// Container SimpleInjector que gerencia a resolução das dependências.
        /// </param>
        public static void AddDefaultServices<TAssembly>(this IServiceCollection services, Container container)
        {
            //services.AddDbServices();
            services.AddMapper<TAssembly>();
            services.AddSimpleInjector(container, isWebApp: true);
            services.AddMediator(container);
            services.AddValidators(container);
        }
    }
}

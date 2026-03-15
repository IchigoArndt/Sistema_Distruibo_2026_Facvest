using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace SD_Api_Extensions
{
    /// <summary>
    /// Fornece métodos de extensão para integrar o container do SimpleInjector com a coleção de serviços padrão do ASP.NET Core.
    /// Essa integração pode ser configurada para suportar funcionalidades de aplicações web, como a ativação de controllers.
    /// </summary>
    public static class SimpleInjectorExtensions
    {
        /// <summary>
        /// Adiciona e configura o SimpleInjector na coleção de serviços <see cref="IServiceCollection"/>,
        /// integrando-o com o ASP.NET Core e, opcionalmente, registrando os componentes necessários para aplicações web.
        /// </summary>
        /// <param name="services">A coleção de serviços do .NET na qual o SimpleInjector será integrado.</param>
        /// <param name="container">A instância do <see cref="Container"/> do SimpleInjector que gerenciará as dependências.</param>
        /// <param name="isWebApp">
        /// Indica se a aplicação é uma aplicação web. Se <c>true</c>, registra o MVC Core e a ativação de controllers.
        /// </param>
        public static void AddSimpleInjector(this IServiceCollection services, Container container, bool isWebApp = false)
        {
            if (isWebApp)
                services.AddMvcCore();

            services.AddSimpleInjector(container, options =>
            {
                if (isWebApp)
                    options.AddAspNetCore().AddControllerActivation();
            });
        }
    }
}

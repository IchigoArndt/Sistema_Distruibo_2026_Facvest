

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SD_Api_Extensions
{
    /// <summary>
    /// Lê a seção especificada e a converte em uma instância de <typeparamref name="T"/>.
    /// Opcionalmente registra a configuração no container de DI, se <paramref name="service"/> for fornecido.
    /// </summary>
    /// <typeparam name="T">Tipo de configuração a ser carregada.</typeparam>
    /// <param name="configuration">Instância de IConfiguration que contém a seção.</param>
    /// <param name="sectionName">Nome da seção a ser lida.</param>
    /// <param name="service">Opcional: IServiceCollection para registrar as configurações via IOptions.</param>
    /// <returns>Uma instância de <typeparamref name="T"/> preenchida com os valores da seção.</returns>
    /// <exception cref="InvalidOperationException">Lançada se a seção de configuração estiver ausente ou inválida.</exception>
    public static class SettingsExtensions
    {
        public static T LoadSettings<T>(this IConfiguration configuration, string sectionName, IServiceCollection? service = null)
            where T : class
        {
            var section = configuration.GetSection(sectionName);
            service?.Configure<T>(section);

            var settings = section.Get<T>() ?? throw new InvalidOperationException($"A seção de configuração '{sectionName}' não foi encontrada ou está inválida.");

            return settings;
        }
    }
}

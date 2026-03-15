using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace SD_Server.Infra.Configurations
{
    public class EnviromentConfiguration
    {
        private static IConfiguration _configuration;

        /// <summary>
        /// Configura o provider de configuração que será utilizado pelas propriedades estáticas desta classe.
        /// </summary>
        /// <param name="configuration">Instância de <see cref="IConfiguration"/> que contém as configurações da aplicação.</param>
        public static void Configure(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SD_Api_Extensions.Settings
{
    /// <summary>
    /// Representa as configurações gerais da aplicação, incluindo a connection string.
    /// </summary>
    public class AppSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
    }
}

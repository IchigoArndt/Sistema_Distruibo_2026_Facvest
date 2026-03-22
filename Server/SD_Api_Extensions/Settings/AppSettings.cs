namespace SD_Api_Extensions.Settings
{
    /// <summary>
    /// Representa as configurações gerais da aplicação, incluindo a connection string.
    /// </summary>
    public class AppSettings
    {
        /// <summary>Connection string do SQL Server (espelha o padrão da Matrix: AppSettings + env Connection_Sql).</summary>
        public string ConnectionString { get; set; } = string.Empty;

        /// <summary>URL de conexão do MongoDB (logs e demais usos; espelha env Connection_Mongo).</summary>
        public string MongoConnectionString { get; set; } = string.Empty;
    }
}

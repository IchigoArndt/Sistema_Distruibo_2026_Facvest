using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace SD_Server.Infra.Data.Context
{
    /// <summary>
    /// Permite que ferramentas do EF Core (Add-Migration, dotnet ef) criem o <see cref="SdServerDbContext"/>
    /// sem depender do host nem da variável <c>Connection_Sql</c>, lendo <c>appsettings.json</c> quando necessário.
    /// </summary>
    public sealed class SdServerDbContextFactory : IDesignTimeDbContextFactory<SdServerDbContext>
    {
        public SdServerDbContext CreateDbContext(string[] args)
        {
            var connectionString = ResolveConnectionString();

            var optionsBuilder = new DbContextOptionsBuilder<SdServerDbContext>();
            optionsBuilder.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "dbo");
            });

            return new SdServerDbContext(optionsBuilder.Options);
        }

        private static string ResolveConnectionString([System.Runtime.CompilerServices.CallerFilePath] string callerFile = "")
        {
            var env = Environment.GetEnvironmentVariable("Connection_Sql");
            if (!string.IsNullOrWhiteSpace(env))
                return env;

            // Diretórios candidatos: working-dir, base do assembly (bin/...) e diretório do arquivo-fonte desta factory
            var candidateRoots = new[]
            {
                Directory.GetCurrentDirectory(),
                AppContext.BaseDirectory,
                Path.GetDirectoryName(callerFile) ?? ""
            };

            foreach (var root in candidateRoots.Where(r => !string.IsNullOrEmpty(r)))
            {
                for (var dir = new DirectoryInfo(root); dir != null; dir = dir.Parent)
                {
                    var appsettingsPath = Path.Combine(dir.FullName, "appsettings.json");
                    if (!File.Exists(appsettingsPath))
                        continue;

                    var config = new ConfigurationBuilder()
                        .SetBasePath(dir.FullName)
                        .AddJsonFile("appsettings.json", optional: false)
                        .AddJsonFile("appsettings.Development.json", optional: true)
                        .Build();

                    var cs = config["AppSettings:ConnectionString"] ?? config.GetConnectionString("SqlServer");
                    if (!string.IsNullOrWhiteSpace(cs))
                        return cs;
                }
            }

            throw new InvalidOperationException(
                "Não foi possível obter a connection string em design-time. Defina Connection_Sql ou AppSettings:ConnectionString / ConnectionStrings:SqlServer em appsettings.json.");
        }
    }
}

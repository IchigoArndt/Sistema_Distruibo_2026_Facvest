using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SD_Server.Domain.Features.Students;

namespace SD_Server.Infra.Data.Context
{
    /// <summary>
    /// DbContext principal do sistema, responsável por mapear as entidades
    /// e aplicar as configurações via Fluent API.
    /// As opções de conexão são injetadas via DI.
    /// </summary>
    /// <remarks>
    /// Construtor que recebe as opções do DbContext e ILoggerFactory.
    /// </remarks>
    /// <param name="options">As opções de configuração do DbContext.</param>
    /// <param name="loggerFactory">Um logger factory para registrar as operações do EF Core.</param>
    public class SdServerDbContext(DbContextOptions<SdServerDbContext> options, ILoggerFactory? loggerFactory = null) : DbContext(options)
    {
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SdServerDbContext).Assembly);
        }
    }
}

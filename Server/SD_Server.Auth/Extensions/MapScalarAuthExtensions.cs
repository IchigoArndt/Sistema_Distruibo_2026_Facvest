using Scalar.AspNetCore;

namespace SD_Server.Auth.Extensions
{
    public static class MapScalarAuthExtensions
    {
        public static void MapScalar(WebApplication app)
        {
            app.MapScalarApiReference(opt =>
            {
                opt.Title = "Auth SD_Server";
                opt.Theme = ScalarTheme.DeepSpace;

                // Gera client de exemplo em C# usando HttpClient
                opt.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);

                // Adicionando todos os servidores de teste e o local para ficar disponível.
                opt.Servers = [
                    new ScalarServer("https://localhost:7003", "Local")
                ];
            });
        }
    }
}

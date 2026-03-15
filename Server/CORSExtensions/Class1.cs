using Microsoft.AspNetCore.Builder;

namespace CORSExtensions
{
    public static class CORSExtensions
    {
        public static void UseCORS(this IApplicationBuilder app)
        {
            var corsSettings = new CORSSettings().Default();

            app.UseCors(builder => builder
                                    .WithOrigins(corsSettings.Origins)
                                    .WithMethods(corsSettings.Methods)
                                    .WithHeaders(corsSettings.Headers)
                                    .Build());
        }
    }
}

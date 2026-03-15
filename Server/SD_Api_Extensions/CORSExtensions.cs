using Microsoft.AspNetCore.Builder;
using SD_Api_Extensions.Settings;

namespace SD_Api_Extensions
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

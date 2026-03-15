using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SD_Server.Application;
using SimpleInjector;

namespace SD_Api_Extensions
{
    public static class FluentValidationExtensions
    {
        public static void AddValidators(this IServiceCollection services, Container container)
        {
            services.AddValidatorsFromAssembly(typeof(ApplicationModule).Assembly);

            var entryAssembly = System.Reflection.Assembly.GetEntryAssembly();
            if (entryAssembly != null)
                services.AddValidatorsFromAssembly(entryAssembly);
        }
    }
}

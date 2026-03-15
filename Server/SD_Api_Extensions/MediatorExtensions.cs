using Microsoft.Extensions.DependencyInjection;
using SD_Api_Base.Behavious;
using SD_Server.Application;
using SimpleInjector;
using System.Reflection;

namespace SD_Api_Extensions
{
    public static class MediatorExtensions
    {
        public static void AddMediator(this IServiceCollection services, Container container)
        {
            var assemblies = new List<Assembly> { typeof(ApplicationModule).Assembly };

            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
                assemblies.Add(entryAssembly);

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies([.. assemblies]);
                cfg.AddOpenBehavior(typeof(ValidationPipeline<,>));
            });
        }
    }
}

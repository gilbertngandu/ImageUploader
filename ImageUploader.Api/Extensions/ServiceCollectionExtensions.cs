using ImageUpload.Api.Registration;
using System.Reflection;

namespace ImageUpload.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterServicesByConvention(this IServiceCollection services, Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                                .Where(t => t.IsClass && !t.IsAbstract);

            foreach (var type in types)
            {
                var interfaceType = type.GetInterface($"I{type.Name}");
                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, type);
                }
            }
        }

        public static IServiceCollection RegisterDependencies(this IServiceCollection services, IConfigurationRoot config)
        { 
            DiRegistration.RegisterDependencies(services, config); 
            return services;
        }
    }
}

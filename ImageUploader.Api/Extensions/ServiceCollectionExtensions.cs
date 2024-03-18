using System.Reflection;

namespace ImageUpload.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterServicesByConvention(this IServiceCollection services, Assembly assembly)
        {
            //Ici j'ai éssayé d'enregistrer les interfaces automatiquement 
            //Si IImage exist ca peut etre resolu en Image par example
            //Mais ca marche pas bien

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
    }
}

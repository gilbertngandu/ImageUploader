using Microsoft.Extensions.DependencyInjection;

namespace ImageUploader.Common.Models
{
    public interface IRegistration
    {
        public void RegisterDependencies(IServiceCollection services);
    }
}

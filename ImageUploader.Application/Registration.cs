using ImageUploader.Application.Services;
using ImageUploader.Common.Models;
using ImageUploader.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ImageUploader.Application
{
    public class Registration: IRegistration
    {
        public void RegisterDependencies(IServiceCollection services)
        {
            services.AddTransient<IImageService, ImageService>();
            services.AddSingleton<IImageValidatorService, ImageValidatorService>();
            services.AddTransient<IImageRecordRepo, ImageRecordRepo>();
            services.AddTransient<IImageUploadService, ImageUploadService>();
            services.AddTransient<ICaptionService, CaptionService>();
        }
    }
}

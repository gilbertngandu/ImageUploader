using AutoMapper;
using CloudinaryDotNet;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Vision.V1;
using ImageUploader.Application.Plugins.GoogleVision;
using ImageUploader.Common.Extension;
using ImageUploader.Common.Models;
using ImageUploader.Domain.Models;
using ImageUploader.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;
using OpenAI_API;

namespace ImageUpload.Api.Registration
{
    public class DiRegistration
    {
        public static List<IRegistration> Discover()
        {
            return new List<IRegistration>
            {
                new ImageUploader.Application.Registration(),
            };
        }
        public static void RegisterDependencies(IServiceCollection services, IConfigurationRoot config)
        {
            var connectionString = config.GetConnectionString("CosmosDbConnectionString");
            var databaseName = config["CosmosDbName"];
            var openAIKey = config["OpenAIKey"];
            var googleCredentialPath = config["GoogleCredentialPath"];
            var cloudinaryUrl = config["CloudinaryUrl"];

            var registrations = Discover();
            
            foreach (var registration in registrations)
            {
                registration.RegisterDependencies(services);
            }

            services.AddSingleton<IOpenAIAPI>((p) => new OpenAIAPI(openAIKey));
            services.AddSingleton<ICloudinary>((c) => new Cloudinary(cloudinaryUrl));
            services.AddSingleton( _ => 
                new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper()
            );

            services.AddSingleton( _ =>
            {
                //How to get the google credentials https://developers.google.com/workspace/guides/create-credentials
                var credential = GoogleCredential.FromJson(
                    config
                    .GetSection("GoogleCredentials")
                    .AsJsonString());

                var imageAnnotationBuilder = new ImageAnnotatorClientBuilder
                {
                    GoogleCredential = credential
                };
                return imageAnnotationBuilder.Build();
            });

            services.AddSingleton<ICaptionExtractor, GoogleVisionCaptionExtractor>();

            services.AddDbContext<ImageUploaderDbContext>(
                dbContextOptions => dbContextOptions
                    .UseCosmos(connectionString, databaseName)
                    .LogTo(Console.WriteLine, LogLevel.Debug)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
            );
        }
    }
}

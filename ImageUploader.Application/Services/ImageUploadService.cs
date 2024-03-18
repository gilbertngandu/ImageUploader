using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ImageUploader.Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ImageUploader.Application.Services
{
    public class ImageUploadService : IImageUploadService
    {
        private readonly ICloudinary cloudinaryClient;
        //Ceci est juste pour debug plus rapidement mais ne devrais pas etre la
            
        public ImageUploadService(ICloudinary cloudinaryClient)
        {
            this.cloudinaryClient = cloudinaryClient;  
        }

        public async Task<string> UploadImage(IFormFile imageFormFile)
        {
            //J'ai eu des difficultés avec imageFormFile stream quand le systeme Upload et génére les
            //etiquettes en meme temps du au faite que ca utilise le meme stream
            //TODO Utilisé lock ou semaphore 
            using Stream imageStream = imageFormFile.OpenReadStream();

            var imageName = imageFormFile.FileName;

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(imageName, imageStream),
                PublicId = imageName,
            };

            //TODO On pourrait aussi utiliser Polly Retry 
            var result = await cloudinaryClient.UploadAsync(uploadParams);

            if (result.Error != null)
                throw new Exception($"Upload service failed: {result.Error.Message}");

            return result.Url.ToString();
        }
    }

    public interface IImageUploadService
    {
        public Task<string> UploadImage(IFormFile imageFormFile);
    }
}

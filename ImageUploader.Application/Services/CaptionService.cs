using Google.Cloud.Vision.V1;
using ImageUploader.Common.Models;
using Microsoft.AspNetCore.Http;

namespace ImageUploader.Application.Services
{
    public class CaptionService : ICaptionService
    {
        //TODO: Ce service est dependend des classes de Google
        //Créer des interfaces ou un factory pour enlever cette dependence
        private readonly ICaptionExtractor captionExtractor;

        public CaptionService(ICaptionExtractor captionExtractor)
        {
            this.captionExtractor = captionExtractor;
        }

        public async Task<string> GetCaption(IFormFile imageFormFile)
        {
            try
            {
                return await this.captionExtractor.ExtractCaption(imageFormFile);
                

            }
            catch(Exception ex)
            {
                throw new Exception($"Caption service failed: {ex.Message}");
            }
        }
    }

    public interface ICaptionService
    {
        Task<string> GetCaption(IFormFile imageFormFile);
    }
}

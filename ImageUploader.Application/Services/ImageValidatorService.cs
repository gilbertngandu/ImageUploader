using Microsoft.AspNetCore.Http;

namespace ImageUploader.Application.Services
{
    public class ImageValidatorService: IImageValidatorService
    {
        private const int DefaultMaxSize = 4 * 1024 * 1024;
        private int maxFileSize;
        public ImageValidatorService(int maxSize = DefaultMaxSize)
        {
            maxFileSize = maxSize;
        }
        public async Task<bool> ValidateImage(IFormFile imageFormFile)
        {
            if (imageFormFile == null || imageFormFile.Length == 0 || imageFormFile.Length > maxFileSize)
            {
                return false;
            }

            //TODO Peut etre amélioré avec d'autre business rule 
            if(!imageFormFile.ContentType.Contains("image")) return false;

            return true;
        }
    }

    public interface IImageValidatorService
    {
        Task<bool> ValidateImage(IFormFile formfile);
    }
}

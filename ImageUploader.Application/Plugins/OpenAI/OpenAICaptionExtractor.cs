using ImageUploader.Common.Models;
using Microsoft.AspNetCore.Http;

namespace ImageUploader.Application.Plugins.OpenAI
{
    public class OpenAICaptionExtractor : ICaptionExtractor
    {
        public Task<string> ExtractCaption(IFormFile imageFormFile)
        {
            throw new NotImplementedException();
        }
    }
}

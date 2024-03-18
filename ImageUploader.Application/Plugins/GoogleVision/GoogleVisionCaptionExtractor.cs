using Google.Cloud.Vision.V1;
using ImageUploader.Common.Models;
using Microsoft.AspNetCore.Http;

namespace ImageUploader.Application.Plugins.GoogleVision
{
    public class GoogleVisionCaptionExtractor : ICaptionExtractor
    {
        private readonly ImageAnnotatorClient imageAnnotatorClient;

        public GoogleVisionCaptionExtractor(ImageAnnotatorClient imageAnnotatorClient)
        {
            this.imageAnnotatorClient = imageAnnotatorClient;
        }
        public async Task<string> ExtractCaption(IFormFile imageFormFile)
        {
            using (Stream imageStream = imageFormFile.OpenReadStream())
            {
                var image = Image.FromStream(imageStream);

                //DetectLabels est la fonction de AI de Google Vision
                //Ca permet de créer une liste d'etiquettes 

                //TODO Pour les API externes il faudrait peut etre utiliser aussi 
                //le package Polly pour réésayer en cas de problème de connection
                var labels = await imageAnnotatorClient.DetectLabelsAsync(image);

                if (!labels.Any()) return null;

                var captions = labels.Select(c => c.Description).ToList();

                return string.Join(",", captions); //Je combine just les etiquettes
            }
        }
    }
}

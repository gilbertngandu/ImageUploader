using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Google.Cloud.Vision.V1;
using ImageApplication.Domain.Models;
using ImageUploader.Application.Plugins.GoogleVision;
using ImageUploader.Application.Services;
using ImageUploader.Common.Models;
using ImageUploader.Infrastructure.Mapper;
using ImageUploader.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Moq;

namespace ImageUploader.Tests.ImageServiceTests
{
    public class ImageServiceTest
    {
        private Mock<IImageService> imageServiceMock;
        private Mock<IImageValidatorService> imageValidationMock;
        private Mock<IImageRecordRepo> imgRepoMock;
        private Mock<IImageUploadService> uploadServiceMock;
        private Mock<ICaptionService> captionServiceMock;
        private Mock<IMapper> mapperMock;
        private Mock<ICloudinary> cloudinaryMock;
        private Mock<IFormFile> formfileMock;
        private Mock<ImageUploadParams> imgParam;
        private Uri url;
        private Mock<ImageAnnotatorClient> imageAnnotatorClientMock;
        private Mock<ICaptionExtractor> captionExtractorMock;

        [SetUp]
        public void SetUp()
        {
            this.imageValidationMock = new Mock<IImageValidatorService>();
            this.imgRepoMock = new Mock<IImageRecordRepo>();
            this.uploadServiceMock = new Mock<IImageUploadService>();
            this.captionServiceMock = new Mock<ICaptionService>();
            this.mapperMock = new Mock<IMapper>();
            this.cloudinaryMock = new Mock<ICloudinary>();
            this.formfileMock = new Mock<IFormFile>();
            this.imgParam = new Mock<ImageUploadParams>();
            this.captionExtractorMock = new Mock<ICaptionExtractor>();
            this.url = new Uri("https://www.gilbertngandu.com/cloud_developer/image.png");

            //Pour mock le processur de OpenStream etc j'ai inclus un fichier 
            string imagePath = Path.Combine(TestContext.CurrentContext.TestDirectory,
                "Resources", "applesandbananassong.jpg");

            //Ici 
            formfileMock.Setup(s => s.FileName).Returns("gilbert.jpg");

            //la function Openstream() va ouvrir le fichier local
            formfileMock.Setup(s => s.OpenReadStream()).Returns(File.OpenRead(imagePath));
            

            //UploadAsync va etre Mock et retourner un ImageUploadResul avec le lien
            cloudinaryMock.Setup(c => c.UploadAsync(It.IsAny<ImageUploadParams>(), default))
                .ReturnsAsync(new ImageUploadResult
                {
                    Url = url
                });
            
            this.imageAnnotatorClientMock = new Mock<ImageAnnotatorClient>();
        }

        [Test]
        public async Task GetImageRecordDTOsTest()
        {
            //Ici je declare une liste qui represente les données de la BD
            var imgRepoSamples = new List<ImageRecord>()
            {
                new ImageRecord
                {
                    Id = Guid.NewGuid(),
                    Caption="Io Node, Camera, Camera Personnel",
                    CreatedDateTime = DateTime.Now,
                    CreatedBy = "Gilbert"
                }
            };
            
            //Je mock la reponse de GetAll a la liste ci haut
            imgRepoMock.Setup(s => s.GetAll()).ReturnsAsync(imgRepoSamples);

            //Ici j ai eu des difficulter a mocker IMapper donc j'ai just créer un IMapper
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = config.CreateMapper();

            //ImageService construit avec toutes ces dependences
            var imageService = new ImageService(imageValidationMock.Object, imgRepoMock.Object, 
                uploadServiceMock.Object, captionServiceMock.Object, mapper);

            var result = await imageService.GetImageRecordDTOs();

            //Je verifie juste que la function a le meme nombre d'objet
            Assert.That(imgRepoSamples.Count(), Is.EqualTo(result.Count()));

            //Je m'assure que la reponse est une liste de IImageDTO
            Assert.IsInstanceOf<IEnumerable<IImageRecordDTO>>(result);

        }

        [Test]
        public async Task UploadImageTest()
        {
            
            var uploadService = new ImageUploadService(cloudinaryMock.Object);

            //Je teste le service upload qui devrait retourner le meme url déclaré dans le Setup
            //ca re-utilise le me formfileMock
            var urlResult = await uploadService.UploadImage(formfileMock.Object);

            //Je m'assure que le url et le meme
            Assert.That(url.ToString(), Is.EqualTo(urlResult));
        }

        [Test]
        public async Task GoogleExtractorCaptionTest()
        {
            //Ici le resultat de generate caption est une liste mots séparé par une virgule
            //Donc, je créé ce que j'attend de la function
            var captions = "MvidiMukulu,apple,banana,gilbert,awesome";

            //Google Vision repond avec une liste d'annotation

            var labels = new List<EntityAnnotation>
            {
                new EntityAnnotation {Description =  "MvidiMukulu"},
                new EntityAnnotation {Description =  "apple"},
                new EntityAnnotation {Description =  "banana"},
                new EntityAnnotation {Description =  "gilbert"},
                new EntityAnnotation {Description =  "awesome"},
            };

            //Pour eviter que le test appelle Google, je mock DetectLabels pour
            imageAnnotatorClientMock.Setup(m =>
            m.DetectLabelsAsync(It.IsAny<Image>(), default, default, default))
                .ReturnsAsync(labels);

            var captionService = new CaptionService(
                new GoogleVisionCaptionExtractor(imageAnnotatorClientMock.Object));

            var captionResult = await captionService.GetCaption(formfileMock.Object);

            //A la fin je m'assure que la function GetCaption géner ce que j'attends
            Assert.That(captions.ToString(), Is.EqualTo(captionResult));
        }

        [Test]
        public async Task GenerateCaptionTest()
        {
            //Ici le resultat de generate caption est une liste mots séparé par une virgule
            //Donc, je créé ce que j'attend de la function
            var captions = "MvidiMukulu,apple,banana,gilbert,awesome";
            
            //Pour eviter que le test appelle Google, je mock DetectLabels pour
            captionExtractorMock.Setup(m =>
            m.ExtractCaption(It.IsAny<IFormFile>()))
                .ReturnsAsync(captions);

            var captionService = new CaptionService(captionExtractorMock.Object);
            var captionResult = await captionService.GetCaption(formfileMock.Object);

            //A la fin je m'assure que la function GetCaption géner ce que j'attends
            Assert.That(captions.ToString(), Is.EqualTo(captionResult));
        }
    }
}

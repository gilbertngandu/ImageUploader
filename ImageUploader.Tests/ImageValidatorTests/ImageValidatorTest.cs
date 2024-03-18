using ImageUploader.Application.Services;
using Microsoft.AspNetCore.Http;
using Moq;

namespace ImageUploader.Tests.ImageValidatorTests
{
    public class ImageValidatorTest
    {
        [Test]
        public async Task ImageValidator()
        {
            var size = 10;

            //Fichier jpeg
            var jpegformfileMock = new Mock<IFormFile>();
            jpegformfileMock.Setup(s => s.ContentType).Returns("image/jpeg");
            jpegformfileMock.Setup(s => s.Length).Returns(size);

            //Exe
            var exeformfileMock = new Mock<IFormFile>();
            exeformfileMock.Setup(s => s.ContentType).Returns("application/octet-stream");
            exeformfileMock.Setup(s => s.Length).Returns(size);

            //Fichier vide
            var emptyformfileMock = new Mock<IFormFile>();
            emptyformfileMock.Setup(s => s.Length).Returns(0);


            var imageValidatorService = new ImageValidatorService();
            //Je m'assure que le system supporte des fichiers jpg, rejette les fichiers exe et les 
            //fichiers vides
            //TODO tester plus de format

            Assert.That(await imageValidatorService.ValidateImage(jpegformfileMock.Object), Is.True);
            Assert.That(await imageValidatorService.ValidateImage(exeformfileMock.Object), Is.False);
            Assert.That(await imageValidatorService.ValidateImage(emptyformfileMock.Object), Is.False);
        }
    }
}

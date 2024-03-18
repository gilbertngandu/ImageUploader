using Asp.Versioning;
using ImageUploader.Application.Models;
using ImageUploader.Application.Services;
using ImageUploader.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using System.Net;

namespace ImageUpload.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    [ApiVersion("1")]
    public class ImageController : ControllerBase
    {
        //TODO Travailler sur la logic de Journalisation/Logging
        //Avoir un traceId au debut de la requete qui pourrait etre injecté au service
        //pour degoger facilement
        private readonly ILogger<ImageController> logger;
        private readonly IImageService imageService;
        private readonly IImageValidatorService imageValidatorService;

        //Le systeme inject des services internes. Il y'a imageService et validatorService
        //ImageService est le service principal qui utilise CaptionService et UploadService
        //J'aurais pu injecter tous ces services ici mais le controller doit etre legé
        //TODO: Créer des microservices independent qui communique par HTTP 
        public ImageController(ILogger<ImageController> logger, IImageService imageService,
            IImageValidatorService imageValidatorService)
        {
            this.logger = logger;
            this.imageService = imageService;
            this.imageValidatorService = imageValidatorService;
        }

        [HttpPost(Name = "Image")]
        [MapToApiVersion("1")] //Pas necessaire mais juste pour montrer comment supporter d'autre version
        //au niveau du endpoint
        //Ca c'est pour etre à mesure de televerser un fichier avec swagger
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<IApiResponse<IImageRecordDTO>>> Post(IFormFile image)
        {
            try
            {
                if (!await imageValidatorService.ValidateImage(image))
                    throw new HttpException("Invalid image", HttpStatusCode.BadRequest);

                var result = await imageService.UploadAndSave(image);

                return SuccessResponse(result);
            }
            catch (Exception ex)
            {
                var errorResult = ErrorResponse<IImgUploadResult>(ex);
                return StatusCode((int)errorResult.StatusCode, errorResult);
            }
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        [MapToApiVersion("1")]
        public async Task<ActionResult<IApiResponse<IEnumerable<IImageRecordDTO>>>> Get()
        {
            return SuccessResponse(await imageService.GetImageRecordDTOs());
        }

        private IApiResponse<T> ErrorResponse<T>(Exception ex)
        {
            if (ex is HttpException exception) //J'ai créé une exception avec la possibilité de passer un HttpErrorCode
            {
                return new ApiResponse<T>(exception.Message, exception.HttpStatusCode);
            }

            return new ApiResponse<T>(ex.Message, HttpStatusCode.InternalServerError);
        }

        private ActionResult<IApiResponse<T>> SuccessResponse<T>(T result)
        {
            //Wrapper pour toutes les réponses
            return new ApiResponse<T>(result);
        }
    }

}
using AutoMapper;
using ImageUploader.Application.Models;
using ImageUploader.Common.Models;
using ImageUploader.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace ImageUploader.Application.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageValidatorService imageValidationService;
        private readonly IImageRecordRepo imgRepo;
        private readonly IImageUploadService uploadService;
        private readonly IMapper mapper;
        private readonly ICaptionService captionService;

        //Injecter tout les services nécessaire
        public ImageService(IImageValidatorService imageValidationService, 
            IImageRecordRepo imgRepo, IImageUploadService uploadService,
            ICaptionService captionService,
            IMapper mapper)
        {
            this.imageValidationService = imageValidationService;
            this.imgRepo = imgRepo;
            this.uploadService = uploadService;
            this.mapper = mapper;
            this.captionService = captionService;
        }

        public async Task<string> GenerateCaption(IFormFile image)
        {
            try
            {
                return await captionService.GetCaption(image);
            }
            catch (Exception ex)
            {
                throw new HttpException(ex.Message);
            }
        }

        public async Task<IEnumerable<IImageRecordDTO>> GetImageRecordDTOs(Guid? id = null)
        {
            //Lire les données à partir de image repository et map a un DTO
            var imageRecords = await imgRepo.GetAll();
            return mapper.Map<List<IImageRecordDTO>>(imageRecords);
        }

        public async Task<IImageRecordDTO> SaveImageMetaData(string url, string caption)
        {
            try
            {
                return await imgRepo.AddImageMetaData(url, caption);
            }
            catch (Exception ex)
            {
                //TODO Journaliser les erreurs
                throw new HttpException(ex.Message);
            }
        }

        public async Task<string> UploadImage(IFormFile image)
        {
            try
            {
                return await uploadService.UploadImage(image);

            }
            catch (Exception ex)
            {
                throw new HttpException(ex.Message);
            }
        }

        public async Task<IImageRecordDTO> UploadAndSave(IFormFile image)
        {
            //TODO: UploadImage et GenerateCaption pourrait être exécuté en même temps 
            var url = await UploadImage(image);
            var caption = await GenerateCaption(image);

            //Enregistrer dans cosmosdb
            return await SaveImageMetaData(url, caption);
        }
    }

    //J'enregistre souvent les interfaces dans le meme fichier si c'est seulement 
    //implémenté ici
    public interface IImageService
    {
        public Task<IEnumerable<IImageRecordDTO>> GetImageRecordDTOs(Guid? id = null);
        public Task<string> UploadImage(IFormFile image);
        public Task<string> GenerateCaption(IFormFile image);
        public Task<IImageRecordDTO> SaveImageMetaData(string url, string caption);
        public Task<IImageRecordDTO> UploadAndSave(IFormFile image);
    }
}
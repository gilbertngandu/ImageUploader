using AutoMapper;
using ImageApplication.Domain.Models;
using ImageUploader.Common.Models;
using ImageUploader.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace ImageUploader.Infrastructure.Repositories
{
    //J utilise le repo generic et je passe simplement la classe ImageRecord
    public class ImageRecordRepo : GenericRepo<ImageRecord>, IImageRecordRepo
    {
        private readonly IMapper mapper;

        public ImageRecordRepo(ImageUploaderDbContext context, IConfiguration config, IMapper mapper) : base(context, config)
        {
            this.mapper = mapper;
        }

        public async Task<IImageRecordDTO> AddImageMetaData(string url, string caption)
        {
            var result = await Add(new ImageRecord
            {
                Id = Guid.NewGuid(),
                Url = url,
                Caption = caption,
                CreatedBy = "Gil", //TODO prendre ca du HttpContext
                CreatedDateTime = DateTime.Now,
                UpdatedDateTime = DateTime.Now,
            });

            return mapper.Map<IImageRecordDTO>(result);
        }
    }

    //DI Pour tester
    public interface IImageRecordRepo : IRepository<ImageRecord>
    {
        Task<IImageRecordDTO> AddImageMetaData(string url, string caption);
    }
}
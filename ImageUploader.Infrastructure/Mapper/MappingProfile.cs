using AutoMapper;
using ImageApplication.Domain.Models;
using ImageUploader.Common.Models;

namespace ImageUploader.Infrastructure.Mapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            //Je map avec ConstructUsing parce que CreateMap<ImageRecord, IImageRecordDTO> 
            //refuse de marcher. 
            CreateMap<ImageRecord, IImageRecordDTO>()
                .ConstructUsing(x => new ImageRecordDTO {
                    Id = x.Id, Caption = x.Caption, Url = x.Url, CreatedDateTime = x.CreatedDateTime});

        }
    }
}

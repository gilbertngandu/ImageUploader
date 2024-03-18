namespace ImageUploader.Common.Models
{
    //Dto pour eviter les classes de la BD
    public class ImageRecordDTO: IImageRecordDTO
    {
        public Guid Id { get; set; }
        public string Caption { get; set; }
        public string Url { get; set; }
        public DateTime CreatedDateTime { get; set; }

    }

    public interface IImageRecordDTO
    {
        public Guid Id { get; set; }
        public string Caption { get; set; }
        public string Url { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
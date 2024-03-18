namespace ImageUploader.Application.Models
{
    public class ImgUploadResult : IImgUploadResult
    {
        public string Url { get; set; }
        public string Caption { get; set; }

        public ImgUploadResult(string url, string caption)
        {
            this.Url = url;
            this.Caption = caption;
        }
    }

    public interface IImgUploadResult
    {
        public string Caption { get; set; }
        public string Url { get; set; }
    }
}

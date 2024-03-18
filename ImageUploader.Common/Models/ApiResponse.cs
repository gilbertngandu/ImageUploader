using ImageUploader.Common.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ImageUploader.Common.Models
{
    //Simple ApiResponse wrapper pour toutes les réponses
    public class ApiResponse<T> : IApiResponse<T>
    {
        
        public T Data { get; set; }
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool Success => StatusCode.IsSuccessful();

        public ApiResponse(T data, string message = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            this.Data = data;
            this.Message = message;
            this.StatusCode = statusCode;
        }

        public ApiResponse(string message, HttpStatusCode statusCode)
        {
            this.Message = message;
            this.StatusCode = statusCode;
        }
    }

    public interface IApiResponse<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool Success { get; }
    }
}
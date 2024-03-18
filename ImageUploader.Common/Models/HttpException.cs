using System.Net;

namespace ImageUploader.Common.Models
{
    //Quand on throw une execption il faudrait savoir si c'est une mauvaise requete ou une erreur interne
    //Cette classe permet de passer le HttpStatusCode 
    public class HttpException:Exception
    {
        public HttpException(string message, HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError):
            base(message)
        {
            this.HttpStatusCode = httpStatusCode;
        }

        public HttpStatusCode HttpStatusCode { get; set; }
        public int HttpCode => int.Parse(HttpStatusCode.ToString());

    }
}

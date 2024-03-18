using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ImageUploader.Common.Extension
{
    public static class HttpCodeExtension
    {
        //Extension pour facilement identifier les HttpStatusCode successful
        public static bool IsSuccessful(this HttpStatusCode statusCode)
        {
            var statusCodeValue = (int)statusCode;
            return statusCodeValue >= 200 && statusCodeValue < 300;
        }
    }
}

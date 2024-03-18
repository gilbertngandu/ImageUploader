using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageUploader.Common.Models
{
    public interface ICaptionExtractor
    {
        Task<string> ExtractCaption(IFormFile imageFormFile);
    }
}

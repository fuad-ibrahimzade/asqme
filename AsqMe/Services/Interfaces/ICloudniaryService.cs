using CloudinaryDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AsqMe.Services.Interfaces
{
    public interface ICloudniaryService
    {
        public Cloudinary cloudinary { get; }
        string UploadImage(string fileName, Stream stream);
        string DeleteImage(string url);
    }
}

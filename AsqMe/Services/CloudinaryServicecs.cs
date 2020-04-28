using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using AsqMe.Helpers;
using AsqMe.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AsqMe.Services
{
    public class CloudinaryService : ICloudniaryService
    {
        public Cloudinary cloudinary { get; }
        public CloudinaryService(IOptions<AppSettings> appSettings)
        {
            Account account = new Account(
                appSettings.Value.CloudinaryConfig.CloudName,
                appSettings.Value.CloudinaryConfig.ApiKey,
                appSettings.Value.CloudinaryConfig.ApiSecret);

            cloudinary = new Cloudinary(account);
        }

        public string UploadImage(string filename,Stream stream)
        {
            var trustedFileName= System.Net.WebUtility.HtmlEncode(filename);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(trustedFileName, stream)
            };
            var uploadResult = cloudinary.Upload(uploadParams);
            string url = cloudinary.Api.UrlImgUp.BuildUrl(String.Format("{0}.{1}", uploadResult.PublicId, uploadResult.Format));
            return url;
        }

        public string DeleteImage(string url)
        {
            string publicId= Regex.Replace(url,$@"{(url.Contains("https") ? "https" : "http")}:\/\/res\.cloudinary\.com\/.*\/image\/upload\/", "");
            publicId = Regex.Replace(publicId, $@"\.\w+$", "");
            var deletionParams = new DeletionParams(publicId);
            var deletionResult = cloudinary.Destroy(deletionParams);

            //cloudinary.DeleteResourcesAsync()
            //string url = cloudinary.Api.UrlImgUp.BuildUrl(String.Format("{0}.{1}", uploadResult.PublicId, uploadResult.Format));
            return deletionResult.Result;
        }

    }
}

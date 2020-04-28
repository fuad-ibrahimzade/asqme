using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fotografolio.Data.ViewModels
{
    public class FileAttribute : ValidationAttribute
    {
        public string[] FileTypes { get; set; }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile)
            {
                return ValidateSingleFile((IFormFile)value);
            }
            else if (value is IEnumerable<IFormFile>)
            {
                return ValidateMultipleFiles(value);
            }
            else
            {
                return new ValidationResult("The input type is not a file.");
            }
        }

        private ValidationResult ValidateSingleFile(IFormFile file)
        {
            if (!isFileAllowedType(file))
            {
                return DisallowedFileTypeError(file);
            }

            return ValidationResult.Success;
        }

        private ValidationResult ValidateMultipleFiles(object value)
        {
            IEnumerable<IFormFile> files = (IEnumerable<IFormFile>)value;

            foreach (var file in files)
            {
                if (!isFileAllowedType(file))
                {
                    return DisallowedFileTypeError(file);
                }
            }

            return ValidationResult.Success;
        }

        private bool isFileAllowedType(IFormFile file)
        {
            string fileType = file.ContentType.ToLower();
            return FileTypes.Length == 0 || FileTypes.Contains(fileType);
        }

        private ValidationResult DisallowedFileTypeError(IFormFile file)
        {
            string fileName = file.FileName;
            return new ValidationResult($"{ fileName } is not an allowed file type.");
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fotografolio.Data.ViewModels
{
    public class PhotosViewModel
    {
        [Required]
        [File(FileTypes = new string[] { "image/jpeg", "image/png", "image/bmp", "image/gif", "image/svg+xml" })]
        [BindProperty(Name = "photos[]")]
        public IFormFile[] photos { get; set; }
        [Required]
        public string gallery_id { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsqMe.Helpers
{
    public class AppSettings
    {
        public string BaseUrl { get; set; }
        public string Secret { get; set; }
        public string AdminEmail { get; set; }
        public string AdminPassword { get; set; }
        public FacebookAppConfig FacebookAppConfig { get; set; }
        public CloudinaryConfig CloudinaryConfig { get; set; }
    }
}

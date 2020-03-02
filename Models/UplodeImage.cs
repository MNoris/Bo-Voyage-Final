using System;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using static System.Net.Mime.MediaTypeNames;

namespace Bo_Voyage_Final.Models
{
    public class UplodeImage
    {
        public string Nom { get; set; }
        public string Destination { get; set; }
        [DisplayName("Impoter une image ...")]
        public IFormFile ImageFile { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace Bo_Voyage_Final.Models
{
    public class ContactInfos
    {
        [Required]
        public string Nom { get; set;}
        [Required]
        public string Prenom { get; set; }
        [Required,EmailAddress]
        public string Email { get; set; }
        [StringLength(250)]
        public string Message { get; set; }
    }
}

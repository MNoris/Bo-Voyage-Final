using System;
using System.ComponentModel.DataAnnotations;

namespace Bo_Voyage_Final.Models
{
    public class ContactInfos
    {
        [Required(ErrorMessage ="Votre nom doit êtee renseigné")]
        public string Nom { get; set;}
        [Required(ErrorMessage ="Votre prénom doit être renseigné")]
        public string Prenom { get; set; }
        [Required(ErrorMessage ="Votre email doit être renseigné"),EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage ="Veuillez rédiger votre message our vos commentaires"),StringLength(300)]
        public string Message { get; set; }
    }
}

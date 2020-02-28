using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bo_Voyage_Final.Models
{
    public partial class Personne
    {
        public Personne()
        {
            Voyageur = new HashSet<Voyageur>();
        }

        [Display(Name = "ID client")]
        public int Id { get; set; }
        public byte TypePers { get; set; }
        [Display(Name = "Civilité")]
        public string Civilite { get; set; }
        [Display(Name = "Nom")]
        public string Nom { get; set; }
        [Display(Name = "Prénom")]
        public string Prenom { get; set; }
        [Display(Name = "Adresse e-mail"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "Numéro de téléphone"), DataType(DataType.PhoneNumber)]
        public string Telephone { get; set; }
        [Display(Name = "Date de Naissance"), DataType(DataType.Date)]
        public DateTime? Datenaissance { get; set; }

        public virtual Client Client { get; set; }
        public virtual ICollection<Voyageur> Voyageur { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bo_Voyage_Final.Models
{
    public partial class Dossierresa
    {
        [Display(Name = "Numéro de dossier")]
        public int Id { get; set; }
        [Display(Name = "Numéro de carte bleue"), Required]
        [StringLength(16, ErrorMessage ="Veuillez entrer un numéro de carte de crédit de 16 chiffres (Sans les espaces)")]
        public string NumeroCb { get; set; }
        [Display(Name = "Prix Total")]
        public decimal PrixTotal { get; set; }
        [Display(Name = "Numéro client")]
        public int IdClient { get; set; }
        [Display(Name = "Etat du dossier")]
        public byte IdEtatDossier { get; set; }
        [Display(Name = "Numéro de voyage")]
        public int IdVoyage { get; set; }

        public virtual Client IdClientNavigation { get; set; }
        [Display(Name ="Etat du dossier")]
        public virtual Etatdossier IdEtatDossierNavigation { get; set; }
        public virtual Voyage IdVoyageNavigation { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bo_Voyage_Final.Models
{
    public partial class Dossierresa
    {
        [Display(Name = "Numéro de dossier")]
        public int Id { get; set; }
        [Display(Name = "Numéro de carte bleue")]
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
        public virtual Etatdossier IdEtatDossierNavigation { get; set; }
        public virtual Voyage IdVoyageNavigation { get; set; }
    }
}

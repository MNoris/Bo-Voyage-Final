using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Bo_Voyage_Final.Models
{
    public partial class Voyage
    {
        public Voyage()
        {
            Dossierresa = new HashSet<Dossierresa>();
            Voyageur = new HashSet<Voyageur>();
        }

        [Display(Name = "Id voyage")]
        public int Id { get; set; }
        [Display(Name = "Destination")]
        public int IdDestination { get; set; }

        [Display(Name ="Date de départ"), DataType(DataType.Date)]
        public DateTime DateDepart { get; set; }

        [Display(Name = "Date de Retour"), DataType(DataType.Date)]
        public DateTime DateRetour { get; set; }

        [Display(Name="Places disponibles")]
        public int PlacesDispo { get; set; }
        [Display(Name = "Prix hors-taxe par pers.")]
        [DataType(DataType.Currency)]
        public decimal PrixHt { get; set; }
        [Display(Name = "Réduction")]
        public decimal Reduction { get; set; }
        public string Descriptif { get; set; }
        [Display(Name="Destination")]
        public virtual Destination IdDestinationNavigation { get; set; }
        public virtual ICollection<Dossierresa> Dossierresa { get; set; }
        public virtual ICollection<Voyageur> Voyageur { get; set; }
    }
}

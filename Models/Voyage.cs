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

        public int Id { get; set; }
        public int IdDestination { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateDepart { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateRetour { get; set; }
        public int PlacesDispo { get; set; }
        public decimal PrixHt { get; set; }
        public decimal Reduction { get; set; }
        public string Descriptif { get; set; }

        public virtual Destination IdDestinationNavigation { get; set; }
        public virtual ICollection<Dossierresa> Dossierresa { get; set; }
        public virtual ICollection<Voyageur> Voyageur { get; set; }
    }
}

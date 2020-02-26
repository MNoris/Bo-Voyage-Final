using System;
using System.Collections.Generic;


namespace Bo_Voyage_Final.Models
{
    public partial class Photo
    {
        public int Id { get; set; }
        public string NomFichier { get; set; }
        public int IdDestination { get; set; }

        public virtual Destination IdDestinationNavigation { get; set; }
    }
}

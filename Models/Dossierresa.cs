using System;
using System.Collections.Generic;


namespace Bo_Voyage_Final.Models
{
    public partial class Dossierresa
    {
        public int Id { get; set; }
        public string NumeroCb { get; set; }
        public int IdClient { get; set; }
        public byte IdEtatDossier { get; set; }
        public int IdVoyage { get; set; }

        public virtual Client IdClientNavigation { get; set; }
        public virtual Etatdossier IdEtatDossierNavigation { get; set; }
        public virtual Voyage IdVoyageNavigation { get; set; }
    }
}

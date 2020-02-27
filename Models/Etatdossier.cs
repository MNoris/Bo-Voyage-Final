using System;
using System.Collections.Generic;


namespace Bo_Voyage_Final.Models
{
    public partial class Etatdossier
    {
        public Etatdossier()
        {
            Dossierresa = new HashSet<Dossierresa>();
        }

        public byte Id { get; set; }
        public string Libelle { get; set; }

        public virtual ICollection<Dossierresa> Dossierresa { get; set; }
    }
}

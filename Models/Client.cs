using System;
using System.Collections.Generic;

namespace Bo_Voyage_Final.Models
{
    public partial class Client
    {
        public Client()
        {
            Dossierresa = new HashSet<Dossierresa>();
        }

        public int Id { get; set; }

        public virtual Personne IdNavigation { get; set; }
        public virtual ICollection<Dossierresa> Dossierresa { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace Bo_Voyage_Final.Models
{
    public partial class Personne
    {
        public Personne()
        {
            Voyageur = new HashSet<Voyageur>();
        }

        public int Id { get; set; }
        public byte TypePers { get; set; }
        public string Civilite { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public DateTime? Datenaissance { get; set; }

        public virtual Client Client { get; set; }
        public virtual ICollection<Voyageur> Voyageur { get; set; }
    }
}

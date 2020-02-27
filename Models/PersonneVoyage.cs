using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bo_Voyage_Final.Models
{
    public class PersonneVoyage
    {
        public PersonneVoyage()
        {

        }

        public PersonneVoyage(Personne personnes, Voyage voyages)
        {
            Personne = personnes;
            Voyage = voyages;
        }

        public Personne Personne { get; set; }
        public Voyage Voyage { get; set; }
    }
}

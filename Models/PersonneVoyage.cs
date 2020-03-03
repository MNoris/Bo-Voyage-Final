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

        public PersonneVoyage(Personne personne, Voyage voyage)
        {
            Personne = personne;
            Voyage = voyage;
            Voyageurs = new List<Personne>();
        }


        public Personne Personne { get; set; }
        public Voyage Voyage { get; set; }
        public List<Personne> Voyageurs { get; set; }

        public void addVoyageur()
        {
            Voyageurs.Add(new Personne());
        }

        public void removeVoyageur()
        {
            Voyageurs.RemoveAt(Voyageurs.Count());
        }
    }
}

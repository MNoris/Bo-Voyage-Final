using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bo_Voyage_Final.Models
{
    public class VoyagesDossiersResa
    {

        public List<Voyage> listeVoyageImminent { get; set; }

        public  List<Dossierresa> listeDossierResaEnCours { get; set; }

        public VoyagesDossiersResa(List<Voyage> listeVoyageImminent, List<Dossierresa> listeDossierResaEnCours)
        {
            this.listeVoyageImminent = listeVoyageImminent;
            this.listeDossierResaEnCours = listeDossierResaEnCours;
        }
    }
}

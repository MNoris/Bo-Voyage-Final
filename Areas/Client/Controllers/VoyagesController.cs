using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bo_Voyage_Final.Models;
using Microsoft.AspNetCore.Identity;

namespace Bo_Voyage_Final.Areas.Client.Controllers
{
    [Area("Client")]
    public class VoyagesController : Controller
    {
        private readonly BoVoyageContext _context;
        private readonly UserManager<IdentityUser> _um;

        public VoyagesController(BoVoyageContext context, UserManager<IdentityUser> um)
        {
            _context = context;
            _um = um;
        }

        // GET: Client/Voyages
        public async Task<IActionResult> Index(int idCont, int idPays, int idRegion, decimal minPrice, decimal maxPrice, DateTime dateMin, DateTime dateMax, int page = 1)
        {
            //Rq pour import des destinations dans la liste déroulante
            ViewBag.Destinations = _context.Destination.AsNoTracking().ToList();
            //Memo des valeurs des filtres en cours

            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;

            //Passer un string format yyyy-MM-dd comme valeur par défaut pour l'input type date
            //valeur par defaut minimal : date du jour, max : 7jours plus tard
            if (dateMin == DateTime.MinValue)
            {
                dateMin = DateTime.Today;

                ViewBag.dateMin = dateMin.ToString("yyyy-MM-dd");

            }
            else ViewBag.dateMin = dateMin.ToString("yyyy-MM-dd");

            if (dateMax == DateTime.MinValue)
            {
                dateMax = DateTime.Today.AddDays(7);

                ViewBag.dateMax = dateMax.ToString("yyyy-MM-dd");

            }
            else ViewBag.dateMax = dateMax.ToString("yyyy-MM-dd");

            //Requete de récupération des voyages et destination
            IQueryable<Voyage> reqVoyages = _context.Voyage.Where(v => v.PlacesDispo > 0).Include(v => v.IdDestinationNavigation).ThenInclude(e => e.Photo)
                .OrderBy(e => e.IdDestinationNavigation.Nom);

            //////////////////////////////////////////////////////////
            ViewBag.Continent = _context.Destination.Where(d => d.Niveau == 1).AsNoTracking().ToList();
            ViewData["idCont"] = idCont;

            ViewBag.Pays = _context.Destination.Where(d => d.Niveau == 2 && d.IdParente == idCont).AsNoTracking().ToList();
            ViewData["IdPays"] = idPays;

            ViewBag.Region = _context.Destination.Where(d => d.Niveau == 3 && d.IdParente == idPays).AsNoTracking().ToList();
            ViewData["IdRegion"] = idRegion;

            if (idCont != 0)
            {  //Application du filtre Sur les continents

                //recuperation de la liste des iddestination des region de l'id du continent
                var req2 = reqVoyages.Where(d => d.IdDestinationNavigation.IdParente == idCont).ToList();
                var req3 = reqVoyages.Where(d => d.IdDestinationNavigation.Niveau == 3).ToList();
                List<int> idRegional = new List<int>();

                foreach (var voy3 in req3)
                {
                    foreach (var voy2 in req2)
                    {
                        if (voy3.IdDestinationNavigation.Niveau == 3 && voy3.IdDestinationNavigation.IdParente == voy2.IdDestination)
                        { 
                            idRegional.Add(voy3.Id);
                        }
                    }
                }

                // req 4 = toutes les regions
                var req4 = reqVoyages.Where(d => d.IdDestinationNavigation.Niveau == 3);

                // reqvoyages contients les voyages du continent et pays concernés
                reqVoyages = reqVoyages.Where(d => (d.IdDestination == idCont) || (d.IdDestinationNavigation.IdParente == idCont));

                //pour chaque id de la liste idRegional, req 5 recupère un voyage dans les regions,
                //puis fusionne cette valeur à reqVoyage
                foreach (var identifiant in idRegional)
                {
                    var req5 = req4.Where(a => a.Id == identifiant);
                    reqVoyages = reqVoyages.Union(req5);
                }

                //filtre pays, dépendant du conitnent
                if (idPays != 0)
                {
                    reqVoyages = reqVoyages.Where(d => d.IdDestination == idPays || d.IdDestinationNavigation.IdParente == idPays);

                    if (idRegion != 0)
                    {
                        reqVoyages = reqVoyages.Where(d => d.IdDestination == idRegion);

                    }
                }
            }

            //filtres par prix
            if (minPrice != 0 || maxPrice != 0)
                reqVoyages = reqVoyages.Where(p => p.PrixHt <= maxPrice && p.PrixHt >= minPrice);

            //filtres par date de départ
            if (dateMin != DateTime.Today || dateMax != DateTime.Today.AddDays(7))
                reqVoyages = reqVoyages.Where(d => d.DateDepart >= dateMin && d.DateDepart <= dateMax);

            //   var listeVoyages = await reqVoyages.AsNoTracking().ToListAsync();

            var listeVoyages = await PageItems<Voyage>.CreateAsync(
          reqVoyages.AsNoTracking(), page, 15);

            return View(listeVoyages);
        }

        // GET: Client/Voyages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            //TODO comments & errors
            if (id == null)
                return NotFound();

            var voyage = await _context.Voyage
                .Include(v => v.IdDestinationNavigation).ThenInclude(d => d.Photo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (voyage == null)
                return NotFound();

            return View(voyage);
        }

        public async Task<IActionResult> Reserver(int? id)
        {
            try
            {
                //TODO comments & errors
                var mail = _um.GetUserName(HttpContext.User);
                var user = await _context.Personne.AsNoTracking().FirstOrDefaultAsync(p => p.Email == mail);
                var voyage = await _context.Voyage.Include(v => v.IdDestinationNavigation).FirstOrDefaultAsync(v => v.Id == id);
                var personneVoyage = new PersonneVoyage(user, voyage);

                return View("Reserver", personneVoyage);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public IActionResult AddVoyageur(int idPersonne, int idVoyage, int? nbVoyageurs)
        {
            //TODO comments & errors
            var personne = _context.Personne.Find(idPersonne);
            var voyage = _context.Voyage.Include(v => v.IdDestinationNavigation).FirstOrDefault(v => v.Id == idVoyage);

            var pv = new PersonneVoyage(personne, voyage);

            for (int i = 0; i < (int)nbVoyageurs; i++)
            {
                pv.addVoyageur();
            }

            return View("Reserver", pv);
        }

        [HttpPost]
        public IActionResult Payer(int idPersonne, int idVoyage, [Bind("Email", "Telephone", "Datenaissance")]List<Personne> voyageurs)
        {
            //TODO comments & errors
            var client = _context.Personne.Find(idPersonne);
            client.TypePers = 1;
            client.Client = new Models.Client { Id = idPersonne };
            var voyage = _context.Voyage.Include(v => v.IdDestinationNavigation).FirstOrDefault(v => v.Id == idVoyage);

            var pv = new PersonneVoyage(client, voyage);

            var price = voyage.PrixHt;

            foreach (var item in voyageurs)
            {
                item.Civilite = "";
                item.Nom = "";
                item.Prenom = "";
                item.TypePers = 2;
                item.Datenaissance ??= null;

                if (item.Datenaissance != null)
                {
                    var age = DateTime.Today.Year - ((DateTime)item.Datenaissance).Year;
                    if (((DateTime)item.Datenaissance).Date > DateTime.Today.AddYears(-age)) age--;

                    if (age <= 12)
                        price += voyage.PrixHt * (1 - voyage.Reduction);
                }
                else
                    price += voyage.PrixHt;

                client.Voyageur.Add(new Voyageur() { Id = item.Id, Idvoyage = idVoyage });
            }

            var dossierRes = new Dossierresa
            {
                IdClient = idPersonne,
                IdEtatDossier = 1,
                IdVoyage = idVoyage,
                NumeroCb = "",
                PrixTotal = price
            };

            try
            {
                _context.Personne.Update(client);
                _context.SaveChanges();
            }
            catch (Exception e)
            {

                throw e;
            }

            return View("Payer", dossierRes);
        }

        [HttpPost]
        public IActionResult EnregistrerResa(string numeroCb, decimal prixTotal, int idPersonne, int idVoyage)
        {
            //TODO comments & errors
            var dossier = new Dossierresa
            {
                IdClient = idPersonne,
                IdEtatDossier = 1,
                IdVoyage = idVoyage,
                PrixTotal = prixTotal,
                NumeroCb = numeroCb
            };

            try
            {
                _context.Dossierresa.Add(dossier);
                _context.SaveChanges();
            }
            catch (Exception e)
            {

                throw e;
            }

            return View("../Personnes/MesReservations");
        }

        //[HttpPost]
        //public IActionResult RemoveVoyageur(PersonneVoyage personneVoyage)
        //{
        //    personneVoyage.removeVoyageur();

        //    return View("Reserver", personneVoyage);
        //}

        private bool VoyageExists(int id)
        {
            return _context.Voyage.Any(e => e.Id == id);
        }
    }
}

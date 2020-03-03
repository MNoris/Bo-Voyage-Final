using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bo_Voyage_Final.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

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




        public async Task<IActionResult> ListeDestinationVoyage(int id)
        {
            var listeVoyages = _context.Voyage
                                .Include(v => v.IdDestinationNavigation)
                                .ThenInclude(d => d.Photo)
                                .Where(v => v.IdDestination == id && v.PlacesDispo > 0);


            var voyage = listeVoyages.FirstOrDefault();
            ViewBag.NomDestination = voyage.IdDestinationNavigation.Nom;


            return View(await listeVoyages.AsNoTracking().ToListAsync());
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

            ViewBag.Nom = voyage.IdDestinationNavigation.Nom;

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

                return BadRequest();
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
                pv.AddVoyageur();
            }

            return View("Reserver", pv);
        }

        [HttpPost]
        public IActionResult Payer(int idPersonne, int idVoyage, [Bind("Email", "Telephone", "Datenaissance")]List<Personne> voyageurs)
        {
            //Récupération de la personne
            var client = _context.Personne.Find(idPersonne);

            //Vérifie si la personne qui effectue une réservation est déjà enregistrée en tant que Client ou non
            if (!_context.Client.Any(c => c.Id == idPersonne))
            {
                client.TypePers = 1;
                client.Client = new Models.Client { Id = idPersonne };
            }

            //Récupération du voyage à réserver
            var voyage = _context.Voyage.Include(v => v.IdDestinationNavigation).FirstOrDefault(v => v.Id == idVoyage);

            var price = voyage.PrixHt;

            foreach (var item in voyageurs)
            {
                //Vérifie que l'adresse mail de chaque voyageur est différente de celle du client
                if (item.Email == client.Email)
                {
                    return BadRequest($"L'adresse mail {item.Email} est déjà utilisée par le client, merci d'utiliser une adresse mail différente.");
                }

                if (!_context.Personne.Where(p => p.Email == item.Email).Any())
                {
                    item.Civilite = "";
                    item.Nom = "";
                    item.Prenom = "";
                    item.TypePers = 2;
                    item.Datenaissance ??= null;
                    item.Telephone ??= null;

                    try
                    {
                        //Enregistre la personne dans un premier temps, afin de pouvoir récupérer son ID et l'assigner en tant que voyageur
                        _context.Personne.Add(item);
                        _context.SaveChanges();
                    }
                    catch (DbUpdateException dbue)
                    {
                        var sqle = (SqlException)dbue.InnerException;

                        //Vérifie si l'erreur reçue a pour numéro 515, et renvoie un message associé
                        if (sqle.Number == 515)
                        {
                            return BadRequest("L'adresse email doit être renseignée et unique pour tous les voyageurs.");
                        }
                    }
                    catch (Exception)
                    {
                        return BadRequest("Une erreur innatendue est survenue. Merci de rééssayer.");
                    }

                    //Instancie le voyageur et sauvegarde sa liaison
                    Voyageur voyageur = new Voyageur() { Id = item.Id, Idvoyage = idVoyage };
                    _context.Voyageur.Add(voyageur);
                }
                else
                {
                    //Si le voyageur existe déjà en base, l'associe au voyage, au lieu de créer un nouveau voyageur, basé sur son adresse email
                    var voyageur = _context.Personne.Where(p => p.Email == item.Email).FirstOrDefault();
                    _context.Voyageur.Add(new Voyageur() { Id = voyageur.Id, Idvoyage = idVoyage });
                }

                if (item.Datenaissance != null)
                {
                    var age = DateTime.Today.Year - ((DateTime)item.Datenaissance).Year;
                    if (((DateTime)item.Datenaissance).Date > DateTime.Today.AddYears(-age)) age--;

                    // Réduc enfant
                    if (age <= 12)
                        price += voyage.PrixHt * (decimal)0.40;
                    else
                        price += voyage.PrixHt;
                }
                else
                    price += voyage.PrixHt;
            }

            //Création du dossier de réservation
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
            catch (DbUpdateException dbue)
            {
                var sqle = (SqlException)dbue.InnerException;

                //Vérifie si l'erreur reçue a pour numéro 2627, et renvoie un message associé
                if (sqle.Number == 2627)
                {
                    return BadRequest("Un des voyageurs participe déjà à ce voyage.");
                }
            }
            catch (Exception)
            {
                return BadRequest("Une erreur innatendue est survenue. Merci de rééssayer.");
            }

            return View("Payer", dossierRes);
        }

        [HttpPost]
        public IActionResult EnregistrerResa([Bind("NumeroCb", "PrixTotal", "IdClient", "IdVoyage")] Dossierresa dossier)
        {
            //TODO comments & errors

            dossier.IdEtatDossier = 1;

            try
            {
                _context.Dossierresa.Add(dossier);
                _context.SaveChanges();
            }
            catch (Exception e)
            {

                throw e;
            }

            return RedirectToAction("Index", "Dossierresas");
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

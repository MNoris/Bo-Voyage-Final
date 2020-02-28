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
        public async Task<IActionResult> Index(int IdDestination, decimal minPrice, decimal maxPrice, DateTime dateMin, DateTime dateMax)
        {
            //Rq pour import des destinations dans la liste déroulante
            ViewBag.Destinations = _context.Destination.AsNoTracking().ToList();
            //Memo des valeurs des filtres en cours
            ViewData["idDestination"] = IdDestination;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;

            //Passer un string format yyyy-MM-dd comme valeur par défaut pour l'input type date
            //valeur par defaut minimal : date du jour, max : 7jours plus tard
            if (dateMin == DateTime.MinValue)
                ViewBag.dateMin = DateTime.Now.ToString("yyyy-MM-dd");
            else ViewBag.dateMin = dateMin.ToString("yyyy-MM-dd");

            if (dateMax == DateTime.MinValue)
                ViewBag.dateMax = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");
            else ViewBag.dateMax = dateMax.ToString("yyyy-MM-dd");



            //Requete de récupération des voyages et destination
            IQueryable<Voyage> reqVoyages = _context.Voyage.Where(v => v.PlacesDispo > 0).Include(v => v.IdDestinationNavigation).ThenInclude(e => e.Photo)
                .OrderBy(e => e.IdDestinationNavigation.Nom);

            if (IdDestination != 0)
                //Application du filtre Sur les destinations
                reqVoyages = reqVoyages.Where(d => d.IdDestination == IdDestination);

            //filtres par prix
            if (minPrice != 0 || maxPrice != 0)
                reqVoyages = reqVoyages.Where(p => p.PrixHt <= maxPrice && p.PrixHt >= minPrice);

            //filtres par date de départ
            if (dateMin != DateTime.MinValue || dateMax != DateTime.MinValue)
                reqVoyages = reqVoyages.Where(d => d.DateDepart >= dateMin && d.DateDepart <= dateMax);

            var listeVoyages = await reqVoyages.AsNoTracking().ToListAsync();

            return View(listeVoyages);
        }

        // GET: Client/Voyages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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
            //    string mail = "";
            //    if (!string.IsNullOrWhiteSpace((string)ViewBag.User))



            try
            {
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
        public IActionResult AddVoyageur(int? idPersonne, int? idVoyage, int? nbVoyageurs)
        {
            var personne = _context.Personne.Find(idPersonne);
            var voyage = _context.Voyage.Include(v => v.IdDestinationNavigation).FirstOrDefault(v => v.Id == idVoyage);

            var pv = new PersonneVoyage(personne, voyage);

            for (int i = 0; i < (int)nbVoyageurs; i++)
            {
                pv.addVoyageur();
            }

            return View("Reserver", pv);
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

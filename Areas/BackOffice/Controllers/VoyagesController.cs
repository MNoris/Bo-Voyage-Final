using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bo_Voyage_Final.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace Bo_Voyage_Final.Areas.BackOffice.Controllers
{
    [Area("BackOffice")]
    [Authorize(Roles = "Admin")]
    public class VoyagesController : Controller
    {
        private readonly BoVoyageContext _context;

        public VoyagesController(BoVoyageContext context)
        {
            _context = context;
        }

        // GET: Identity/Voyages
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
            if (minPrice != 0)
                reqVoyages = reqVoyages.Where(p => p.PrixHt >= minPrice);

            if (maxPrice != 0)
                reqVoyages = reqVoyages.Where(p => p.PrixHt <= maxPrice);

            //filtres par date de départ
            if (dateMin != DateTime.Today || dateMax != DateTime.Today.AddDays(7))
                reqVoyages = reqVoyages.Where(d => d.DateDepart >= dateMin && d.DateDepart <= dateMax);

            //   var listeVoyages = await reqVoyages.AsNoTracking().ToListAsync();

            var listeVoyages = await PageItems<Voyage>.CreateAsync(
          reqVoyages.AsNoTracking(), page, 15);

            return View(listeVoyages);
        }


        public IActionResult EditerVoyages(string id)
        {
            ViewBag.Action = id;
            var voyages = _context.Voyage.Include(v => v.IdDestinationNavigation).OrderBy(v => v.DateDepart).ToList();
            return View(voyages);
        }

        // GET: Identity/Voyages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voyage = await _context.Voyage
                .Include(v => v.IdDestinationNavigation).ThenInclude(p => p.Photo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (voyage == null)
            {
                return NotFound();
            }

            return View(voyage);
        }

        // GET: Identity/Voyages/Create
        public IActionResult Create()
        {
            ViewData["IdDestination"] = new SelectList(_context.Destination.OrderBy(d => d.Nom), "Id", "Nom");
            return View();
        }

        // POST: Identity/Voyages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdDestination,DateDepart,DateRetour,PlacesDispo,PrixHt,Reduction,Descriptif")] Voyage voyage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(voyage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(EditerVoyages));
            }
            ViewData["IdDestination"] = new SelectList(_context.Destination.OrderBy(d => d.Nom), "Id", "Nom", voyage.IdDestination);
            return View(voyage);
        }

        // GET: Identity/Voyages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voyage = await _context.Voyage.FindAsync(id);
            if (voyage == null)
            {
                return NotFound();
            }
            ViewData["IdDestination"] = new SelectList(_context.Destination, "Id", "Nom", voyage.IdDestination);
            return View(voyage);
        }

        // POST: Identity/Voyages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdDestination,DateDepart,DateRetour,PlacesDispo,PrixHt,Reduction,Descriptif")] Voyage voyage)
        {
            if (id != voyage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(voyage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoyageExists(voyage.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(EditerVoyages));
            }
            ViewData["IdDestination"] = new SelectList(_context.Destination, "Id", "Nom", voyage.IdDestination);
            return View(voyage);
        }

        // GET: Identity/Voyages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voyage = await _context.Voyage
                .Include(v => v.IdDestinationNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (voyage == null)
            {
                return NotFound();
            }

            return View(voyage);
        }

        // POST: Identity/Voyages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var voyage = await _context.Voyage.FindAsync(id);
            try
            {
                _context.Voyage.Remove(voyage);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException is SqlException)
                {
                    if (((SqlException)e.InnerException).Number == 547)
                    {

                        return View("ErreurSuppression");
                    }
                }
            }
            return RedirectToAction(nameof(EditerVoyages));
        }

        private bool VoyageExists(int id)
        {
            return _context.Voyage.Any(e => e.Id == id);
        }

        public ActionResult Error(SqlException e)
        {
            if (e.Number == 547)
            {
                return BadRequest("Probleme!! lien avec d'autre table (clé etrangere)");
            }
            return StatusCode(500, e.Message);

        }
    }
}

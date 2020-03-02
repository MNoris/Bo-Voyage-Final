using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bo_Voyage_Final.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bo_Voyage_Final.Areas.BackOffice.Controllers
{
    [Area("BackOffice")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly BoVoyageContext _context;
            public HomeController (BoVoyageContext context)
            {
                _context = context;
            }
        public IActionResult Index(int nbJours=15)
        {
            

            ViewBag.NbJours = nbJours;
            

            var DateLimite = DateTime.Now.AddDays(nbJours);



            List<Voyage>voyages = _context.Voyage.Include(d=>d.IdDestinationNavigation).ThenInclude(p=>p.Photo)
                                                                    .Where(i=>i.DateDepart<= DateLimite)
                                                                    .AsNoTracking().ToList();



           List<Dossierresa> reservations = _context.Dossierresa.Include(d=>d.IdEtatDossierNavigation)
              .Where(d=>d.IdEtatDossierNavigation.Id==1).AsNoTracking().ToList();
              

            VoyagesDossiersResa vdr = new VoyagesDossiersResa(voyages, reservations);



            return View(vdr);
        }
    }
}
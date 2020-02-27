using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bo_Voyage_Final.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bo_Voyage_Final.Areas.Client.Controllers
{
    [Area("Client")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BoVoyageContext _context;

        public HomeController(ILogger<HomeController> logger, BoVoyageContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            /*
             * "Top 5 Voyages le moins chèrs" = 0
             * "Top 5 Voyages de départ iminent" )= 1
             * "Top 5 Voyages pour les pays les plus visités" = 2
             */
            var top5Voyages = new Dictionary<int, List<Voyage>>();

            List<Voyage> list5MoinsCher = _context.Voyage.Include(v => v.IdDestinationNavigation)
                                                         .ThenInclude(d => d.Photo)
                                                         .OrderBy(v => v.PrixHt * (1 - v.Reduction)).Take(5).ToList();
            top5Voyages.Add(0, list5MoinsCher);
            List<Voyage> list5DepartIminent = _context.Voyage.Include(v => v.IdDestinationNavigation)
                                                    .ThenInclude(d => d.Photo)
                                                    .OrderBy(v => v.DateDepart).Take(5).ToList();
            top5Voyages.Add(1, list5DepartIminent);


            List<Voyage> list5VoyagePays = new List<Voyage>();
            var req = @"select top(5) d.Id,d.Nom,count(*) NbVoyage
                        from Voyage v
                        inner join Destination d on d.Id = v.IdDestination
                        where d.Niveau = 2
                        group by d.Id,d.Nom
                        order by count(*) desc
                        ";
            using (var cnx = (SqlConnection)_context.Database.GetDbConnection())
            {
                var cmd = new SqlCommand(req, cnx);
                cnx.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read() && list5VoyagePays.Count < 6)
                    {
                        int idDest = (int)reader["Id"];
                        var voyages = _context.Voyage.Include(v => v.IdDestinationNavigation)
                                                     .ThenInclude(d => d.Photo).Where(v => v.IdDestination == idDest)
                                                     .AsNoTracking().ToList();
                        list5VoyagePays.AddRange(voyages);
                    }
                }

            }
            top5Voyages.Add(2, list5VoyagePays);

            return View(top5Voyages);
        }


        public IActionResult Contact(ContactInfos contactInfos)
        {
            ViewBag.sendingEmail = false;
            if (ModelState.IsValid)
            {
                ViewBag.sendingEmail = true;
            }
            return View();
            }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}
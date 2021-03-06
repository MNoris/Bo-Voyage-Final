﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bo_Voyage_Final.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<IdentityUser> _userManager;


        public HomeController(ILogger<HomeController> logger, BoVoyageContext context,UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
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
                                                         .Where(v => v.PlacesDispo>0)
                                                         .OrderBy(v => v.PrixHt * (1 - v.Reduction)).Take(5).AsNoTracking().ToList();
            top5Voyages.Add(0, list5MoinsCher);
            List<Voyage> list5DepartIminent = _context.Voyage.Include(v => v.IdDestinationNavigation)
                                                    .ThenInclude(d => d.Photo)
                                                    .Where(v => v.PlacesDispo > 0)
                                                    .OrderBy(v => v.DateDepart).Take(5).AsNoTracking().ToList();
            top5Voyages.Add(1, list5DepartIminent);




           // var top5destinations = _context.Voyage.Include(d => d.IdDestinationNavigation).ThenInclude(p => p.Photo).GroupBy(a=>a.IdDestination).ToList();
    


             List<Destination> listTop5Destinations = new List<Destination>();
                 var req = @"select distinct top(5) d.Id,d.Nom,count(*) NbVoyage
                             from Voyage v
                             inner join Destination d on d.Id = v.IdDestination
                             group by d.Id,d.Nom
                             order by count(*) desc
                             ";

            

                      using (var cnx = (SqlConnection)_context.Database.GetDbConnection())
                      {
                          var cmd = new SqlCommand(req, cnx);
                          cnx.Open();
                          using (SqlDataReader reader = cmd.ExecuteReader())
                          {
                              while (reader.Read() && listTop5Destinations.Count < 5)
                              {
                                  int idDest = (int)reader["Id"];
                                  var destinations = _context.Destination.Include(d => d.Photo).Where(v => v.Id == idDest)
                                                               .AsNoTracking().ToList();
                        listTop5Destinations.AddRange(destinations);
                              }
                          }

                      }

            ViewBag.Destinations = listTop5Destinations;

            return View(top5Voyages);
        }

        public IActionResult Contact()
        {
            var userEmail = _userManager.GetUserName(HttpContext.User);
            var user = _context.Personne.Where(p => p.Email == userEmail).AsNoTracking().SingleOrDefault();
            ViewBag.Prenom = user != null ? user.Prenom : "";
            ViewBag.Nom = user != null ? user.Nom : "";
            ViewBag.Email = user != null ? user.Email : "";

            ViewBag.sendingEmail = false;
            return View();
        }

        [HttpPost]
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

        public IActionResult APropos()
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
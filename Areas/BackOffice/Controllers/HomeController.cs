using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Bo_Voyage_Final.Areas.BackOffice.Controllers
{
    [Area("BackOffice")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {


            return View();
        }
    }
}
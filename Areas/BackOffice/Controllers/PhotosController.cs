using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bo_Voyage_Final.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Bo_Voyage_Final.Areas.BackOffice.Controllers
{
    [Area("BackOffice")]
    public class PhotosController : Controller
    {
        private readonly BoVoyageContext _context;
        [Obsolete]
        private readonly IHostingEnvironment _env;

        [Obsolete]
        public PhotosController(BoVoyageContext context, IHostingEnvironment environment)
        {
            _context = context;
            _env = environment;
        }

        // GET: BackOffice/Photos
        public async Task<IActionResult> Index()
        {
            var boVoyageContext = _context.Photo.Include(p => p.IdDestinationNavigation);
            return View(await boVoyageContext.ToListAsync());
        }

        // GET: BackOffice/Photos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photo
                .Include(p => p.IdDestinationNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        // GET: BackOffice/Photos/Create
        public IActionResult Create(int id)
        {
            ViewData["IdDestination"] = new SelectList(_context.Destination, "Id", "Nom", id);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<IActionResult> Create(List<IFormFile> files, [Bind("NomFichier,IdDestination")] Photo photo)
        {

            if (ModelState.IsValid)
            {
                string imagesPath = "/Images/";
                string nomFichier = photo.NomFichier;
                for (int i = 0; i < files.Count(); i++)
                {
                    var img = files[i];
                    var imgTemp = new Photo();

                    var nomImage = string.Empty;
                    var uploadPath = _env.WebRootPath + imagesPath;
                    if (!string.IsNullOrEmpty(nomFichier))
                    {
                        nomImage = Path.GetFileName(nomFichier + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString() + "." + img.FileName.Split(".")[1]);
                    }
                    else
                    {
                        nomImage = Path.GetFileName(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString() + "." + img.FileName.Split(".")[1]);
                    }
                    string fullPath = uploadPath + nomImage;
                    using var fileStreamer = new FileStream(fullPath, FileMode.Create);
                    await img.CopyToAsync(fileStreamer);

                    imgTemp.NomFichier = imagesPath + nomImage;
                    imgTemp.IdDestination = photo.IdDestination;
                    _context.Add(imgTemp);
                    await _context.SaveChangesAsync();
                }
                if (ModelState.IsValid)
                {
                    return View("PhotoLoaded");

                }
            }
            return View();
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,NomFichier,IdDestination")] Photo photo)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(photo);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["IdDestination"] = new SelectList(_context.Destination, "Id", "Nom", photo.IdDestination);
        //    return View(photo);
        //}

        // GET: BackOffice/Photos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photo.FindAsync(id);
            if (photo == null)
            {
                return NotFound();
            }
            ViewData["IdDestination"] = new SelectList(_context.Destination, "Id", "Nom", photo.IdDestination);
            return View(photo);
        }

        // POST: BackOffice/Photos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NomFichier,IdDestination")] Photo photo)
        {
            if (id != photo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(photo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhotoExists(photo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdDestination"] = new SelectList(_context.Destination, "Id", "Nom", photo.IdDestination);
            return View(photo);
        }

        // GET: BackOffice/Photos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photo
                .Include(p => p.IdDestinationNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        // POST: BackOffice/Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var photo = await _context.Photo.FindAsync(id);
            _context.Photo.Remove(photo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhotoExists(int id)
        {
            return _context.Photo.Any(e => e.Id == id);
        }
    }
}

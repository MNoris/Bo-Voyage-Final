using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bo_Voyage_Final.Models;

namespace Bo_Voyage_Final.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class VoyagesController : Controller
    {
        private readonly BoVoyageContext _context;

        public VoyagesController(BoVoyageContext context)
        {
            _context = context;
        }

        // GET: Identity/Voyages
        public async Task<IActionResult> Index()
        {
            var boVoyageContext = _context.Voyage.Include(v => v.IdDestinationNavigation);
            return View(await boVoyageContext.ToListAsync());
        }

        // GET: Identity/Voyages/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Identity/Voyages/Create
        public IActionResult Create()
        {
            ViewData["IdDestination"] = new SelectList(_context.Destination, "Id", "Nom");
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdDestination"] = new SelectList(_context.Destination, "Id", "Nom", voyage.IdDestination);
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
                return RedirectToAction(nameof(Index));
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
            _context.Voyage.Remove(voyage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VoyageExists(int id)
        {
            return _context.Voyage.Any(e => e.Id == id);
        }
    }
}

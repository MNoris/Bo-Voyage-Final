﻿using System;
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
    public class DossierresasController : Controller
    {
        private readonly BoVoyageContext _context;
        private readonly UserManager<IdentityUser> _userManager;


        public DossierresasController(BoVoyageContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: Client/Dossierresas
        public async Task<IActionResult> Index()
        {
            var userEmail = _userManager.GetUserName(HttpContext.User);
            var user = _context.Personne.Where(p => p.Email == userEmail).AsNoTracking().SingleOrDefault();

            var userReservations = _context.Dossierresa.Include(d => d.IdClientNavigation)
                                                      .Include(d => d.IdEtatDossierNavigation)
                                                      .Include(d => d.IdVoyageNavigation)
                                                      .Include(d => d.IdVoyageNavigation.Voyageur)
                                                      .Include(d => d.IdVoyageNavigation.IdDestinationNavigation)
                                                      .Where(d => d.IdClient == user.Id);  
            return View(await userReservations.ToListAsync());
        }

        // GET: Client/Dossierresas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dossierresa = await _context.Dossierresa
                .Include(d => d.IdClientNavigation)
                .Include(d => d.IdEtatDossierNavigation)
                .Include(d => d.IdVoyageNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dossierresa == null)
            {
                return NotFound();
            }

            return View(dossierresa);
        }

        // GET: Client/Dossierresas/Create
        public IActionResult Create()
        {
            ViewData["IdClient"] = new SelectList(_context.Client, "Id", "Id");
            ViewData["IdEtatDossier"] = new SelectList(_context.Etatdossier, "Id", "Libelle");
            ViewData["IdVoyage"] = new SelectList(_context.Voyage, "Id", "Descriptif");
            return View();
        }

        // POST: Client/Dossierresas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NumeroCb,PrixTotal,IdClient,IdEtatDossier,IdVoyage")] Dossierresa dossierresa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dossierresa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdClient"] = new SelectList(_context.Client, "Id", "Id", dossierresa.IdClient);
            ViewData["IdEtatDossier"] = new SelectList(_context.Etatdossier, "Id", "Libelle", dossierresa.IdEtatDossier);
            ViewData["IdVoyage"] = new SelectList(_context.Voyage, "Id", "Descriptif", dossierresa.IdVoyage);
            return View(dossierresa);
        }

        // GET: Client/Dossierresas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dossierresa = await _context.Dossierresa.FindAsync(id);
            if (dossierresa == null)
            {
                return NotFound();
            }
            ViewData["IdClient"] = new SelectList(_context.Client, "Id", "Id", dossierresa.IdClient);
            ViewData["IdEtatDossier"] = new SelectList(_context.Etatdossier, "Id", "Libelle", dossierresa.IdEtatDossier);
            ViewData["IdVoyage"] = new SelectList(_context.Voyage, "Id", "Descriptif", dossierresa.IdVoyage);
            return View(dossierresa);
        }

        // POST: Client/Dossierresas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NumeroCb,PrixTotal,IdClient,IdEtatDossier,IdVoyage")] Dossierresa dossierresa)
        {
            if (id != dossierresa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dossierresa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DossierresaExists(dossierresa.Id))
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
            ViewData["IdClient"] = new SelectList(_context.Client, "Id", "Id", dossierresa.IdClient);
            ViewData["IdEtatDossier"] = new SelectList(_context.Etatdossier, "Id", "Libelle", dossierresa.IdEtatDossier);
            ViewData["IdVoyage"] = new SelectList(_context.Voyage, "Id", "Descriptif", dossierresa.IdVoyage);
            return View(dossierresa);
        }

        // GET: Client/Dossierresas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dossierresa = await _context.Dossierresa.Include(d => d.IdClientNavigation)
                                          .Include(d => d.IdEtatDossierNavigation)
                                          .Include(d => d.IdVoyageNavigation)
                                          .Include(d => d.IdVoyageNavigation.Voyageur)
                                          .Include(d => d.IdVoyageNavigation.IdDestinationNavigation)
                                          .FirstOrDefaultAsync(m => m.Id == id);

            if (dossierresa == null)
            {
                return NotFound();
            }

            return View(dossierresa);
        }

        // POST: Client/Dossierresas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dossierresa = await _context.Dossierresa.FindAsync(id);
            _context.Dossierresa.Remove(dossierresa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DossierresaExists(int id)
        {
            return _context.Dossierresa.Any(e => e.Id == id);
        }
    }
}

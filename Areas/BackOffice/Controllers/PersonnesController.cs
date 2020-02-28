using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bo_Voyage_Final.Models;
using System.Xml;

namespace Bo_Voyage_Final.Areas.BackOffice.Controllers
{
    [Area("BackOffice")]
    public class PersonnesController : Controller
    {
        private readonly BoVoyageContext _context;

        public PersonnesController(BoVoyageContext context)
        {
            _context = context;
        }

        // GET: BackOffice/Personnes
        public async Task<IActionResult> Index(string search, int idClient, int sexe, int age)
        {
            //stockage des valeurs des filtres
            ViewBag.Search = search;
            ViewBag.IdClient = idClient;
            ViewBag.Age = age;

            var dicSexe = new Dictionary<int, string>()
            { { 1, "Tous" },
                { 2, "Masculin" },
              { 3, "Feminin" }
             
             };
          ViewBag.Sexes = new SelectList(dicSexe, "Key", "Value", sexe = sexe == 0 ? 1 : sexe);



            IQueryable<Personne> reqClients = _context.Personne.Where(p => p.TypePers == 1);

            //filtrage par id
            if (idClient !=0)
            {
                reqClients = reqClients.Where(i=>i.Id==idClient);
            }


            //fitrage par morceau de nom
            if (!string.IsNullOrEmpty(search))
            {
                reqClients = reqClients.Where(e => e.Nom.Contains(search));
            }

            //fitrage par sexe
            if (sexe==2)
            {
            reqClients = reqClients.Where(s=>s.Civilite=="Mr");

            }
            if (sexe == 3)
            {
                reqClients = reqClients.Where(s => s.Civilite == "Mme");

            }

            //filtrage par  age
            if (age !=0)
            {
           var dateNaissanceMax =DateTime.Today.AddDays(-age*365.242);

                     
            reqClients = reqClients.Where((a => a.Datenaissance >= dateNaissanceMax));

            }  



            //creation de la liste
            List<Personne> listeClients =await reqClients.AsNoTracking().ToListAsync();

            return View(listeClients);
        }

        // GET: BackOffice/Personnes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personne = await _context.Personne
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personne == null)
            {
                return NotFound();
            }

            return View(personne);
        }



        public FileResult ExporterXml()
        {
            var clientsInfos = _context.Personne.Where(p => p.TypePers == 1);


            // Définit les paramètres pour l'indentation du flux xml généré
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";

            // Utilisation d'un XmlWriter avec les paramètres définis précédemment pour l'écriture du fichier
            using (XmlWriter writer = XmlWriter.Create(@"wwwroot/Files/ListeClients.xml", settings))
            {
                // Ecriture du prologue
                writer.WriteStartDocument();

                // Ecriture de l'élément racine
                writer.WriteStartElement("ListeClients");

                // Ecriture du contenu interne, avec une structure différente
               

                foreach (var client in clientsInfos)
                {
                    writer.WriteStartElement("Client");
                    writer.WriteAttributeString("Nom", client.Nom);
                    writer.WriteAttributeString("Prénom", client.Prenom);
                    writer.WriteAttributeString("E-mail", client.Email);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                // Ecriture de la balise fermante de l'élément racine et fin du document
                writer.WriteEndDocument();
            }


            string fileName = "ListeClients.xml";
            byte[] fileBytes = System.IO.File.ReadAllBytes($"wwwroot/Files/{fileName}");
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }





        // GET: BackOffice/Personnes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BackOffice/Personnes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TypePers,Civilite,Nom,Prenom,Email,Telephone,Datenaissance")] Personne personne)
        {
            if (ModelState.IsValid)
            {
                _context.Add(personne);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(personne);
        }

        // GET: BackOffice/Personnes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personne = await _context.Personne.FindAsync(id);
            if (personne == null)
            {
                return NotFound();
            }
            return View(personne);
        }

        // POST: BackOffice/Personnes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TypePers,Civilite,Nom,Prenom,Email,Telephone,Datenaissance")] Personne personne)
        {
            if (id != personne.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(personne);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonneExists(personne.Id))
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
            return View(personne);
        }

        // GET: BackOffice/Personnes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personne = await _context.Personne
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personne == null)
            {
                return NotFound();
            }

            return View(personne);
        }

        // POST: BackOffice/Personnes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var personne = await _context.Personne.FindAsync(id);
            _context.Personne.Remove(personne);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonneExists(int id)
        {
            return _context.Personne.Any(e => e.Id == id);
        }
    }
}

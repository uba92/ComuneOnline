using ComuneOnline.Data;
using ComuneOnline.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComuneOnline.Controllers
{
    public class CittadiniController : Controller
    {
        private readonly ComuneDbContext _context;
        private ILogger<CittadiniController> _logger;
        public CittadiniController(ComuneDbContext context, ILogger<CittadiniController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var cittadini = await _context.Cittadini.ToListAsync();

            _logger.LogInformation("Cittadini recuperati: " + cittadini.Count);

            foreach (var cittadino in cittadini)
            {
                _logger.LogInformation("Cittadno {Nome} {Cognome}", cittadino.Nome, cittadino.Cognome);
            }
            

            return View(cittadini);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cittadino cittadino)
        {
            if (ModelState.IsValid)
            {
                // Controllo se il cittadino esiste già
                var existingCittadino = await _context.Cittadini
                    .FirstOrDefaultAsync(c => c.Nome == cittadino.Nome && c.Cognome == cittadino.Cognome);

                if (existingCittadino != null)
                {
                    TempData["ErrorMessage"] = "Cittadino già esistente!";
                    return View(cittadino);
                }
                _context.Add(cittadino);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Cittadino creato con successo!";

                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Errore! Formato dati inseriti non corretti";
            return View(cittadino);


        }

        public IActionResult Edit(int id)
        {
            if(id<=0)
            {
                TempData["ErrorMessage"] = "ID non valido";
            }

            var cittadino = _context.Cittadini.Find(id);
            if (cittadino != null)
            {
                return View(cittadino);
            }
            else
            {
                TempData["ErrorMessage"] = "Errore! Cittadino non trovato";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Cittadino cittadino)
        {
            if(!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Errore! Formato dati inseriti non corretti";
                return View();
            }
            var cittadinoToUpdate = await _context.Cittadini.FindAsync(cittadino.CittadinoId);
            if (cittadinoToUpdate == null)
            {
                TempData["ErrorMessage"] = "Errore! Cittadino non trovato";
                return RedirectToAction(nameof(Index));
            }
            // Aggiorno i campi del cittadino
            cittadinoToUpdate.Nome = cittadino.Nome;
            cittadinoToUpdate.Cognome = cittadino.Cognome;
            cittadinoToUpdate.LuogoNascita = cittadino.LuogoNascita;
            cittadinoToUpdate.IndirizzoResidenza = cittadino.IndirizzoResidenza;
            cittadinoToUpdate.Email = cittadino.Email;
            cittadinoToUpdate.Telefono = cittadino.Telefono;
            
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Cittadino aggiornato con successo!";

            return RedirectToAction(nameof(Index));
        }
    }
}

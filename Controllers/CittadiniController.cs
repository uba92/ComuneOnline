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
    }
}

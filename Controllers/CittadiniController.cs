using AutoMapper;
using ComuneOnline.Data;
using ComuneOnline.Models.DTOs;
using ComuneOnline.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComuneOnline.Controllers
{
    public class CittadiniController : Controller
    {
        private readonly ComuneDbContext _context;
        private ILogger<CittadiniController> _logger;
        private readonly IMapper _mapper;

        public CittadiniController(ComuneDbContext context, ILogger<CittadiniController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
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
            if (id <= 0)
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
            if (!ModelState.IsValid)
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

        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "ID non valido";
                return RedirectToAction(nameof(Index));
            }
            var cittadino = await _context.Cittadini.AsNoTracking().Where(c => c.CittadinoId == id).FirstOrDefaultAsync();
            if (cittadino == null)
            {
                TempData["ErrorMessage"] = "Errore! Cittadino non trovato";
                return RedirectToAction(nameof(Index));
            }
            var cittadinoDetails = _mapper.Map<CittadinoDetailsDTO>(cittadino);
            return View(cittadinoDetails);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "ID non valido";
                return RedirectToAction(nameof(Index));
            }
            var cittadino = await _context.Cittadini.AsNoTracking().FirstOrDefaultAsync(c => c.CittadinoId == id);
            var cittadinoDTO = _mapper.Map<CittadinoDetailsDTO>(cittadino);
            if (cittadino == null)
            {
                TempData["ErrorMessage"] = "Errore! Cittadino non trovato";
                return RedirectToAction(nameof(Index));
            }
            return View(cittadinoDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "ID non valido";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var cittadino = await _context.Cittadini
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.CittadinoId == id);

                if (cittadino == null)
                {
                    TempData["ErrorMessage"] = "Errore! Cittadino non trovato";
                    return RedirectToAction(nameof(Index));
                }

                _context.Cittadini.Remove(cittadino);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Cittadino eliminato con successo!";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Errore durante l'eliminazione del cittadino.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}


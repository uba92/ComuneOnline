using ComuneOnline.Data;
using ComuneOnline.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComuneOnline.Controllers
{
    public class CittadiniController : Controller
    {
        private readonly ComuneDbContext _context;

        public CittadiniController(ComuneDbContext context)
        {
            _context = context;
        }

        //GET: Cittadini
        public async Task<IActionResult> Index()
        {
            var cittadini = await _context.Cittadini.ToListAsync();
            return View(cittadini);
        }

        //GET : /Cittadini/Create
        public IActionResult Create()
        {
            return View();
        }

        //POST: /Cittadini/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cittadino cittadino)
        { 
            if (ModelState.IsValid)
            {
                //check if citizens alredy exists
                var existingCittadino = await _context.Cittadini.FirstOrDefaultAsync(
                    c => c.Nome == cittadino.Nome
                    && c.Cognome == cittadino.Cognome && c.DataNascita == cittadino.DataNascita
                    && c.LuogoNascita == cittadino.LuogoNascita
                    && c.IndirizzoResidenza == cittadino.IndirizzoResidenza);

                if (existingCittadino != null)
                {
                    TempData["ErrorMessage"] = "Cittadino già esistente.";
                    return View(cittadino);
                }
                else
                {
                    cittadino.DataNascita = DateTime.SpecifyKind(cittadino.DataNascita, DateTimeKind.Utc);
                    _context.Cittadini.Add(cittadino);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cittadino creato con successo.";
                    return RedirectToAction(nameof(Index));
                }
            }
            TempData["ErrorMessage"] = "Errore durante la creazione del cittadino.";
            return View(cittadino);

        }
    }
}

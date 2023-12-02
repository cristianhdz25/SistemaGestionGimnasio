using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaGestionGimnasio.Models;

namespace SistemaGestionGimnasio.Controllers
{
    public class MembresiaController : Controller
    {
        private readonly Gym_BDContext _context;

        public MembresiaController(Gym_BDContext context)
        {
            _context = context;
        }

        // GET: Membresia
        public async Task<IActionResult> Index(string filtroCedula, DateTime? filtroFechaInicio, DateTime? filtroFechaFin)
        {
            if (HttpContext.Session.GetString("Usuario") == "" || HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            ViewBag.Usuario = HttpContext.Session.GetString("Usuario");
            ViewBag.Tipo = HttpContext.Session.GetInt32("Tipo");
            // Obtén todas las membresías incluyendo la información del cliente.
            var membresiasQuery = _context.Membresia.Include(m => m.ClienteCedulaNavigation).AsQueryable();

            // Aplica los filtros según sea necesario.
            if (!string.IsNullOrEmpty(filtroCedula))
            {
                membresiasQuery = membresiasQuery
    .Where(m => m.ClienteCedulaNavigation.ClienteCedula.StartsWith(filtroCedula) && !m.ClienteCedulaNavigation.Eliminado);
            }

            if (filtroFechaInicio.HasValue)
            {
                membresiasQuery = membresiasQuery.Where(m => m.FechaInicio >= filtroFechaInicio);
            }

            if (filtroFechaFin.HasValue)
            {
                membresiasQuery = membresiasQuery.Where(m => m.FechaVencimiento <= filtroFechaFin);
            }

            // Ejecuta la consulta y devuelve los resultados a la vista.
            var resultadosFiltrados = await membresiasQuery.ToListAsync();

            return View(resultadosFiltrados);
        }


        // GET: Membresia/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Membresia == null)
            {
                return NotFound();
            }

            var membresium = await _context.Membresia
                .Include(m => m.ClienteCedulaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (membresium == null)
            {
                return NotFound();
            }

            return View(membresium);
        }

        // GET: Membresia/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Usuario") == "" || HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            ViewBag.Usuario = HttpContext.Session.GetString("Usuario");
            ViewBag.Tipo = HttpContext.Session.GetInt32("Tipo");
            ViewData["ClienteCedula"] = new SelectList(_context.Clientes, "ClienteCedula", "ClienteCedula");
            return View();
        }

        // POST: Membresia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FechaInicio,FechaVencimiento,TipoMembresia,Precio,ClienteCedula")] Membresium membresium)
        {
            if (HttpContext.Session.GetString("Usuario") == "" || HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            ViewBag.Usuario = HttpContext.Session.GetString("Usuario");
            ViewBag.Tipo = HttpContext.Session.GetInt32("Tipo");
            if (ModelState.IsValid)
            {
                _context.Add(membresium);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteCedula"] = new SelectList(_context.Clientes, "ClienteCedula", "ClienteCedula", membresium.ClienteCedula);
            return View(membresium);
        }

        // GET: Membresia/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("Usuario") == "" || HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            ViewBag.Usuario = HttpContext.Session.GetString("Usuario");
            ViewBag.Tipo = HttpContext.Session.GetInt32("Tipo");
            if (id == null || _context.Membresia == null)
            {
                return NotFound();
            }

            var membresium = await _context.Membresia.FindAsync(id);
            if (membresium == null)
            {
                return NotFound();
            }
            ViewData["ClienteCedula"] = new SelectList(_context.Clientes, "ClienteCedula", "ClienteCedula", membresium.ClienteCedula);
            return View(membresium);
        }

        // POST: Membresia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaInicio,FechaVencimiento,TipoMembresia,Precio,ClienteCedula")] Membresium membresium)
        {
            if (HttpContext.Session.GetString("Usuario") == "" || HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            ViewBag.Usuario = HttpContext.Session.GetString("Usuario");
            ViewBag.Tipo = HttpContext.Session.GetInt32("Tipo");
            if (id != membresium.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(membresium);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MembresiumExists(membresium.Id))
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
            ViewData["ClienteCedula"] = new SelectList(_context.Clientes, "ClienteCedula", "ClienteCedula", membresium.ClienteCedula);
            return View(membresium);
        }

        // GET: Membresia/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Membresia == null)
            {
                return NotFound();
            }

            var membresium = await _context.Membresia
                .Include(m => m.ClienteCedulaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (membresium == null)
            {
                return NotFound();
            }

            return View(membresium);
        }

        // POST: Membresia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Membresia == null)
            {
                return Problem("Entity set 'Gym_BDContext.Membresia'  is null.");
            }
            var membresium = await _context.Membresia.FindAsync(id);
            if (membresium != null)
            {
                _context.Membresia.Remove(membresium);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MembresiumExists(int id)
        {
          return (_context.Membresia?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

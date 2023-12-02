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
    public class ClienteController : Controller
    {
        private readonly Gym_BDContext _context;

        public ClienteController(Gym_BDContext context)
        {
            _context = context;
        }

        // GET: Cliente
        public async Task<IActionResult> Index(string filtroCedula)
        {

            if (HttpContext.Session.GetString("Usuario") == "" || HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }

            ViewBag.Usuario = HttpContext.Session.GetString("Usuario");
            ViewBag.Tipo = HttpContext.Session.GetInt32("Tipo");

            var clientes = _context.Clientes.Where(c => c.Eliminado == false).AsQueryable();

            if (!string.IsNullOrEmpty(filtroCedula))
            {
                clientes = clientes.Where(c => c.ClienteCedula.StartsWith(filtroCedula));
            }

            if (TempData.ContainsKey("ErrorEliminar"))
            {
                ViewBag.ErrorEliminar = TempData["ErrorEliminar"].ToString();
            }

            return View(clientes.ToList());
        }

        // GET: Cliente/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (HttpContext.Session.GetString("Usuario") == "" || HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            if (id == null || _context.Clientes == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.ClienteCedula == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Cliente/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Usuario") == "" || HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }

            ViewBag.Usuario = HttpContext.Session.GetString("Usuario");
            ViewBag.Tipo = HttpContext.Session.GetInt32("Tipo");
            return View();
        }

        // POST: Cliente/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClienteCedula,Nombre,PrimerApellido,SegundoApellido,Teléfono,Activo,Eliminado")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                cliente.Eliminado = false;
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Cliente/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (HttpContext.Session.GetString("Usuario") == "" || HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            ViewBag.Usuario = HttpContext.Session.GetString("Usuario");
            ViewBag.Tipo = HttpContext.Session.GetInt32("Tipo");
            if (id == null || _context.Clientes == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Cliente/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ClienteCedula,Nombre,PrimerApellido,SegundoApellido,Teléfono,Activo,Eliminado")] Cliente cliente)
        {

            if (id != cliente.ClienteCedula)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.ClienteCedula))
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
            return View(cliente);
        }

        // GET: Cliente/Delete/5
        public async Task<IActionResult> DeleteView(string id)
        {
            if (id == null || _context.Clientes == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.ClienteCedula == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Cliente/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (HttpContext.Session.GetString("Usuario") == "" || HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            if (_context.Clientes == null)
            {
                return Problem("Entity set 'Gym_BDContext.Clientes'  is null.");
            }
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                // Verifica si el cliente tiene membresías asociadas
                var tieneMembresias = _context.Membresia.Any(m => m.ClienteCedula == cliente.ClienteCedula);

                if (tieneMembresias)
                {
                    // Almacena el mensaje de error en TempData
                    TempData["ErrorEliminar"] = "No se puede eliminar el cliente porque tiene membresías asociadas.";
                }
                else
                {
                    // Si no tiene membresías asociadas, procede con la eliminación
                    cliente.Eliminado = true;
                    _context.Clientes.Remove(cliente);

                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(string id)
        {
          return (_context.Clientes?.Any(e => e.ClienteCedula == id)).GetValueOrDefault();
        }
    }
}

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
    public class EmpleadoController : Controller
    {
        private readonly Gym_BDContext _context;

        public EmpleadoController(Gym_BDContext context)
        {
            _context = context;
        }

        // GET: Empleado
        public async Task<IActionResult> Index(string filtroCedula)
        {
            if (HttpContext.Session.GetString("Usuario") == "" || HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            ViewBag.Usuario = HttpContext.Session.GetString("Usuario");
            ViewBag.Tipo = HttpContext.Session.GetInt32("Tipo");

            var empleados = _context.Empleados.Where(c => c.Eliminado == false).AsQueryable();

            if (!string.IsNullOrEmpty(filtroCedula))
            {
                empleados = empleados.Where(e => e.EmpleadoCedula.StartsWith(filtroCedula));
            }

            return View(empleados.ToList());
        }

        // GET: Empleado/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (HttpContext.Session.GetString("Usuario") == "" || HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            ViewBag.Usuario = HttpContext.Session.GetString("Usuario");
            ViewBag.Tipo = HttpContext.Session.GetInt32("Tipo");
            if (id == null || _context.Empleados == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(m => m.EmpleadoCedula == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // GET: Empleado/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Empleado/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmpleadoCedula,Nombre,PrimerApellido,SegundoApellido,Teléfono,Edad,Eliminado,Salario,Horario")] Empleado empleado)
        {
            ViewBag.Usuario = HttpContext.Session.GetString("Usuario");
            ViewBag.Tipo = HttpContext.Session.GetInt32("Tipo");
            if (HttpContext.Session.GetString("Usuario") == "" || HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            if (ModelState.IsValid)
            {
                empleado.Eliminado = false;
                _context.Add(empleado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(empleado);
        }

        // GET: Empleado/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            ViewBag.Usuario = HttpContext.Session.GetString("Usuario");
            ViewBag.Tipo = HttpContext.Session.GetInt32("Tipo");
            if (HttpContext.Session.GetString("Usuario") == "" || HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            if (id == null || _context.Empleados == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }
            return View(empleado);
        }

        // POST: Empleado/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("EmpleadoCedula,Nombre,PrimerApellido,SegundoApellido,Teléfono,Edad,Eliminado,Salario,Horario")] Empleado empleado)
        {
            ViewBag.Usuario = HttpContext.Session.GetString("Usuario");
            ViewBag.Tipo = HttpContext.Session.GetInt32("Tipo");
            if (HttpContext.Session.GetString("Usuario") == "" || HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            if (id != empleado.EmpleadoCedula)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empleado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleadoExists(empleado.EmpleadoCedula))
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
            return View(empleado);
        }

        public async Task<IActionResult> Delete(string id)
        {
            ViewBag.Usuario = HttpContext.Session.GetString("Usuario");
            ViewBag.Tipo = HttpContext.Session.GetInt32("Tipo");
            if (HttpContext.Session.GetString("Usuario") == "" || HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            if (_context.Empleados == null)
            {
                return Problem("Entity set 'Gym_BDContext.Empleados'  is null.");
            }
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado != null)
            {
                empleado.Eliminado = true;
                _context.Empleados.Update(empleado);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpleadoExists(string id)
        {
          return (_context.Empleados?.Any(e => e.EmpleadoCedula == id)).GetValueOrDefault();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using SistemaGestionGimnasio.Models;

namespace SistemaGestionGimnasio.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SistemaGestionGimnasio.Models;
    using BCrypt.Net;
    using Microsoft.CodeAnalysis.Scripting;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;

    public class UsuarioController : Controller
    {
        private readonly Gym_BDContext _context;

        public UsuarioController(Gym_BDContext context)
        {
            _context = context;
        }

        // GET: /Auth/Login
        public IActionResult Index()
        {
            
            return View();
        }

        // POST: /Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Usuario model)
        {
            if (ModelState.IsValid)
            {
               
                var usuario = _context.Usuarios.FirstOrDefault(u => u.NombreUsuario == model.NombreUsuario);

                if (usuario != null)
                {
                    ViewBag.Error = "";
                    string hashedpassword = BCrypt.HashPassword(usuario.Contrasenia);

                    if (usuario != null && BCrypt.Verify(model.Contrasenia, hashedpassword))
                    {
                        // Si las credenciales son válidas, puedes realizar acciones adicionales si es necesario
                        
                        HttpContext.Session.SetString("Usuario", usuario.NombreUsuario);
                        HttpContext.Session.SetInt32("Tipo", usuario.TipoUsuario);

                        if (usuario.TipoUsuario == 1)
                        {
                            return RedirectToAction("IndexAdmin", "Home");
                        }
                        else if (usuario.TipoUsuario == 2)
                        {
                            return RedirectToAction("IndexEmpleado", "Home");
                        }

                    }

                    else
                    {
                        ViewBag.Error = 1;
                        return View(model);
                    }
                }
            }


            // Si llegamos aquí, algo salió mal, volvemos a mostrar el formulario de inicio de sesión con errores
            ViewBag.Error = 1;
            return View(model);
        }

        public async Task<IActionResult> CerrarSesion()
        {
            // Cierra la sesión actual
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString("Usuario", "");
            HttpContext.Session.SetInt32("Tipo", 0);
            // Redirige a la página de inicio de sesión u otra página según tus necesidades
            return RedirectToAction("Index", "Usuario");
        }
    }

}

using Microsoft.AspNetCore.Mvc;
using MyVirtualAcademy.Helper;
using MyVirtualAcademy.Models;
using MyVirtualAcademy.Repositories;

namespace MyVirtualAcademy.Controllers
{
    public class ManagedController : Controller
    {
        private RepositoryMyVirtualAcademy repo;

        public ManagedController(RepositoryMyVirtualAcademy repo)
        {
            this.repo = repo;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string nombre, string apellidos,
            string email, string password)
        {
            await this.repo.Register(nombre, apellidos, email, password);
            ViewData["MENSAJE"] = "Usuario registrado correctamente!!!";
            return View();
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(string email, string password)
        {
            Usuario user = await this.repo.LogInUserAsync(email, password);
            if (user != null)
            {
                string userRole = await this.repo.GetUserRoleAsync(user.IdUsuario);

                HttpContext.Session.SetInt32("IdUsuario", user.IdUsuario);
                HttpContext.Session.SetString("Imagen", user.FotoPerfil ?? string.Empty);
                HttpContext.Session.SetString("Nombre", user.Nombre + " " + user.Apellidos.Split(' ')[0]);
                HttpContext.Session.SetString("Rol", userRole);

                return RedirectToAction("Estudiante", "PersonalArea");
            }
            else
            {
                ViewData["MENSAJE"] = "Las credenciales son erroneas!!!";
                return View();
            }
        }

        public async Task<IActionResult> LogOut()
        {
            int? userId = SessionHelper.GetUserId(HttpContext);
            if (userId != null)
            {
                await this.repo.UpdateLastAccessAsync(userId.Value);
            }

            SessionHelper.Logout(HttpContext);
            return RedirectToAction("LogIn");
        }
    }
}

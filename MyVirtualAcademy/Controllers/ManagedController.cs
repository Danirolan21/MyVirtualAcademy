using Microsoft.AspNetCore.Mvc;
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
                return View(user);
            }
            else
            {
                ViewData["MENSAJE"] = "Las credenciales son erroneas!!!";
                return View();
            }
        }
    }
}

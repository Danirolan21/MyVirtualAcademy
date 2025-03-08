using Microsoft.AspNetCore.Mvc;
using MyVirtualAcademy.Helper;
using MyVirtualAcademy.Models;
using MyVirtualAcademy.Repositories;

namespace MyVirtualAcademy.Controllers
{
    public class ManagedController : Controller
    {
        private RepositoryMyVirtualAcademy repo;
        private HelperPathProvider helperPath;

        public ManagedController(RepositoryMyVirtualAcademy repo, HelperPathProvider helperPath)
        {
            this.repo = repo;
            this.helperPath = helperPath;
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

                return RedirectToAction("Index", "PersonalArea");
            }
            else
            {
                ViewData["MENSAJE"] = "Las credenciales son erroneas!!!";
                return View();
            }
        }

        public async Task<IActionResult> Perfil()
        {
            int? idUsuario = SessionHelper.GetUserId(HttpContext);
            Usuario usuario =
                await this.repo.FindUserAsync(idUsuario.Value);
            usuario.FotoPerfil = this.helperPath.MapUrlPath(usuario.FotoPerfil ?? "ProfileImage_Default.jpg", Folders.images);
            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Perfil(int idUsuario, string nombre, string apellidos, IFormFile fotoPerfil, string telefono)
        {
            string? FileName = null;
            if (fotoPerfil != null)
            {
                string extension = Path.GetExtension(fotoPerfil.FileName);
                string[] permittedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
                if (!permittedExtensions.Contains(extension.ToLower()))
                {
                    ViewData["MENSAJE"] = "Formato de imagen no permitido.";
                    return RedirectToAction("Perfil");
                }

                FileName = "User" + idUsuario + extension;
                string path = this.helperPath.MapPath(FileName, Folders.users);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await fotoPerfil.CopyToAsync(stream);
                }
            }

            Usuario usuario = await this.repo.UpdateUserAsync(idUsuario, nombre, apellidos, FileName, telefono);
            return View(usuario);
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

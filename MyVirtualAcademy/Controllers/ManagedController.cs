using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MyVirtualAcademy.Filters;
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

        [AuthorizeUsuarios(Policy = "AdminOnly")]
        public async Task<IActionResult> Register()
        {
            List<Rol> roles = await this.repo.GetRolesAsync();
            return View(roles);
        }

        [HttpPost]
        public async Task<IActionResult> Register(string nombre, string apellidos,
            string email, string password, int idRol)
        {
            await this.repo.Register(nombre, apellidos, email, password, idRol);
            ViewData["MENSAJE"] = "Usuario registrado correctamente!!!";

            List<Rol> roles = await this.repo.GetRolesAsync();
            return View(roles);
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
                string nombreCompleto = user.Nombre + " " + user.Apellidos.Split(' ')[0];

                ClaimsIdentity identity =
                    new ClaimsIdentity(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        ClaimTypes.Name, ClaimTypes.Role
                        );
                Claim claimName =
                    new Claim(ClaimTypes.Name, nombreCompleto);
                identity.AddClaim(claimName);
                Claim claimId =
                    new Claim(ClaimTypes.NameIdentifier,
                    user.IdUsuario.ToString());
                identity.AddClaim(claimId);
                Claim claimRol =
                    new Claim(ClaimTypes.Role, userRole);
                identity.AddClaim(claimRol);
                Claim claimFoto =
                    new Claim("FotoPerfil", user.FotoPerfil);
                identity.AddClaim(claimFoto);
                if(user.IdUsuario == 1)
                {
                    Claim claimAdmin =
                        new Claim("Admin", "El Admin Suprem");
                    identity.AddClaim(claimAdmin);
                }
                ClaimsPrincipal userPrincipal =
                    new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal
                    );
                string controller =
                    TempData["controller"].ToString();
                string action =
                    TempData["action"].ToString();
                if (TempData["id"] != null)
                {
                    string id = 
                        TempData["id"].ToString();
                    return RedirectToAction(action, controller
                        , new { id = id });
                }
                else
                {
                    return RedirectToAction(action, controller);
                }
            }
            else
            {
                ViewData["MENSAJE"] = "Las credenciales son erroneas!!!";
                return View();
            }
        }

        [AuthorizeUsuarios]
        public async Task<IActionResult> Perfil()
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Usuario usuario =
                await this.repo.FindUserAsync(usuarioId);
            usuario.FotoPerfil = this.helperPath.MapUrlPath(usuario.FotoPerfil, Folders.users);
            return View(usuario);
        }

        [AuthorizeUsuarios]
        public async Task<IActionResult> EditProfile()
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Usuario usuario =
                await this.repo.FindUserAsync(usuarioId);
            usuario.FotoPerfil = this.helperPath.MapUrlPath(usuario.FotoPerfil, Folders.users);
            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(int idUsuario, string nombre, string apellidos, IFormFile fotoPerfil, string telefono)
        {
            string? FileName = fotoPerfil.FileName;
            if (fotoPerfil != null)
            {
                string extension = Path.GetExtension(fotoPerfil.FileName);
                string[] permittedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
                if (!permittedExtensions.Contains(extension.ToLower()))
                {
                    ViewData["MENSAJE"] = "Formato de imagen no permitido.";
                    return RedirectToAction("Perfil");
                }

                //FileName = "User" + idUsuario + extension;
                string path = this.helperPath.MapPath(FileName, Folders.users);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await fotoPerfil.CopyToAsync(stream);
                }
            }

            Usuario usuario = await this.repo.UpdateUserAsync(idUsuario, nombre, apellidos, FileName, telefono);

            string userRole = await this.repo.GetUserRoleAsync(usuario.IdUsuario);
            string nombreCompleto = usuario.Nombre + " " + usuario.Apellidos.Split(' ')[0];

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            ClaimsIdentity identity = new ClaimsIdentity(
                CookieAuthenticationDefaults.AuthenticationScheme,
                ClaimTypes.Name, ClaimTypes.Role);

            identity.AddClaim(new Claim(ClaimTypes.Name, nombreCompleto));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Role, userRole));
            identity.AddClaim(new Claim("FotoPerfil", usuario.FotoPerfil ?? string.Empty));

            if (usuario.IdUsuario == 1)
            {
                identity.AddClaim(new Claim("Admin", "El Admin Suprem"));
            }

            ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                userPrincipal);

            return RedirectToAction("Perfil");
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync
                (CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}

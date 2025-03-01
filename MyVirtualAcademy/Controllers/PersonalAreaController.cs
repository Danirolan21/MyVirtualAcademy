using Microsoft.AspNetCore.Mvc;
using MyVirtualAcademy.Helper;
using MyVirtualAcademy.Models;
using MyVirtualAcademy.Repositories;

namespace MyVirtualAcademy.Controllers
{
    public class PersonalAreaController : Controller
    {
        private RepositoryMyVirtualAcademy repo;

        public PersonalAreaController(RepositoryMyVirtualAcademy repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            string userRole = HttpContext.Session.GetString("Rol");
            
            if (userRole != null)
            {
                if(userRole == "Profesor" || userRole == "Tutor")
                {
                    return RedirectToAction("Profesor");
                }
                else
                {
                    return RedirectToAction("Estudiante");
                }
            }
            else
            {
                return RedirectToAction("LogIn", "Managed");
            }
        }

        public IActionResult Profesor()
        {
            return View();
        }

        public async Task<IActionResult> Estudiante()
        {
            int? userId = SessionHelper.GetUserId(HttpContext);
            List<ViewAsignaturaUsuario> asignaturas =
                await this.repo.GetAsignaturasByUserAsync(userId.Value);
            return View(asignaturas);
        }
    }
}

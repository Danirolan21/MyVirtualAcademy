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
                else if (userRole == "Administrador")
                {
                    return RedirectToAction("Administrador");
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

        public async Task<IActionResult> Administrador()
        {
            List<VistaCursosDetalles> cursos =
                await this.repo.GetCursosDetallesAsync();
            return View(cursos);
        }
        public async Task<IActionResult> DetallesCurso(int idCurso)
        {
            VistaCursosDetalles detallesCurso = await this.repo.GetDetallesCursoAsync(idCurso);
            List<Asignatura> asignaturas = await this.repo.GetAsignaturasPorCursoAsync(idCurso);
            List<Usuario> alumnos = await this.repo.GetAlumnosPorCursoAsync(idCurso);
            return View(new ViewDetallesCursoModel
            {
                DetallesCurso = detallesCurso,
                Asignaturas = asignaturas,
                Alumnos = alumnos
            });
        }

        public async Task<IActionResult> DetallesAsignatura(int idAsignatura)
        {
            AsignaturaDetalleViewModel model = await this.repo.GetDetallesAsignatura(idAsignatura);

            if (model == null)
            {
                return null;
            }

            return View(model);
        }

        public IActionResult AñadirTema()
        {
            return View();
        }

        public async Task<IActionResult> CrearCurso()
        {
            List<VistaUsuariosConRoles> profesores = await this.repo.GetProfesoresAsync();
            ViewData["ESTADOS"] = new List<string> { "Borrador", "Activo", "Finalizado", "Archivado", "Suspendido" };
            return View(profesores);
        }

        [HttpPost]
        public async Task<IActionResult> CrearCurso(string nombre, string? descripcion, int idProfesor, DateTime fechaInicio, DateTime fechaFin, string estado, IFormFile imagenPortada)
        {
            await this.repo.CreateCourseAsync(nombre, descripcion, idProfesor, fechaInicio, fechaFin, estado, imagenPortada.FileName);
            return RedirectToAction("Administrador");
        }

        public async Task<IActionResult> Profesor()
        {
            int? userId = SessionHelper.GetUserId(HttpContext);
            Curso curso = await this.repo.GetCursoByProfesorAsync(userId.Value);
            return View(curso);
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

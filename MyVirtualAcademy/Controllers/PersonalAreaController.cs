using Microsoft.AspNetCore.Mvc;
using MyVirtualAcademy.Helper;
using MyVirtualAcademy.Models;
using MyVirtualAcademy.Repositories;

namespace MyVirtualAcademy.Controllers
{
    public class PersonalAreaController : Controller
    {
        private RepositoryMyVirtualAcademy repo;
        private HelperPathProvider helperPath;

        public PersonalAreaController(RepositoryMyVirtualAcademy repo, HelperPathProvider helperPath)
        {
            this.repo = repo;
            this.helperPath = helperPath;
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
            Curso curso = await this.repo.GetCursoPorAsignaturaAsync(idAsignatura);
            ViewData["IDCURSO"] = curso.IdCurso;

            if (model == null)
            {
                return null;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AñadirTema(Tema tema)
        {
            await this.repo.CreateTemaAsync(tema.IdAsignatura, tema.Nombre, tema.Orden);
            return RedirectToAction("DetallesAsignatura", new { idAsignatura = tema.IdAsignatura });
        }

        [HttpPost]
        public async Task<IActionResult> AñadirContenido(Contenido contenido, int IdAsignatura)
        {
            int idTema = contenido.IdTema;

            await this.repo.CreateContenidoAsync
                ( contenido.IdTema, contenido.Titulo, contenido.Tipo, contenido.UrlContenido
                 , contenido.Descripcion, contenido.Orden, contenido.FechaEntrega, contenido.PuntuacionMaxima);

            return RedirectToAction("DetallesAsignatura", new { idAsignatura = IdAsignatura });
        }


        public IActionResult GetFormularioContenido()
        {
            return PartialView("_FormularioContenido", new Contenido());
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
            string fileName = imagenPortada.FileName;
            string path =
                this.helperPath.MapPath(fileName, Folders.courses);
            using (Stream stream = new FileStream(path, FileMode.Create))
            {
                await imagenPortada.CopyToAsync(stream);
            }
            await this.repo.CreateCourseAsync(nombre, descripcion, idProfesor, fechaInicio, fechaFin, estado, imagenPortada.FileName);
            return RedirectToAction("Administrador");
        }

        public async Task<IActionResult> ObtenerDatosCurso(int idCurso)
        {
            VistaCursosDetalles curso = await this.repo.GetDetallesCursoAsync(idCurso);
            if (curso == null)
            {
                return NotFound();
            }

            List<VistaUsuariosConRoles> profesores = await this.repo.GetProfesoresAsync();

            var resultado = new
            {
                idCurso = curso.IdCurso,
                nombreCurso = curso.NombreCurso,
                descripcion = curso.Descripcion,
                idProfesor = curso.IdProfesor,
                idSuplente = curso.IdSuplente,
                profesores = profesores.Select(p => new {
                    idUsuario = p.IdUsuario,
                    nombre = p.Nombre,
                    apellidos = p.Apellidos
                }),
                fechaInicio = curso.FechaInicio,
                fechaFin = curso.FechaFin,
                estado = curso.Estado,
                imagenPortada = curso.ImagenPortada
            };

            return Json(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> EditarCurso(int idCurso, string nombre, string? descripcion,
            int idProfesor, int? idSuplente, DateTime fechaInicio, DateTime fechaFin, string estado, IFormFile? imagenPortada)
        {
            string fileName = null;

            if (imagenPortada != null)
            {
                fileName = imagenPortada.FileName;
                string path = this.helperPath.MapPath(fileName, Folders.courses);

                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    await imagenPortada.CopyToAsync(stream);
                }
            }

            bool result = await this.repo.UpdateCourseAsync(idCurso, nombre, descripcion,
                idProfesor, idSuplente, fechaInicio, fechaFin, estado, fileName);

            if (!result)
            {
                return Json(new { success = false, message = "No se encontró el curso" });
            }

            return Json(new { success = true });
        }

        public async Task<IActionResult> Profesor()
        {
            int? userId = SessionHelper.GetUserId(HttpContext);
            if (userId == null)
            {
                return RedirectToAction("LogIn", "Managed");
            }

            var viewModel = new ProfesorViewModel
            {
                Cursos = await this.repo.GetCursosByProfesorAsync(userId.Value),
                Asignaturas = await this.repo.GetAsignaturasByProfesorAsync(userId.Value)
            };

            return View(viewModel);
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

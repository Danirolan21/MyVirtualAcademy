using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyVirtualAcademy.Filters;
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

        [AuthorizeUsuarios]
        public IActionResult Index()
        {
            string userRole = User.FindFirstValue(ClaimTypes.Role);

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

        [AuthorizeUsuarios(Policy = "AdminOnly")]
        public async Task<IActionResult> Administrador()
        {
            List<VistaCursosDetalles> cursos =
                await this.repo.GetCursosDetallesAsync();
            return View(cursos);
        }

        [AuthorizeUsuarios(Policy = "AdminOnly")]
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


        [AuthorizeUsuarios(Policy = "AdminOnly")]
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
        public async Task<IActionResult> AñadirContenido(Contenido contenido, IFormFile ArchivoContenido, int idAsignatura)
        {
            if (ArchivoContenido != null && ArchivoContenido.Length > 0)
            {
                string fileName = ArchivoContenido.FileName;
                string uniqueFileName = $"{Guid.NewGuid()}_{fileName}";

                // Determinar la carpeta según el tipo
                string path = this.helperPath.MapPath(uniqueFileName, Folders.contents);

                // Guardar el archivo
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await ArchivoContenido.CopyToAsync(stream);
                }

                // Asignar la URL del contenido
                contenido.URLContenido = uniqueFileName;
            }

            Contenido nuevoContenido = await this.repo.CreateContenidoAsync
                (contenido.IdTema, contenido.Titulo, contenido.Tipo, contenido.URLContenido
                    , contenido.Descripcion, contenido.Orden, contenido.FechaEntrega, contenido.PuntuacionMaxima);

            // Si es examen o quiz, guardar datos adicionales
            if (contenido.Tipo == "Examen" || contenido.Tipo == "Quiz")
            {
                // Crear objeto para datos de examen
                Examen examen = new Examen
                {
                    IdContenido = nuevoContenido.IdContenido,
                    DuracionMinutos = int.Parse(Request.Form["DuracionMinutos"].ToString()),
                    FechaPublicacionNotas =
                        string.IsNullOrEmpty(Request.Form["FechaPublicacionNotas"]) ?
                        null : DateTime.Parse(Request.Form["FechaPublicacionNotas"]),
                    IntentosMaximos =
                        string.IsNullOrEmpty(Request.Form["IntentosMaximos"]) ?
                        1 : int.Parse(Request.Form["IntentosMaximos"]),
                    PenalizacionIncorrecta =
                        string.IsNullOrEmpty(Request.Form["PenalizacionIncorrecta"]) ?
                        0 : decimal.Parse(Request.Form["PenalizacionIncorrecta"])
                };

                await this.repo.CreateExamenAsync(examen.IdContenido, examen.DuracionMinutos.Value
                    , examen.FechaPublicacionNotas.Value, examen.IntentosMaximos, examen.PenalizacionIncorrecta);
            }

            // Redireccionar al detalle de la asignatura
            return RedirectToAction("DetallesAsignatura", new { idAsignatura = nuevoContenido.Tema.IdAsignatura });
        }


        [AuthorizeUsuarios(Policy = "AdminOnly")]
        public IActionResult GetFormularioContenido()
        {
            return PartialView("_FormularioContenido", new Contenido());
        }


        [AuthorizeUsuarios(Policy = "AdminOnly")]
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

        [AuthorizeUsuarios(Policy = "AdminOnly")]
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


        [AuthorizeUsuarios(Policy = "ProfesorUTutor")]
        public async Task<IActionResult> Profesor()
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var viewModel = new ProfesorViewModel
            {
                Cursos = await this.repo.GetCursosByProfesorAsync(usuarioId),
                Asignaturas = await this.repo.GetAsignaturasByProfesorAsync(usuarioId)
            };

            return View(viewModel);
        }

        [AuthorizeUsuarios]
        public async Task<IActionResult> Estudiante()
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<ViewAsignaturaUsuario> asignaturas =
                await this.repo.GetAsignaturasByUserAsync(usuarioId);
            return View(asignaturas);
        }

        #region METODOS CONTENIDOS
            public async Task<IActionResult> Detalle(int id)
            {
                var contenido = await this.repo.ObtenerContenidoPorIdAsync(id);
                if (contenido == null) return NotFound();

                var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (!await this.repo.UsuarioTieneAccesoAsync(id, usuarioId, User.IsInRole("Administrador")))
                    return Forbid();

                return contenido.Tipo switch
                {
                    "Examen" => RedirectToAction("DetalleExamen", new { id }),
                    "Quiz" => RedirectToAction("DetalleQuiz", new { id }),
                    "Tarea" => RedirectToAction("DetalleTarea", new { id }),
                    "Video" => RedirectToAction("DetalleVideo", new { id }),
                    "Documento" => RedirectToAction("DetalleDocumento", new { id }),
                    "Enlace" => RedirectToAction("DetalleEnlace", new { id }),
                    _ => NotFound()
                };
            }

            public async Task<IActionResult> DetalleTarea(int id)
            {
                var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var tarea = await this.repo.ObtenerTareaDetalleAsync(id, usuarioId);
        
                if (tarea == null)
                    return NotFound();
            
                return View(tarea);
            }

            [HttpPost]
            public async Task<IActionResult> Editar(int id, TareaEditViewModel model)
            {
                    // Procesar el archivo si se ha subido uno nuevo
                    string urlContenido = null;
                    if (model.ContenidoArchivo != null && model.ContenidoArchivo.Length > 0)
                    {
                        // Implementar lógica para guardar el archivo
                        urlContenido = await GuardarArchivoAsync(model.ContenidoArchivo, "tareas");
                    }

                    await this.repo.ActualizarTareaAsync(id, model, urlContenido);

                    TempData["Mensaje"] = "La tarea se ha actualizado correctamente.";
                    TempData["TipoMensaje"] = "success";

                return RedirectToAction("DetalleTarea", new { id });
            }

            private async Task<string> GuardarArchivoAsync(IFormFile archivo, string carpeta)
            {
                // Implementar lógica para guardar el archivo en el servidor o en la nube
                var nombreArchivo = $"{Guid.NewGuid()}_{Path.GetFileName(archivo.FileName)}";
                var rutaArchivo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "archivos", carpeta, nombreArchivo);
    
                Directory.CreateDirectory(Path.GetDirectoryName(rutaArchivo));
    
                using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                {
                    await archivo.CopyToAsync(stream);
                }
    
                return $"{nombreArchivo}";
            }

            [HttpPost]
            public async Task<IActionResult> Entregar(int id, string comentarioEstudiante, IFormFile archivoEntrega)
            {
                if (archivoEntrega == null)
                {
                    TempData["Error"] = "Debes adjuntar un archivo para realizar la entrega.";
                    return RedirectToAction("DetalleTarea", new { id });
                }

                var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                bool resultado = await this.repo.GuardarEntregaAsync(id, usuarioId, comentarioEstudiante, archivoEntrega);

                if (resultado)
                    TempData["Success"] = "Tu entrega se ha realizado correctamente.";
                else
                    TempData["Error"] = "Ha ocurrido un error al procesar tu entrega. Inténtalo de nuevo.";

                return RedirectToAction("DetalleTarea", new { id });
            }

            public async Task<IActionResult> DetalleRecurso(int id, string tipo)
            {
                var contenido = await this.repo.ObtenerContenidoPorIdAsync(id);
                if (contenido == null || contenido.Tipo != tipo) return NotFound();

                var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                await this.repo.RegistrarVisualizacionRecursoAsync(usuarioId, id);

                var viewModel = new RecursoViewModel
                {
                    IdContenido = contenido.IdContenido,
                    Titulo = contenido.Titulo,
                    Descripcion = contenido.Descripcion,
                    UrlContenido = contenido.URLContenido,
                    FechaPublicacion = contenido.FechaPublicacion,
                    Tipo = contenido.Tipo,
                    IdTema = contenido.Tema.IdTema,
                    NombreTema = contenido.Tema.Nombre,
                    IdAsignatura = contenido.Tema.Asignatura.IdAsignatura,
                    NombreAsignatura = contenido.Tema.Asignatura.Nombre
                };

                return View($"Detalle{tipo}", viewModel);
            }

            public async Task<IActionResult> DetalleVideo(int id) => await DetalleRecurso(id, "Video");
            public async Task<IActionResult> DetalleDocumento(int id) => await DetalleRecurso(id, "Documento");
            public async Task<IActionResult> DetalleEnlace(int id) => await DetalleRecurso(id, "Enlace");
        #endregion
    }
}

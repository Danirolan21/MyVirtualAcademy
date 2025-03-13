using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.EntityFrameworkCore;
using MyVirtualAcademy.Data;
using MyVirtualAcademy.Helper;
using MyVirtualAcademy.Models;

namespace MyVirtualAcademy.Repositories
{
    public class RepositoryMyVirtualAcademy
    {
        private MyVirtualAcademyContext context;
        private HelperPathProvider helperPath;

        public RepositoryMyVirtualAcademy(MyVirtualAcademyContext context, HelperPathProvider helperPath)
        {
            this.context = context;
            this.helperPath = helperPath;
        }

        #region MANAGED METHODS
        private async Task<int> GetMaxIdUserAsync()
        {
            if (this.context.Usuarios.Count() == 0)
            {
                return 1;
            }
            else
            {
                return await this.context.Usuarios.MaxAsync
                    (x => x.IdUsuario) + 1;
            }
        }

        private string GetIniciales(string nombre)
        {
            string iniciales = "";
            foreach (var palabra in nombre.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                iniciales += char.ToUpper(palabra[0]);
                if (iniciales.Length == 2) break;
            }
            return iniciales;
        }

        private byte[] GenerarAvatar(string iniciales)
        {
            int ancho = 150, alto = 150;
            Bitmap bitmap = new Bitmap(ancho, alto);

            Random random = new Random();
            Color colorFondo = Color.FromArgb(
                random.Next(10, 150),
                random.Next(10, 150),
                random.Next(10, 150)
            );

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(colorFondo);
                Font fuente = new Font("FavoritMno", 50, FontStyle.Bold);
                Brush textoBlanco = Brushes.White;
                SizeF tamano = g.MeasureString(iniciales, fuente);
                float x = (ancho - tamano.Width) / 2;
                float y = (alto - tamano.Height) / 2;
                g.DrawString(iniciales, fuente, textoBlanco, x, y);
            }

            using MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }

        public async Task Register(string nombre, string apellidos,
            string email, string password, int idRol)
        {
            Usuario user = new Usuario();
            user.IdUsuario = await GetMaxIdUserAsync();
            user.Nombre = nombre;
            user.Apellidos = apellidos;
            user.Email = email;
            user.Salt = HelperCryptography.GenerateSalt();
            user.Password_Hash = HelperCryptography.EncryptPassword(password, user.Salt);
            user.Password = password;
            user.FechaRegistro = DateTime.Now;

            // Generar avatar con iniciales
            string nombreCompleto = $"{nombre} {apellidos}";
            string iniciales = GetIniciales(nombreCompleto);
            byte[] avatarBytes = GenerarAvatar(iniciales);

            // Guardar el avatar como archivo
            string nombreAvatar = $"{user.IdUsuario}_{Guid.NewGuid()}.png";
            string rutaArchivo = this.helperPath.MapPath(nombreAvatar, Folders.users);
            System.IO.File.WriteAllBytes(rutaArchivo, avatarBytes);

            // Asignar URL del avatar al usuario
            user.FotoPerfil = nombreAvatar;

            this.context.Usuarios.Add(user);
            await this.context.SaveChangesAsync();

            // Asignar rol al usuario
            await AsignarRolUsuarioAsync(user.IdUsuario, idRol);
        }

        public async Task<Usuario> LogInUserAsync(string email, string password)
        {
            var consulta = from datos in this.context.Usuarios
                           where datos.Email == email
                           select datos;
            Usuario user = await consulta.FirstOrDefaultAsync();
            if (user == null)
            {
                return null;
            }
            else
            {
                string salt = user.Salt;
                byte[] temp =
                    HelperCryptography.EncryptPassword(password, salt);
                byte[] passBytes = user.Password_Hash;
                bool response =
                    HelperCryptography.CompararArrays(temp, passBytes);
                if (response == true)
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<Usuario> FindUserAsync(int idUsuario)
        {
            var consulta = from datos in this.context.Usuarios
                           where datos.IdUsuario == idUsuario
                           select datos;
            return await consulta.FirstOrDefaultAsync();
        }

        public async Task<string> GetUserRoleAsync(int idUsuario)
        {
            var consulta = from v in this.context.VistaUsuariosConRoles
                           where v.IdUsuario == idUsuario
                           select v.Rol;

            return await consulta.FirstOrDefaultAsync();
        }

        public async Task AsignarRolUsuarioAsync(int idUsuario, int idRol)
        {
            UsuarioRol usuarioRol = new UsuarioRol
            {
                IdUsuario = idUsuario,
                IdRol = idRol
            };

            this.context.UsuariosRoles.Add(usuarioRol);
            await this.context.SaveChangesAsync();
        }

        public async Task<List<Rol>> GetRolesAsync()
        {
            return await this.context.Roles.ToListAsync();
        }

        public async Task<Usuario> 
            UpdateUserAsync(int idUsuario, string nombre, string apellidos
            , string? fotoPerfil, string telefono)
        {
            Usuario user = await this.FindUserAsync(idUsuario);
            user.Nombre = nombre;
            user.Apellidos = apellidos;
            user.FotoPerfil = fotoPerfil ?? user.FotoPerfil;
            user.Telefono = telefono;
            await this.context.SaveChangesAsync();

            return user;
        }

        public async Task UpdateLastAccessAsync(int idUsuario)
        {
            Usuario user = await this.context.Usuarios.FindAsync(idUsuario);
            if (user != null)
            {
                user.UltimoAcceso = DateTime.Now;
                await this.context.SaveChangesAsync();
            }
        }
        #endregion

        #region AREA PERSONAL ADMIN

        public async Task<List<VistaCursosDetalles>> GetCursosDetallesAsync()
        {
            var consulta = from datos in this.context.VistaCursosDetalles
                           select datos;
            return await consulta.ToListAsync();
        }
        public async Task<VistaCursosDetalles> GetDetallesCursoAsync(int idCurso)
        {
            return await this.context.VistaCursosDetalles
                                 .FirstOrDefaultAsync(c => c.IdCurso == idCurso);
        }

        public async Task<List<Asignatura>> GetAsignaturasPorCursoAsync(int idCurso)
        {
            return await this.context.Asignaturas
                .Where(a => a.IdCurso == idCurso)
                .ToListAsync();
        }

        public async Task<List<Usuario>> GetAlumnosPorCursoAsync(int idCurso)
        {
            return await this.context.Usuarios
                .Where(u => this.context.Inscripciones.Any(i => i.IdCurso == idCurso && i.IdEstudiante == u.IdUsuario))
                .ToListAsync();
        }

        public async Task<List<VistaUsuariosConRoles>> GetProfesoresAsync()
        {
            return await this.context.VistaUsuariosConRoles
                .Where(u => u.Rol == "Profesor")
                .ToListAsync();
        }

        public async Task<AsignaturaDetalleViewModel> GetDetallesAsignatura(int idAsignatura)
        {
            var datos = await this.context.VistaDetallesAsignatura
                .Where(a => a.IdAsignatura == idAsignatura)
                .ToListAsync();

            if (!datos.Any())
                return null; //Si no hay datos significa que no hay profesores que imparta la asignatura

            var asignaturaDetalle = new AsignaturaDetalleViewModel();

            var primerRegistro = datos.First();
            asignaturaDetalle.IdAsignatura = primerRegistro.IdAsignatura;
            asignaturaDetalle.NombreAsignatura = primerRegistro.NombreAsignatura;

            asignaturaDetalle.Profesores = datos
                .GroupBy(p => p.IdProfesor)
                .Select(g => new ProfesorViewModel
                {
                    IdProfesor = g.Key,
                    NombreProfesor = g.First().NombreProfesor,
                    ApellidosProfesor = g.First().ApellidosProfesor,
                    FotoPerfil = g.First().FotoPerfil
                })
                .ToList();

            List<TemaViewModel> temas = datos
                .Where(t => t.IdTema.HasValue)
                .GroupBy(t => new { t.IdTema, t.NombreTema, t.OrdenTema })
                .Select(tg => new TemaViewModel
                {
                    IdTema = tg.Key.IdTema,
                    NombreTema = tg.Key.NombreTema,
                    OrdenTema = tg.Key.OrdenTema,

                    Contenidos = tg
                        .Where(c => c.IDContenido.HasValue)
                        .Select(c => new ContenidoViewModel
                        {
                            IdContenido = c.IDContenido,
                            TituloContenido = c.TituloContenido,
                            DescripcionContenido = c.DescripcionContenido,
                            TipoContenido = c.TipoContenido,
                            Orden_Contenido = c.OrdenContenido,
                            Contenido_Completado = c.ContenidoCompletado
                        })
                        .OrderBy(c => c.Orden_Contenido)
                        .ToList()
                })
                .OrderBy(t => t.OrdenTema)
                .ToList();

            asignaturaDetalle.Temas = temas;

            return asignaturaDetalle;
        }

        public async Task CreateCourseAsync(string nombre, string? descripcion
            , int idProfesor, DateTime fechaInicio, DateTime FechaFin, string Estado, string? imagenPortada)
        {
            Curso curso = new Curso();
            curso.IdCurso = await this.GetMaxIdCourseAsync();
            curso.Nombre = nombre;
            curso.Descripcion = descripcion;
            curso.IdProfesor = idProfesor;
            curso.FechaInicio = fechaInicio;
            curso.FechaFin = FechaFin;
            curso.Estado = Estado;
            curso.ImagenPortada = imagenPortada;
            await this.context.Cursos.AddAsync(curso);
            await this.context.SaveChangesAsync();
        }

        public async Task<bool> UpdateCourseAsync(int idCurso, string nombre, string? descripcion,
                int idProfesor, int? idSuplente, DateTime fechaInicio, DateTime fechaFin, string estado, string? imagenPortada)
        {
            Curso curso = await this.context.Cursos.FindAsync(idCurso);

            if (curso == null)
            {
                return false;
            }

            curso.Nombre = nombre;
            curso.Descripcion = descripcion;
            curso.IdProfesor = idProfesor;
            curso.IdSuplente = idSuplente;
            curso.FechaInicio = fechaInicio;
            curso.FechaFin = fechaFin;
            curso.Estado = estado;

            if (imagenPortada != null)
            {
                curso.ImagenPortada = imagenPortada;
            }

            await this.context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region AREA PERSONAL PROFESOR
        private async Task<int> GetMaxIdCourseAsync()
        {
            if (this.context.Cursos.Count() == 0)
            {
                return 1;
            }
            else
            {
                return await this.context.Cursos.MaxAsync
                    (x => x.IdCurso) + 1;
            }
        }

        private async Task<int> GetMaxIdTemaAsync()
        {
            if (this.context.Temas.Count() == 0)
            {
                return 1;
            }
            else
            {
                return await this.context.Temas.MaxAsync
                    (x => x.IdTema) + 1;
            }
        }

        public async Task CreateTemaAsync(int idAsignatura, string nombre, int Orden)
        {
            Tema tema = new Tema();
            tema.IdTema = await this.GetMaxIdTemaAsync();
            tema.IdAsignatura = idAsignatura;
            tema.Nombre = nombre;
            tema.Orden = Orden;
            await this.context.Temas.AddAsync(tema);
            await this.context.SaveChangesAsync();
        }

        private async Task<int> GetMaxIdContenidoAsync()
        {
            if (this.context.Contenidos.Count() == 0)
            {
                return 1;
            }
            else
            {
                return await this.context.Contenidos.MaxAsync(x => x.IdContenido) + 1;
            }
        }

        public async Task<Contenido> CreateContenidoAsync(int idTema, string titulo, string tipo, string urlContenido
            , string descripcion, int orden, DateTime? fechaEntrega = null, decimal? puntuacionMaxima = null)
        {
            Contenido contenido = new Contenido();
            contenido.IdContenido = await this.GetMaxIdContenidoAsync();
            contenido.IdTema = idTema;
            contenido.Titulo = titulo;
            contenido.Tipo = tipo;
            contenido.URLContenido = urlContenido;
            contenido.Descripcion = descripcion;
            contenido.Orden = orden;
            contenido.FechaPublicacion = DateTime.Now;

            if (tipo == "Tarea" || tipo == "Quiz" || tipo == "Examen")
            {
                contenido.FechaEntrega = fechaEntrega;
                contenido.PuntuacionMaxima = puntuacionMaxima;
            }

            await this.context.Contenidos.AddAsync(contenido);
            await this.context.SaveChangesAsync();
            return await this.ObtenerContenidoPorIdAsync(contenido.IdContenido);
        }

        public async Task CreateExamenAsync(int idContenido, int duracionMinutos, DateTime fechaPublicacionNotas
            , int numeroIntentos, decimal PenalizacionIncorrecta)
        {
            Examen examen = new Examen
            {
                IdContenido = idContenido,
                DuracionMinutos = duracionMinutos,
                FechaPublicacionNotas = fechaPublicacionNotas,
                IntentosMaximos = numeroIntentos,
                PenalizacionIncorrecta = PenalizacionIncorrecta
            };

            await this.context.Examenes.AddAsync(examen);
            await this.context.SaveChangesAsync();
        }

        public async Task<Curso> GetCursoPorAsignaturaAsync(int idAsignatura)
        {
            var asignatura = await this.context.Asignaturas
                .FirstOrDefaultAsync(a => a.IdAsignatura == idAsignatura);

            if (asignatura == null)
            {
                return null;
            }

            var curso = await this.context.Cursos
                .FirstOrDefaultAsync(c => c.IdCurso == asignatura.IdCurso);

            return curso;
        }

        public async Task<List<VistaCursosDetalles>> GetCursosByProfesorAsync(int idProfesor)
        {
            return await this.context.VistaCursosDetalles
                .Where(c => c.IdProfesor == idProfesor || c.IdSuplente == idProfesor)
                .ToListAsync();
        }

        public async Task<List<VistaAsignaturasProfesor>> GetAsignaturasByProfesorAsync(int idProfesor)
        {
            // Obtenemos primero los IDs de las asignaturas que imparte el profesor
            var asignaturasIds = await this.context.ProfesoresAsignaturas
                .Where(pa => pa.IdProfesor == idProfesor)
                .Select(pa => pa.IdAsignatura)
                .ToListAsync();

            // Agrupamos la información de VistaDetallesAsignatura para obtener datos básicos
            var asignaturas = await this.context.VistaDetallesAsignatura
                .Where(a => asignaturasIds.Contains(a.IdAsignatura))
                .GroupBy(a => new { a.IdAsignatura, a.NombreAsignatura })
                .Select(g => new VistaAsignaturasProfesor
                {
                    IdAsignatura = g.Key.IdAsignatura,
                    NombreAsignatura = g.Key.NombreAsignatura,
                    NumeroTemas = g.Select(a => a.IdTema).Where(id => id != null).Distinct().Count(),
                    NumeroContenidos = g.Select(a => a.IDContenido).Where(id => id != null).Distinct().Count()
                })
                .ToListAsync();

            // Obtener información de los cursos para cada asignatura
            foreach (var asignatura in asignaturas)
            {
                var cursoInfo = await this.context.Asignaturas
                    .Where(a => a.IdAsignatura == asignatura.IdAsignatura)
                    .Join(this.context.VistaCursosDetalles,
                          a => a.IdCurso,
                          c => c.IdCurso,
                          (a, c) => new {
                              IdCurso = c.IdCurso,
                              NombreCurso = c.NombreCurso,
                              Estado = c.Estado,
                              FechaInicio = c.FechaInicio,
                              FechaFin = c.FechaFin
                          })
                    .FirstOrDefaultAsync();

                if (cursoInfo != null)
                {
                    asignatura.IdCurso = cursoInfo.IdCurso;
                    asignatura.NombreCurso = cursoInfo.NombreCurso;
                    asignatura.Estado = cursoInfo.Estado;
                    asignatura.FechaInicio = cursoInfo.FechaInicio;
                    asignatura.FechaFin = cursoInfo.FechaFin;
                }
            }

            return asignaturas;
        }

        #endregion

        #region AREA PERSONAL ESTUDIANTE

        public async Task<List<ViewAsignaturaUsuario>> GetAsignaturasByUserAsync(int idUsuario)
        {
            return await this.context.VistaAsignaturasUsuario
                .Where(x => x.IdUsuario == idUsuario)
                .ToListAsync();
        }

        #endregion

        #region CONTENIDOS

        public async Task<Contenido> ObtenerContenidoPorIdAsync(int id)
        {
            return await this.context.Contenidos
                .Include(c => c.Tema)
                .ThenInclude(t => t.Asignatura)
                .FirstOrDefaultAsync(c => c.IdContenido == id);
        }

        public async Task<bool> UsuarioTieneAccesoAsync(int idContenido, int usuarioId, bool esAdmin)
        {
            if (esAdmin) return true;

            var contenido = await ObtenerContenidoPorIdAsync(idContenido);
            if (contenido == null) return false;

            if (contenido.Tema.Asignatura.Curso.IdProfesor == usuarioId ||
                contenido.Tema.Asignatura.Curso.IdSuplente == usuarioId)
                return true;

            bool esProfesorAsignatura = await this.context.ProfesoresAsignaturas
                .AnyAsync(pa => pa.IdAsignatura == contenido.Tema.IdAsignatura && pa.IdProfesor == usuarioId);

            if (esProfesorAsignatura) return true;

            bool estaInscrito = await this.context.Inscripciones
                .AnyAsync(i => i.IdCurso == contenido.Tema.Asignatura.IdCurso &&
                               i.IdEstudiante == usuarioId &&
                               i.Estado == "Activo");

            return estaInscrito;
        }

        public async Task RegistrarVisualizacionRecursoAsync(int idUsuario, int idContenido)
        {
            var cursoId = await this.context.Contenidos
                .Where(c => c.IdContenido == idContenido)
                .Select(c => c.Tema.Asignatura.IdCurso)
                .FirstOrDefaultAsync();

            if (cursoId == 0) return;

            var inscripcion = await this.context.Inscripciones
                .FirstOrDefaultAsync(i => i.IdCurso == cursoId && i.IdEstudiante == idUsuario);

            if (inscripcion == null) return;

            var progreso = await this.context.ProgresoInscripciones
                .FirstOrDefaultAsync(p => p.IdInscripcion == inscripcion.IdInscripcion && p.IdContenido == idContenido);

            if (progreso == null)
            {
                progreso = new ProgresoInscripcion
                {
                    IdInscripcion = inscripcion.IdInscripcion,
                    IdContenido = idContenido,
                    Completo = true,
                    FechaCompletado = DateTime.Now
                };

                this.context.ProgresoInscripciones.Add(progreso);
                await this.context.SaveChangesAsync();
            }
        }

        public async Task<int?> ObtenerCursoPorContenidoAsync(int idContenido)
        {
            return await this.context.Contenidos
                .Include(c => c.Tema)
                .ThenInclude(t => t.Asignatura)
                .Where(c => c.IdContenido == idContenido)
                .Select(c => c.Tema.Asignatura.IdCurso)
                .FirstOrDefaultAsync();
        }

        public async Task<Inscripcion> ObtenerInscripcionAsync(int cursoId, int usuarioId)
        {
            return await this.context.Inscripciones
                .FirstOrDefaultAsync(i => i.IdCurso == cursoId && i.IdEstudiante == usuarioId);
        }

        public async Task<ProgresoInscripcion> ObtenerProgresoAsync(int inscripcionId, int idContenido)
        {
            return await this.context.ProgresoInscripciones
                .FirstOrDefaultAsync(p => p.IdInscripcion == inscripcionId && p.IdContenido == idContenido);
        }

        public async Task RegistrarProgresoAsync(ProgresoInscripcion progreso)
        {
            this.context.ProgresoInscripciones.Add(progreso);
            await this.context.SaveChangesAsync();
        }

        public async Task GuardarCambiosAsync()
        {
            await this.context.SaveChangesAsync();
        }

        #endregion
    }
}

using Microsoft.EntityFrameworkCore;
using MyVirtualAcademy.Data;
using MyVirtualAcademy.Helper;
using MyVirtualAcademy.Models;

namespace MyVirtualAcademy.Repositories
{
    public class RepositoryMyVirtualAcademy
    {
        private MyVirtualAcademyContext context;

        public RepositoryMyVirtualAcademy(MyVirtualAcademyContext context)
        {
            this.context = context;
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

        public async Task Register(string nombre, string apellidos,
            string email, string password)
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
            this.context.Usuarios.Add(user);
            await this.context.SaveChangesAsync();
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

        public async Task CreateContenidoAsync(int idTema, string titulo, string tipo, string urlContenido
            , string descripcion, int orden, DateTime? fechaEntrega = null, decimal? puntuacionMaxima = null)
        {
            Contenido contenido = new Contenido();
            contenido.IdContenido = await this.GetMaxIdContenidoAsync();
            contenido.IdTema = idTema;
            contenido.Titulo = titulo;
            contenido.Tipo = tipo;
            contenido.UrlContenido = urlContenido;
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

        public async Task<Curso> GetCursoByProfesorAsync(int idProfesor)
        {
            return await this.context.Cursos
                .Where(x => x.IdProfesor == idProfesor)
                .FirstOrDefaultAsync();
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
    }
}

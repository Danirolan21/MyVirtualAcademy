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
                .Where(a => a.IdCuro == idCurso)
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

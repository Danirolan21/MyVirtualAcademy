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

        public async Task<string> GetUserRoleAsync(int idUsuario)
        {
            var consulta = from v in this.context.VistaUsuariosConRoles
                           where v.IdUsuario == idUsuario
                           select v.Rol;

            return await consulta.FirstOrDefaultAsync();
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

        #region AREA PERSONAL

        public async Task<List<ViewAsignaturaUsuario>> GetAsignaturasByUserAsync(int idUsuario)
        {
            return await this.context.VistaAsignaturasUsuario
                .Where(x => x.IdUsuario == idUsuario)
                .ToListAsync();
        }

        #endregion
    }
}

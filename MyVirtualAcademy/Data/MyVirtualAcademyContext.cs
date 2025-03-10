using Microsoft.EntityFrameworkCore;
using MyVirtualAcademy.Models;

namespace MyVirtualAcademy.Data
{
    public class MyVirtualAcademyContext : DbContext
    {
        public MyVirtualAcademyContext(DbContextOptions<MyVirtualAcademyContext> options)
            :base (options)
        { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Asignatura> Asignaturas { get; set; }
        public DbSet<Tema> Temas { get; set; }
        public DbSet<Contenido> Contenidos { get; set; }
        public DbSet<Inscripcion> Inscripciones { get; set; }
        public DbSet<VistaUsuariosConRoles> VistaUsuariosConRoles { get; set; }
        public DbSet<ViewAsignaturaUsuario> VistaAsignaturasUsuario { get; set; }
        public DbSet<VistaCursosDetalles> VistaCursosDetalles { get; set; }
        public DbSet<VistaDetallesAsignatura> VistaDetallesAsignatura { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VistaUsuariosConRoles>().HasNoKey().ToView("Vista_Usuarios_Con_Roles");
            modelBuilder.Entity<ViewAsignaturaUsuario>().HasNoKey().ToView("Vista_Asignaturas_Usuario");
            modelBuilder.Entity<VistaDetallesAsignatura>().HasNoKey().ToView("Vista_Detalles_Asignatura");
        }
    }
}

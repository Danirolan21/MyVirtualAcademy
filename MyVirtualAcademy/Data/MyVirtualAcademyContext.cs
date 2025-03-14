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
        public DbSet<UsuarioRol> UsuariosRoles { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Asignatura> Asignaturas { get; set; }
        public DbSet<ProfesorAsignatura> ProfesoresAsignaturas { get; set; }
        public DbSet<Tema> Temas { get; set; }
        public DbSet<Contenido> Contenidos { get; set; }
        public DbSet<EntregaTarea> EntregasTareas { get; set; }
        public DbSet<Examen> Examenes { get; set; }
        public DbSet<ExamenUsuario> ExamenesUsuarios { get; set; }
        public DbSet<Inscripcion> Inscripciones { get; set; }
        public DbSet<HistorialCalificacion> HistorialCalificaciones { get; set; }
        public DbSet<ComentarioCalificacion> ComentariosCalificaciones { get; set; }
        public DbSet<ProgresoInscripcion> ProgresoInscripciones { get; set; }
        public DbSet<VistaUsuariosConRoles> VistaUsuariosConRoles { get; set; }
        public DbSet<ViewAsignaturaUsuario> VistaAsignaturasUsuario { get; set; }
        public DbSet<VistaCursosDetalles> VistaCursosDetalles { get; set; }
        public DbSet<VistaDetallesAsignatura> VistaDetallesAsignatura { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsuarioRol>()
                .HasKey(ur => new { ur.IdUsuario, ur.IdRol });
            modelBuilder.Entity<ProfesorAsignatura>()
                .HasKey(pa => new { pa.IdAsignatura, pa.IdProfesor });
            modelBuilder.Entity<ProgresoInscripcion>()
                .HasKey(pi => new { pi.IdInscripcion, pi.IdContenido });
            modelBuilder.Entity<VistaUsuariosConRoles>().HasNoKey().ToView("Vista_Usuarios_Con_Roles");
            modelBuilder.Entity<ViewAsignaturaUsuario>().HasNoKey().ToView("Vista_Asignaturas_Usuario");
            modelBuilder.Entity<VistaDetallesAsignatura>().HasNoKey().ToView("Vista_Detalles_Asignatura");
        }
    }
}

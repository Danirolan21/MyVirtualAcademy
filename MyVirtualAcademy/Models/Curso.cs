using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyVirtualAcademy.Models
{
    [Table("Cursos")]
    public class Curso
    {
        [Key]
        [Column("ID_Curso")]
        public int IdCurso { get; set; }
        [Column("Nombre")]
        public string Nombre { get; set; }
        [Column("Descripcion")]
        public string? Descripcion { get; set; }
        [Column("ID_Profesor")]
        public int IdProfesor { get; set; }
        [Column("ID_Profesor_Suplente")]
        public int? IdSuplente { get; set; }
        [Column("Fecha_Inicio")]
        public DateTime FechaInicio { get; set; }
        [Column("Fecha_Fin")]
        public DateTime FechaFin { get; set; }
        [Column("Estado")]
        public string Estado { get; set; }
        [Column("Imagen_Portada")]
        public string? ImagenPortada { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyVirtualAcademy.Models
{
    [Table("Vista_Cursos_Detalles")]
    public class VistaCursosDetalles
    {
        [Key]
        [Column("ID_Curso")]
        public int IdCurso { get; set; }
        [Column("Nombre_Curso")]
        public string NombreCurso { get; set; }
        [Column("Descripcion")]
        public string? Descripcion { get; set; }
        [Column("ID_Profesor")]
        public int IdProfesor { get; set; }
        [Column("Nombre_Profesor")]
        public string NombreProfesor { get; set; }
        [Column("ID_Suplente")]
        public int? IdSuplente { get; set; }
        [Column("Nombre_Suplente")]
        public string? NombreSuplente { get; set; }
        [Column("Imagen_Portada")]
        public string? ImagenPortada { get; set; }
        [Column("Fecha_Inicio")]
        public DateTime FechaInicio { get; set; }
        [Column("Fecha_Fin")]
        public DateTime FechaFin { get; set; }
        [Column("Estado")]
        public string Estado { get; set; }
        [Column("Numero_Asignaturas")]
        public int NumeroAsignaturas { get; set; }
        [Column("Numero_Alumnos")]
        public int NumeroAlumnos { get; set; }
    }
}

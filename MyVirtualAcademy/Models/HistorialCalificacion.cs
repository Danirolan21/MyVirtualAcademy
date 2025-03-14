using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyVirtualAcademy.Models
{
    [Table("Historial_Calificaciones")]
    public class HistorialCalificacion
    {
        [Key]
        [Column("ID_Calificacion")]
        public int IdCalificacion { get; set; }
        [Column("ID_Estudiante")]
        public int IdEstudiante { get; set; }
        [ForeignKey("IdEstudiante")]
        public Usuario Estudiante { get; set; }
        [Column("ID_Contenido")]
        public int IdContenido { get; set; }
        [ForeignKey("IdContenido")]
        public Contenido Contenido { get; set; }
        [Column("Calificacion")]
        public decimal Calificacion { get; set; }
        [Column("Fecha_Calificacion")]
        public DateTime FechaCalificacion { get; set; } = DateTime.Now;
        [Column("ID_Profesor_Calificador")]
        public int? IdProfesorCalificador { get; set; }
        [ForeignKey("IdProfesorCalificador")]
        public Usuario ProfesorCalificador { get; set; }
    }
}

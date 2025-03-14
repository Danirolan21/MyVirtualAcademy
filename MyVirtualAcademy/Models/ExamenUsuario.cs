using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyVirtualAcademy.Models
{
    [Table("Examenes_Usuarios")]
    public class ExamenUsuario
    {
        [Key]
        [Column("ID_Intento")]
        public int IdIntento { get; set; }
        [Column("ID_Contenido")]
        public int IdContenido { get; set; }
        [Column("ID_Estudiante")]
        public int IdEstudiante { get; set; }
        [Column("Fecha_Inicio")]
        public DateTime FechaInicio { get; set; }
        [Column("Fecha_Fin")]
        public DateTime FechaFin { get; set; }
        [Column("Tiempo_Excedido")]
        public DateTime? TiempoExcedido { get; set; }
        [ForeignKey("IdEstudiante")]
        public Usuario Estudiante { get; set; }
        [ForeignKey("IdContenido")]
        public Contenido Contenido { get; set; }
    }
}

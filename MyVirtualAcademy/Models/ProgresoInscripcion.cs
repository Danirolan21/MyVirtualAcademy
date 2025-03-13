using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyVirtualAcademy.Models
{
    [Table("PROGRESO_INSCRIPCIONES")]
    public class ProgresoInscripcion
    {
        [Column("ID_Inscripcion")]
        public int IdInscripcion { get; set; }

        [ForeignKey("IdInscripcion")]
        public Inscripcion Inscripcion { get; set; }

        [Column("ID_Contenido")]
        public int IdContenido { get; set; }

        [ForeignKey("IdContenido")]
        public Contenido Contenido { get; set; }

        [Column("Completo")]
        public bool Completo { get; set; } = false;

        [Column("Fecha_Completado")]
        public DateTime? FechaCompletado { get; set; }
    }
}

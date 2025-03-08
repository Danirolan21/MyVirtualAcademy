using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyVirtualAcademy.Models
{
    [Table("INSCRIPCIONES")]
    public class Inscripcion
    {
        [Key]
        [Column("ID_Inscripcion")]
        public int IdInscripcion { get; set; }
        [Column("ID_Estudiante")]
        public int IdEstudiante { get; set; }
        [Column("ID_Curso")]
        public int IdCurso { get; set; }
        [Column("Fecha_Inscripcion")]
        public DateTime? FechaInscripcion { get; set; }
        [Column("Estado")]
        public string Estado { get; set; }
        [Column("Rol_Inscrito")]
        public string RolInscrito { get; set; }
        [Column("Porcentaje_Completado")]
        public decimal PorcentajeCompletado { get; set; }
    }

}

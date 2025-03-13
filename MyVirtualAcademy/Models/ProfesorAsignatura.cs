using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyVirtualAcademy.Models
{
    [Table("Profesores_Asignaturas")]
    public class ProfesorAsignatura
    {
        [Column("ID_Asignatura")]
        public int IdAsignatura { get; set; }

        [ForeignKey("IdAsignatura")]
        public Asignatura Asignatura { get; set; }

        [Column("ID_Profesor")]
        public int IdProfesor { get; set; }

        [ForeignKey("IdProfesor")]
        public Usuario Profesor { get; set; }
    }
}

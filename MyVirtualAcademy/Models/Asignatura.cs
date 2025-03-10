using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyVirtualAcademy.Models
{
    [Table("ASIGNATURAS")]
    public class Asignatura
    {
        [Key]
        [Column("ID_Asignatura")]
        public int IdAsignatura { get; set; }
        [Column("ID_Curso")]
        public int IdCurso { get; set; }
        [Column("Nombre")]
        public string Nombre { get; set; }
    }
}

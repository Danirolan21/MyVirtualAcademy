using System.ComponentModel.DataAnnotations.Schema;

namespace MyVirtualAcademy.Models
{
    [Table("Vista_Asignaturas_Usuario")]
    public class ViewAsignaturaUsuario
    {
        [Column("ID_Usuario")]
        public int IdUsuario { get; set; }
        [Column("NombreUsuario")]
        public string NombreUsuario { get; set; }
        [Column("ID_Curso")]
        public int IdCurso { get; set; }
        [Column("NombreCurso")]
        public string NombreCurso { get; set; }
        [Column("ID_Asignatura")]
        public int IdAsignatura { get; set; }
        [Column("NombreAsignatura")]
        public string NombreAsignatura { get; set; }
    }
}

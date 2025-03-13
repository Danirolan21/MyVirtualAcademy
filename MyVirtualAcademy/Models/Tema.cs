using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyVirtualAcademy.Models
{
    [Table("TEMAS")]
    public class Tema
    {
        [Key]
        [Column("ID_Tema")]
        public int IdTema { get; set; }
        [Column("ID_Asignatura")]
        public int IdAsignatura { get; set; }
        [ForeignKey("IdAsignatura")]
        public Asignatura Asignatura { get; set; }
        [Column("Nombre")]
        public string Nombre { get; set; }
        [Column("Orden")]
        public int Orden { get; set; }

        public List<Contenido> Contenidos { get; set; }
    }

}

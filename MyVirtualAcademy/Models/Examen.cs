using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyVirtualAcademy.Models
{
    [Table("EXAMENES")]
    public class Examen
    {
        [Key]
        [Column("ID_Contenido")]
        public int IdContenido { get; set; }

        [ForeignKey("IdContenido")]
        public Contenido Contenido { get; set; }

        [Column("Duracion_Minutos")]
        public int? DuracionMinutos { get; set; }

        [Column("Fecha_Publicacion_Notas")]
        public DateTime? FechaPublicacionNotas { get; set; }

        [Column("Intentos_Maximos")]
        public int IntentosMaximos { get; set; } = 1;

        [Column("Penalizacion_Incorrecta")]
        public decimal PenalizacionIncorrecta { get; set; } = 0;
    }
}

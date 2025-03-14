using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyVirtualAcademy.Models
{
    [Table("Entregas_tareas")]
    public class EntregaTarea
    {
        [Key]
        [Column("ID_Entrega")]
        public int IdEntrega { get; set; }
        [Column("ID_Contenido")]
        public int IdContenido { get; set; }
        [Column("ID_Estudiante")]
        public int IdEstudiante { get; set; }
        [Column("URL_Entrega")]
        public string URLEntrega { get; set; }
        [Column("Fecha_Entrega")]
        public DateTime FechaEntrega { get; set; }
        [Column("Estado")]
        public string Estado { get; set; }
        [ForeignKey("IdContenido")]
        public Contenido Contenido { get; set; }
        [ForeignKey("IdEstudiante")]
        public Usuario Usuario { get; set; }

    }
}

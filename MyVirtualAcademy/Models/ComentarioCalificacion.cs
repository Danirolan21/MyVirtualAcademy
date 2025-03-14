using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyVirtualAcademy.Models
{
    [Table("Comentarios_Calificaciones")]
    public class ComentarioCalificacion
    {
        [Key]
        [Column("ID_Comentario")]
        public int IdComentario { get; set; }
        [Column("ID_Calificacion")]
        public int IdCalificacion { get; set; }
        [ForeignKey("IdCalificacion")]
        public HistorialCalificacion Calificacion { get; set; }
        [Column("ID_Autor")]
        public int IdAutor { get; set; }
        [ForeignKey("IdAutor")]
        public Usuario Autor { get; set; }
        [Column("Comentario")]
        public string Comentario { get; set; }
        [Column("Fecha_Comentario")]
        public DateTime FechaComentario { get; set; } = DateTime.Now;
    }
}

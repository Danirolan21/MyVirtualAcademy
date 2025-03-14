using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyVirtualAcademy.Models
{
    [Table("CONTENIDOS")]
    public class Contenido
    {
        [Key]
        [Column("ID_Contenido")]
        public int IdContenido { get; set; }
        [Column("ID_Tema")]
        public int IdTema { get; set; }
        [ForeignKey("IdTema")]
        public Tema Tema { get; set; }
        [Column("Titulo")]
        public string Titulo { get; set; }
        [Column("Tipo")]
        public string Tipo { get; set; }
        [Column("URL_Contenido")]
        public string URLContenido { get; set; }
        [Column("Descripcion")]
        public string Descripcion { get; set; }
        [Column("Orden")]
        public int Orden { get; set; }
        [Column("Fecha_Publicacion")]
        public DateTime FechaPublicacion { get; set; } = DateTime.Now;
        [Column("Fecha_Entrega")]
        public DateTime? FechaEntrega { get; set; }
        [Column("Puntuacion_Maxima")]
        public decimal? PuntuacionMaxima { get; set; }
    }

}

using System.ComponentModel.DataAnnotations.Schema;

namespace MyVirtualAcademy.Models
{
    public class ContenidoViewModel
    {
        public int? IdContenido { get; set; }
        public string? TituloContenido { get; set; }
        public string? DescripcionContenido { get; set; }
        public string? TipoContenido { get; set; }
        public int? Orden_Contenido { get; set; }
        public bool? Contenido_Completado { get; set; }
    }
}

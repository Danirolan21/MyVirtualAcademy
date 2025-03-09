using System.ComponentModel.DataAnnotations.Schema;

namespace MyVirtualAcademy.Models
{
    public class TemaViewModel
    {
        public int? IdTema { get; set; }
        public string? NombreTema { get; set; }
        public int? OrdenTema { get; set; }
        public List<ContenidoViewModel> Contenidos { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace MyVirtualAcademy.Models
{
    public class TareaEditViewModel
    {
        [Required(ErrorMessage = "El título es obligatorio")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La fecha de entrega es obligatoria")]
        public DateTime FechaEntrega { get; set; }

        [Required(ErrorMessage = "La puntuación máxima es obligatoria")]
        [Range(0, 100, ErrorMessage = "La puntuación debe estar entre 0 y 100")]
        public decimal PuntuacionMaxima { get; set; }

        public bool PermiteEntregaTardia { get; set; }

        public IFormFile ContenidoArchivo { get; set; }
    }
}

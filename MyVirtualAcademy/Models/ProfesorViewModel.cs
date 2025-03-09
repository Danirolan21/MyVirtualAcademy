using System.ComponentModel.DataAnnotations.Schema;

namespace MyVirtualAcademy.Models
{
    public class ProfesorViewModel
    {
        public int IdProfesor { get; set; }
        public string NombreProfesor { get; set; }
        public string ApellidosProfesor { get; set; }
        public string? FotoPerfil { get; set; }
    }
}

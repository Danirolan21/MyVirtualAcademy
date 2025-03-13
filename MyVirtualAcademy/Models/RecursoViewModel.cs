namespace MyVirtualAcademy.Models
{
    public class RecursoViewModel
    {
        public int IdContenido { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string UrlContenido { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public string Tipo { get; set; }
        public string NombreTema { get; set; }
        public int IdTema { get; set; }
        public string NombreAsignatura { get; set; }
        public int IdAsignatura { get; set; }
    }
}

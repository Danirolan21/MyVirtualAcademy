namespace MyVirtualAcademy.Models
{
    public class TareaViewModel
    {
        public int IdContenido { get; set; }
        public int IdAsignatura { get; set; }
        public string NombreAsignatura { get; set; }
        public int IdTema { get; set; }
        public string NombreTema { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string UrlContenido { get; set; }
        public DateTime FechaEntrega { get; set; }
        public decimal PuntuacionMaxima { get; set; }
        public decimal PuntuacionAprobado { get; set; }
        public bool PermiteEntregaTardia { get; set; }
        public EntregaTareaViewModel Entrega { get; set; }
    }
}

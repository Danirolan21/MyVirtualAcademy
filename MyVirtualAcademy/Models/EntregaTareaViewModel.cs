namespace MyVirtualAcademy.Models
{
    public class EntregaTareaViewModel
    {
        public int IdEntrega { get; set; }
        public int IdEstudiante { get; set; }
        public string NombreEstudiante { get; set; }
        public string URLEntrega { get; set; }
        public DateTime FechaEntrega { get; set; }
        public string Estado { get; set; }
        public decimal? Calificacion { get; set; }
        public string Comentarios { get; set; }
    }
}

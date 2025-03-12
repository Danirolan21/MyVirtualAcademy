namespace MyVirtualAcademy.Models
{
    public class VistaAsignaturasProfesor
    {
        public int IdAsignatura { get; set; }
        public string NombreAsignatura { get; set; }
        public int IdCurso { get; set; }
        public string NombreCurso { get; set; }
        public string Estado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int NumeroTemas { get; set; }
        public int NumeroContenidos { get; set; }
    }
}

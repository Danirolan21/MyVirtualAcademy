namespace MyVirtualAcademy.Models
{
    public class AsignaturaUsuarioDTO
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public int IdCurso { get; set; }
        public string NombreCurso { get; set; }
        public int IdAsignatura { get; set; }
        public string NombreAsignatura { get; set; }
        public string NombreProfesor { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; }
        public int NumeroTemas { get; set; }
        public decimal Progreso { get; set; }
    }

}

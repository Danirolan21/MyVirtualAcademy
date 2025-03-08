namespace MyVirtualAcademy.Models
{
    public class ViewDetallesCursoModel
    {
        public VistaCursosDetalles DetallesCurso { get; set; }
        public List<Asignatura> Asignaturas { get; set; }
        public List<Usuario> Alumnos { get; set; }
    }
}

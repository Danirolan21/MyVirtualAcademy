using System.ComponentModel.DataAnnotations.Schema;

namespace MyVirtualAcademy.Models
{
    [Table("Vista_Detalles_Asignatura")]
    public class VistaDetallesAsignatura
    {
        [Column("ID_Asignatura")]
        public int IdAsignatura { get; set; }

        [Column("Nombre_Asignatura")]
        public string NombreAsignatura { get; set; }

        [Column("ID_Profesor")]
        public int IdProfesor { get; set; }

        [Column("Nombre_Profesor")]
        public string NombreProfesor { get; set; }

        [Column("Apellidos_Profesor")]
        public string ApellidosProfesor { get; set; }

        [Column("Foto_Perfil")]
        public string? FotoPerfil { get; set; }

        [Column("ID_Tema")]
        public int? IdTema { get; set; }

        [Column("Nombre_Tema")]
        public string? NombreTema { get; set; }

        [Column("Orden_Tema")]
        public int? OrdenTema { get; set; }

        [Column("ID_Contenido")]
        public int? IDContenido { get; set; }

        [Column("Titulo_Contenido")]
        public string? TituloContenido { get; set; }

        [Column("Descripcion_Contenido")]
        public string? DescripcionContenido { get; set; }

        [Column("Tipo_Contenido")]
        public string? TipoContenido { get; set; }

        [Column("Orden_Contenido")]
        public int? OrdenContenido { get; set; }

        [Column("Contenido_Completado")]
        public bool? ContenidoCompletado { get; set; }
    }

}

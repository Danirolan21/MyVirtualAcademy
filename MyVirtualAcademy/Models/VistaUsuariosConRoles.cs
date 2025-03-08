using System.ComponentModel.DataAnnotations.Schema;

namespace MyVirtualAcademy.Models
{
    [Table("Vista_Usuarios_Con_Roles")]
    public class VistaUsuariosConRoles
    {
        [Column("ID_Usuario")]
        public int IdUsuario { get; set; }
        [Column("Nombre")]
        public string Nombre { get; set; }
        [Column("Apellidos")]
        public string Apellidos { get; set; }
        [Column("Email")]
        public string Email { get; set; }
        [Column("Rol")]
        public string Rol { get; set; }
        [Column("Activo")]
        public bool Activo { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace MyVirtualAcademy.Models
{
    [Table("Vista_Usuarios_Con_Roles")]
    public class VistaUsuariosConRoles
    {
        [Column("ID_Usuario")]
        public int IdUsuario { get; set; }
        [Column("Rol")]
        public string Rol { get; set; }
    }
}

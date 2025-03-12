using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyVirtualAcademy.Models
{
    [Table("USUARIOS_ROLES")]
    public class UsuarioRol
    {
        [Column("ID_Usuario")]
        public int IdUsuario { get; set; }
        [Column("ID_Rol")]
        public int IdRol { get; set; }
    }
}

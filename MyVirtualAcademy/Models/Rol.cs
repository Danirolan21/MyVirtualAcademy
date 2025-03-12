using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyVirtualAcademy.Models
{
    [Table("ROLES")]
    public class Rol
    {
        [Key]
        [Column("ID_Rol")]
        public int IdRol { get; set; }
        [Column("Nombre")]
        public string Nombre { get; set; }
    }
}

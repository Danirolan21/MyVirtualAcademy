using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyVirtualAcademy.Models
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        [Column("ID_Usuario")]
        public int IdUsuario { get; set; }
        [Column("Nombre")]
        public string Nombre { get; set; }
        [Column("Apellidos")]
        public string Apellidos { get; set; }
        [Column("Email")]
        public string Email { get; set; }
        [Column("Pass_Hash")]
        public byte[] Password_Hash { get; set; }
        [Column("Salt")]
        public string Salt { get; set; }
        [Column("Pass")]
        public string Password { get; set; }
        [Column("Fecha_Registro")]
        public DateTime FechaRegistro { get; set; }
        [Column("Activo")]
        public bool Activo { get; set; }
        [Column("Ultimo_Acceso")]
        public DateTime? UltimoAcceso { get; set; }
        [Column("Foto_Perfil")]
        public string? FotoPerfil { get; set; }
        [Column("Telefono")]
        public string? Telefono { get; set; }
        [Column("Token_Recuperacion")]
        public string? TokenRecuperacion { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace EmprestimoFerramentas.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [DataType("nvarchar")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [MaxLength(20, ErrorMessage = "Este campo deve conter entre 3 e 20 caracteres!")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 e 20 caracteres!")]
        public string NomeUsuario { get; set; }

        [DataType("nvarchar")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [MaxLength(20, ErrorMessage = "Este campo deve conter entre 3 e 20 caracteres!")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 e 20 caracteres!")]
        public string Senha { get; set; }

        public string Perfil { get; set; }
    }
}
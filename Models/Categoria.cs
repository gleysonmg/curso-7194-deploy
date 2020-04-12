using System.ComponentModel.DataAnnotations;

namespace EmprestimoFerramentas.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [DataType("nvarchar")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [MaxLength(60, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres!")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres!")]
        public string NomeCategoria { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace EmprestimoFerramentas.Models
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }

        [DataType("nvarchar")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [MaxLength(60, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres!")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres!")]
        public string NomeProduto { get; set; }

        [MaxLength(1024, ErrorMessage = "Este campo deve ter no máximo 1024 caracteres!")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [Range(1, int.MaxValue, ErrorMessage = "O preço deve ser maior que zero!")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [Range(1, int.MaxValue, ErrorMessage = "Categoria inválida")]
        public int CategoriaId { get; set; }

        public Categoria Categoria { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace GestaoLoja.Entities
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Indique o nome da categoria.")]
        [StringLength(100, ErrorMessage = "O nome não pode ter mais de {1} caracteres.")]
        public string Nome { get; set; } = string.Empty;

        public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
    }
}

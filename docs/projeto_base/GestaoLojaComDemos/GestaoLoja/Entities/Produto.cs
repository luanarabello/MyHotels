using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoLoja.Entities
{
    public class Produto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Indique o nome do produto.")]
        [StringLength(100, ErrorMessage = "O nome não pode ter mais de {1} caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "A descrição não pode ter mais de {1} caracteres.")]
        public string? Descricao { get; set; }

        [Range(0.01, 100000, ErrorMessage = "O preço tem de estar entre {1} e {2}.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Preco { get; set; }




        // nome do ficheiro guardado em wwwroot/img (null = sem imagem)
        [StringLength(200)]
        public string? Imagem { get; set; }

        // depois de criada a propriedade Imagem na classe Produto
        // no NuGet package Manager Console fazer:
        // Add-Migration "ImagemNoProduto"
        // Update-Database








        // relacao com Categoria (chave estrangeira explicita)
        [Range(1, int.MaxValue, ErrorMessage = "Escolha a categoria.")]
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }











        // relacao com ModoEntrega (chave estrangeira explicita)
        [Range(1, int.MaxValue, ErrorMessage = "Escolha o modo de entrega.")]
        public int ModoEntregaId { get; set; }
        public ModoEntrega? ModoEntrega { get; set; }
    }
}

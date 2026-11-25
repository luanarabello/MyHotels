using System.ComponentModel.DataAnnotations;

namespace MyHotels.Entities;

/// <summary>
/// Representa uma categoria (ou subcategoria) para classificar alojamentos.
/// </summary>
public class Categoria
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Indique o nome da categoria.")]
    [StringLength(100, ErrorMessage = "O nome não pode ter mais de {1} caracteres.")]
    public string Nome { get; set; } = string.Empty;

    public int Ordem { get; set; } = 0;

    [StringLength(200)]
    public string? Imagem { get; set; }

    // Auto-referência opcional para suportar subcategorias (0..1 : N)
    public int? CategoriaPaiId { get; set; }
    public Categoria? CategoriaPai { get; set; }
    public ICollection<Categoria> Subcategorias { get; set; } = new List<Categoria>();

    // Relação N : M com Alojamento
    public ICollection<Alojamento> Alojamentos { get; set; } = new List<Alojamento>();
}

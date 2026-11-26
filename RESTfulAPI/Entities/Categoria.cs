using System.ComponentModel.DataAnnotations;

namespace RESTfulAPI.Entities;

public class Categoria
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    public int Ordem { get; set; } = 0;

    [StringLength(200)]
    public string? Imagem { get; set; }

    public int? CategoriaPaiId { get; set; }
    public Categoria? CategoriaPai { get; set; }
    public ICollection<Categoria> Subcategorias { get; set; } = new List<Categoria>();

    public ICollection<Alojamento> Alojamentos { get; set; } = new List<Alojamento>();
}

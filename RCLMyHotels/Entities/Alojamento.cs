using System.ComponentModel.DataAnnotations;

namespace RCLMyHotels.Entities;

public class Alojamento
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Descricao { get; set; }

    [Range(1, 20)]
    public int CapacidadePessoas { get; set; } = 2;

    public decimal PrecoFinal { get; set; }

    public int Stock { get; set; }

    public bool Disponivel { get; set; } = true;

    public bool Ativo { get; set; } = true;

    [StringLength(200)]
    public string? Imagem { get; set; }

    public int HotelId { get; set; }
    public Hotel? Hotel { get; set; }

    public int ModoDisponibilizacaoId { get; set; }
    public ModoDisponibilizacao? ModoDisponibilizacao { get; set; }

    public string? FornecedorId { get; set; }

    public List<Categoria> Categorias { get; set; } = new();
}

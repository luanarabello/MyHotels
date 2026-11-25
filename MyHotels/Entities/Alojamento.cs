using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyHotels.Data;

namespace MyHotels.Entities;

/// <summary>
/// Representa um quarto ou apartamento ("produto") pertencente a um Hotel/Aparthotel.
/// </summary>
public class Alojamento
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Indique o nome do alojamento.")]
    [StringLength(100, ErrorMessage = "O nome não pode ter mais de {1} caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "A descrição não pode ter mais de {1} caracteres.")]
    public string? Descricao { get; set; }

    [Range(1, 20, ErrorMessage = "A capacidade deve estar entre {1} e {2} pessoas.")]
    public int CapacidadePessoas { get; set; } = 2;

    [Range(0.01, 100000, ErrorMessage = "O preço base deve estar entre {1} e {2}.")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal PrecoBase { get; set; }

    [Range(0, 500, ErrorMessage = "A percentagem de lucro deve estar entre {1}% e {2}%.")]
    [Column(TypeName = "decimal(5,2)")]
    public decimal PercentagemLucro { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal PrecoFinal { get; set; }

    [Range(0, 10000, ErrorMessage = "O stock não pode ser negativo.")]
    public int Stock { get; set; }

    public bool Disponivel { get; set; } = true;

    public bool Ativo { get; set; } = true;

    [StringLength(200)]
    public string? Imagem { get; set; }

    // Relação 1 : N com Hotel (obrigatória)
    [Range(1, int.MaxValue, ErrorMessage = "Escolha o hotel ou aparthotel.")]
    public int HotelId { get; set; }
    public Hotel? Hotel { get; set; }

    // Relação 1 : N com ModoDisponibilizacao (obrigatória)
    [Range(1, int.MaxValue, ErrorMessage = "Escolha o modo de disponibilização.")]
    public int ModoDisponibilizacaoId { get; set; }
    public ModoDisponibilizacao? ModoDisponibilizacao { get; set; }

    // Relação 0..1 : N com Fornecedor (opcional)
    public string? FornecedorId { get; set; }
    public ApplicationUser? Fornecedor { get; set; }

    // Relação N : M com Categoria
    public ICollection<Categoria> Categorias { get; set; } = new List<Categoria>();

    // Relação 1 : N com ItemReserva
    public ICollection<ItemReserva> ItensReserva { get; set; } = new List<ItemReserva>();
}

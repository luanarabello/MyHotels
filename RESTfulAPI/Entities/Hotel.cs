using System.ComponentModel.DataAnnotations;

namespace RESTfulAPI.Entities;

public class Hotel
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [StringLength(30)]
    public string Tipo { get; set; } = "Hotel";

    [Range(1, 5)]
    public int Estrelas { get; set; } = 3;

    [Required]
    [StringLength(150)]
    public string Localizacao { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Descricao { get; set; }

    [StringLength(200)]
    public string? Imagem { get; set; }

    public bool Ativo { get; set; } = true;

    public ICollection<Alojamento> Alojamentos { get; set; } = new List<Alojamento>();
    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}

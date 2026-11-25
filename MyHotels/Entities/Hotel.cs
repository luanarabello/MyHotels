using System.ComponentModel.DataAnnotations;

namespace MyHotels.Entities;

/// <summary>
/// Representa um Hotel ou Aparthotel pertencente à cadeia MyHotels.
/// </summary>
public class Hotel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Indique o nome do hotel ou aparthotel.")]
    [StringLength(100, ErrorMessage = "O nome não pode ter mais de {1} caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Indique o tipo (Hotel ou Aparthotel).")]
    [StringLength(30, ErrorMessage = "O tipo não pode ter mais de {1} caracteres.")]
    public string Tipo { get; set; } = "Hotel";

    [Range(1, 5, ErrorMessage = "O número de estrelas deve estar entre {1} e {2}.")]
    public int Estrelas { get; set; } = 3;

    [Required(ErrorMessage = "Indique a localização.")]
    [StringLength(150, ErrorMessage = "A localização não pode ter mais de {1} caracteres.")]
    public string Localizacao { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "A descrição não pode ter mais de {1} caracteres.")]
    public string? Descricao { get; set; }

    [StringLength(200)]
    public string? Imagem { get; set; }

    public bool Ativo { get; set; } = true;

    // Navegações (1 : N)
    public ICollection<Alojamento> Alojamentos { get; set; } = new List<Alojamento>();
    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}

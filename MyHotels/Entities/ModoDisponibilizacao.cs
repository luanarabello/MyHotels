using System.ComponentModel.DataAnnotations;

namespace MyHotels.Entities;

/// <summary>
/// Define como um alojamento é disponibilizado (para reserva ou apenas listagem).
/// </summary>
public class ModoDisponibilizacao
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Indique o nome do modo de disponibilização.")]
    [StringLength(100, ErrorMessage = "O nome não pode ter mais de {1} caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [StringLength(250, ErrorMessage = "O detalhe não pode ter mais de {1} caracteres.")]
    public string? Detalhe { get; set; }

    /// <summary>
    /// Indica se os alojamentos com este modo podem ser reservados/comprados (true) ou são apenas listagem (false).
    /// </summary>
    public bool ParaReserva { get; set; } = true;

    // Navegação (1 : N)
    public ICollection<Alojamento> Alojamentos { get; set; } = new List<Alojamento>();
}

using System.ComponentModel.DataAnnotations;

namespace RCLMyHotels.Entities;

public class ModoDisponibilizacao
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [StringLength(250)]
    public string? Detalhe { get; set; }

    public bool ParaReserva { get; set; } = true;
}

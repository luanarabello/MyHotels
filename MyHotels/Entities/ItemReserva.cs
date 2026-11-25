using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyHotels.Entities;

/// <summary>
/// Representa uma linha dentro de uma reserva (alojamento, quantidade e preço unitário no momento da compra).
/// </summary>
public class ItemReserva
{
    public int Id { get; set; }

    public int ReservaId { get; set; }
    public Reserva? Reserva { get; set; }

    public int AlojamentoId { get; set; }
    public Alojamento? Alojamento { get; set; }

    [Range(1, 100, ErrorMessage = "A quantidade deve ser pelo menos {1}.")]
    public int Quantidade { get; set; } = 1;

    [Column(TypeName = "decimal(18,2)")]
    public decimal PrecoUnitario { get; set; }
}

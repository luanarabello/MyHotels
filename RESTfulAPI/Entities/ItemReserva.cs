using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RESTfulAPI.Entities;

public class ItemReserva
{
    public int Id { get; set; }

    public int ReservaId { get; set; }
    public Reserva? Reserva { get; set; }

    public int AlojamentoId { get; set; }
    public Alojamento? Alojamento { get; set; }

    [Range(1, 100)]
    public int Quantidade { get; set; } = 1;

    [Column(TypeName = "decimal(18,2)")]
    public decimal PrecoUnitario { get; set; }
}

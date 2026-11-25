using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyHotels.Data;

namespace MyHotels.Entities;

/// <summary>
/// Representa uma reserva efetuada por um cliente para um único Hotel/Aparthotel.
/// </summary>
public class Reserva
{
    public int Id { get; set; }

    public DateTime DataReserva { get; set; } = DateTime.UtcNow;

    public DateTime DataCheckIn { get; set; }

    public DateTime DataCheckOut { get; set; }

    [Required]
    [StringLength(30)]
    public string Estado { get; set; } = "Pendente";

    public bool Pago { get; set; } = false;

    [Column(TypeName = "decimal(18,2)")]
    public decimal ValorTotal { get; set; }

    // Relação 1 : N com Cliente (obrigatória)
    [Required]
    public string ClienteId { get; set; } = string.Empty;
    public ApplicationUser? Cliente { get; set; }

    // Relação 1 : N com Hotel (obrigatória: uma reserva refere-se a apenas um hotel/aparthotel)
    [Range(1, int.MaxValue, ErrorMessage = "A reserva tem de estar associada a um hotel.")]
    public int HotelId { get; set; }
    public Hotel? Hotel { get; set; }

    // Navegação (1 : N)
    public ICollection<ItemReserva> Itens { get; set; } = new List<ItemReserva>();
}

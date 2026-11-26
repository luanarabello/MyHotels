using System.ComponentModel.DataAnnotations;

namespace RCLMyHotels.Entities;

public class Reserva
{
    public int Id { get; set; }

    public DateTime DataReserva { get; set; }

    public DateTime DataCheckIn { get; set; }

    public DateTime DataCheckOut { get; set; }

    [Required]
    [StringLength(30)]
    public string Estado { get; set; } = "Pendente";

    public bool Pago { get; set; }

    public decimal ValorTotal { get; set; }

    public string ClienteId { get; set; } = string.Empty;

    public int HotelId { get; set; }
    public Hotel? Hotel { get; set; }

    public List<ItemReserva> Itens { get; set; } = new();
}

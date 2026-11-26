using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RESTfulAPI.Data;

namespace RESTfulAPI.Entities;

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

    [Required]
    public string ClienteId { get; set; } = string.Empty;
    public ApplicationUser? Cliente { get; set; }

    public int HotelId { get; set; }
    public Hotel? Hotel { get; set; }

    public ICollection<ItemReserva> Itens { get; set; } = new List<ItemReserva>();
}

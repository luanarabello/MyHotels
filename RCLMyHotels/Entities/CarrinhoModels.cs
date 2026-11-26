using System.ComponentModel.DataAnnotations;

namespace RCLMyHotels.Entities;

public class ItemCarrinho
{
    public Alojamento Alojamento { get; set; } = new();
    public int Quantidade { get; set; } = 1;
    public decimal Subtotal => Alojamento.PrecoFinal * Quantidade;
}

public class CriarReservaRequest
{
    [Required]
    public int HotelId { get; set; }

    [Required]
    public DateTime DataCheckIn { get; set; } = DateTime.Today.AddDays(1);

    [Required]
    public DateTime DataCheckOut { get; set; } = DateTime.Today.AddDays(2);

    public bool PagarAgora { get; set; } = true;

    public List<ItemReservaRequest> Itens { get; set; } = new();
}

public class ItemReservaRequest
{
    public int AlojamentoId { get; set; }
    public int Quantidade { get; set; }
}

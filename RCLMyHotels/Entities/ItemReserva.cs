namespace RCLMyHotels.Entities;

public class ItemReserva
{
    public int Id { get; set; }
    public int ReservaId { get; set; }
    public int AlojamentoId { get; set; }
    public Alojamento? Alojamento { get; set; }
    public int Quantidade { get; set; } = 1;
    public decimal PrecoUnitario { get; set; }
}

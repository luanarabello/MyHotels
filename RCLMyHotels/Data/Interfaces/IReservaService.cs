using RCLMyHotels.Entities;

namespace RCLMyHotels.Data.Interfaces;

public interface IReservaService
{
    Task<(bool Sucesso, string Mensagem, int? ReservaId)> CriarReservaAsync(CriarReservaRequest request);
    Task<List<Reserva>> ObterMinhasReservasAsync();
}

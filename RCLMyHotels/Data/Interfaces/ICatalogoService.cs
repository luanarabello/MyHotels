using RCLMyHotels.Entities;

namespace RCLMyHotels.Data.Interfaces;

public interface ICatalogoService
{
    Task<List<Hotel>> ObterHoteisAsync();
    Task<List<Categoria>> ObterCategoriasAsync();
    Task<List<Alojamento>> ObterAlojamentosAsync(int? hotelId = null, int? categoriaId = null, int? estrelas = null, string? localizacao = null);
    Task<Alojamento?> ObterDestaqueAsync();
    Task<Alojamento?> ObterAlojamentoPorIdAsync(int id);
}

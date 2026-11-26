using RCLMyHotels.Entities;

namespace RCLMyHotels.Data.Interfaces;

public interface ICarrinhoService
{
    event Action? OnChange;
    IReadOnlyList<ItemCarrinho> Itens { get; }
    int? HotelIdAtual { get; }
    decimal Total { get; }

    /// <summary>
    /// Adiciona um alojamento ao carrinho. Devolve false se pertencer a outro hotel diferente do atual.
    /// </summary>
    bool AdicionarItem(Alojamento alojamento, int quantidade = 1);
    void AtualizarQuantidade(int alojamentoId, int quantidade);
    void RemoverItem(int alojamentoId);
    void Limpar();
}

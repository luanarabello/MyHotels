namespace GestaoLoja.Services.Demos
{
    // Tres servicos identicos, registados com tempos de vida diferentes na DI (ver Program.cs).
    // O Guid mostra se estamos a usar a mesma instancia ou uma nova.
    public class DemoSingleton { public Guid Id { get; } = Guid.NewGuid(); public DateTime Criado { get; } = DateTime.Now; }
    public class DemoScoped { public Guid Id { get; } = Guid.NewGuid(); public DateTime Criado { get; } = DateTime.Now; }
    public class DemoTransient { public Guid Id { get; } = Guid.NewGuid(); public DateTime Criado { get; } = DateTime.Now; }

    // Servico partilhado com evento: um componente publica, outros reagem.
    public class AvisoService
    {
        public string? Mensagem { get; private set; }
        public event Action? Mudou;

        public void Publicar(string mensagem)
        {
            Mensagem = mensagem;
            Mudou?.Invoke();
        }
    }
}

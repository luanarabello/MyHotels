namespace GestaoLoja.Components.Pages.Demos.Componentes;

// Registo dos eventos do lifecycle de um componente.
// Esta classe existe para evitar um ciclo de render infinito: se o filho escrevesse numa List<string>
// recebida por [Parameter] e o pai fizesse StateHasChanged para a mostrar, cada render do pai voltaria a
// chamar SetParametersAsync/OnParametersSet no filho (a List e um tipo por referencia, por isso o Blazor
// assume que mudou), o filho escreveria outra vez, e assim sucessivamente.
// Aqui, quem mostra o registo (RegistoView) subscreve o evento Mudou e faz render so de si proprio.
public class RegistoCicloVida
{
    private readonly List<string> _linhas = new();

    public event Action? Mudou;

    public IReadOnlyList<string> Linhas
    {
        get
        {
            lock (_linhas) return _linhas.ToList();
        }
    }

    public void Adicionar(string evento)
    {
        lock (_linhas) _linhas.Add($"{DateTime.Now:HH:mm:ss.fff}  {evento}");
        Mudou?.Invoke();
    }

    public void Limpar()
    {
        lock (_linhas) _linhas.Clear();
        Mudou?.Invoke();
    }
}

namespace GestaoLoja.Components.Pages.Demos.Componentes;

// Codigo-fonte mostrado no fundo de cada demo ("Codigo"): le os ficheiros REAIS do disco, por isso nunca fica
// desactualizado em relacao ao que esta a correr.
//
// SEGURANCA: so e possivel ler os ficheiros desta lista fixa. O caminho nunca vem do utilizador (nem do URL):
// a demo indica o seu "slug" e a lista decide os ficheiros. Nao incluir nunca ficheiros com segredos
// (appsettings.json, connection strings, chaves...).

public static class CodigoFonteDados
{
    private const string P = "Components/Pages/Demos/";
    private const string C = "Components/Pages/Demos/Componentes/";
    private const string S = "Services/Demos/";

    // slug da demo -> ficheiros (caminhos relativos a pasta do projecto), pela ordem em que aparecem
    public static readonly Dictionary<string, string[]> FicheirosPorDemo = new()
    {
        ["bind"] = [P + "Bind.razor"],
        ["binding-manual"] = [P + "BindingManual.razor"],
        ["validacao"] = [P + "Validacao.razor", C + "CustomValidation.razor"],
        ["eventos-form"] = [P + "EventosForm.razor"],
        ["static-ssr"] = [P + "StaticSsr.razor", C + "FormularioDemo.razor"],
        ["ssr-interativo"] = [P + "SsrInterativo.razor", C + "FormularioDemo.razor"],
        ["static-overposting"] = [P + "StaticOverposting.razor"],
        ["ciclo-vida"] = [P + "CicloVida.razor", C + "FilhoCicloVida.razor", C + "RegistoView.razor", C + "RegistoCicloVida.cs"],
        ["prerender"] = [P + "Prerender.razor"],
        ["invoke-async"] = [P + "InvokeAsyncDemo.razor", C + "TimerDemo.razor"],
        ["di-tempo-vida"] = [P + "DiTempoVida.razor", C + "MostraServicos.razor", S + "DemoServicos.cs"],
        ["concorrencia"] = [P + "Concorrencia.razor"],
        ["n-mais-1"] = [P + "NMais1.razor", S + "DemoDb.cs"],
        ["tracking"] = [P + "TrackingDemo.razor", S + "DemoDb.cs"],
        ["concorrencia-optimista"] = [P + "ConcorrenciaOptimista.razor", S + "DemoDb.cs"],
        ["cascade-restrict"] = [P + "CascadeRestrict.razor", S + "DemoDb.cs"],
        ["migrations"] = [P + "MigrationsDemo.razor"],
        ["identidade"] = [P + "Identidade.razor"],
        ["autorizacao"] = [P + "Autorizacao.razor", P + "AdminProtegido.razor"],   // a AdminProtegido nao se consegue abrir sem a role Admin: o codigo ve-se aqui
        ["admin-protegido"] = [P + "AdminProtegido.razor"],
        ["xss"] = [P + "Xss.razor"],
        ["upload-inseguro"] = [P + "UploadInseguro.razor"],
        ["comunicacao"] = [P + "Comunicacao.razor", C + "ContadorFilho.razor", C + "CaixaTema.razor", C + "PainelAviso.razor", S + "DemoServicos.cs"],
        ["componentes-genericos"] = [P + "ComponentesGenericos.razor", C + "TabelaGenerica.razor"],
        ["js-interop"] = [P + "JsInterop.razor"],
    };
}

public class CodigoFonteService(IWebHostEnvironment env)
{
    private static readonly HashSet<string> Permitidos =
        new(CodigoFonteDados.FicheirosPorDemo.Values.SelectMany(f => f), StringComparer.Ordinal);

    public IReadOnlyList<string> FicheirosDe(string slug) =>
        CodigoFonteDados.FicheirosPorDemo.TryGetValue(slug, out var f) ? f : Array.Empty<string>();

    /// <summary>Devolve o texto do ficheiro, ou null se nao for permitido ou nao existir neste ambiente.</summary>
    public string? Ler(string caminhoRelativo)
    {
        // 1) so ficheiros da lista fixa
        if (!Permitidos.Contains(caminhoRelativo)) return null;

        // 2) defesa extra: o caminho final tem de ficar dentro da pasta do projecto
        var raiz = Path.GetFullPath(env.ContentRootPath);
        var completo = Path.GetFullPath(Path.Combine(raiz, caminhoRelativo));
        if (!completo.StartsWith(raiz + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)) return null;

        return File.Exists(completo) ? File.ReadAllText(completo).ReplaceLineEndings("\n") : null;
    }
}

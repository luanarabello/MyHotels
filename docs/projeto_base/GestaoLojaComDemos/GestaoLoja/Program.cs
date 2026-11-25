// =====================================================================================================
//  ESCOLHA A BASE DE DADOS: deixe activa UMA (e so uma) das duas linhas seguintes e comente a outra com //
//
//      #define USAR_SQLITE      -> SQLite: um ficheiro (Data/app.db), nao precisa de servidor     (por omissao)
//      #define USAR_LOCALDB     -> SQL Server LocalDB: base de dados FrutariaPWeb (como no enunciado)
//
//  As linhas '#define' tem de ficar aqui, no topo do ficheiro, antes dos 'using'.
//  Passos completos no README.md, seccao "Escolher a base de dados".
//
//  IMPORTANTE: as migrations dependem do provider. Ao mudar de opcao, apague a pasta Data/Migrations (e a
//  BD antiga) e crie a migration outra vez (Add-Migration BDInicial + Update-Database), como na Ficha 4.
// =====================================================================================================
#define USAR_SQLITE
//#define USAR_LOCALDB

using GestaoLoja;
using GestaoLoja.Components;
using GestaoLoja.Components.Account;
using GestaoLoja.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Cultura da aplicacao: pt-PT (numeros com virgula decimal, datas dd/MM/aaaa, moeda em euros), qualquer que seja
// a configuracao do Windows ou do browser de cada aluno. Tambem se aplica a threads fora de um pedido (tarefas, seed).
var culturaPt = new CultureInfo("pt-PT");
CultureInfo.DefaultThreadCurrentCulture = culturaPt;
CultureInfo.DefaultThreadCurrentUICulture = culturaPt;

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();


// ---------- BASE DE DADOS (a escolha faz-se no topo deste ficheiro: #define USAR_SQLITE ou USAR_LOCALDB) ----------
#if USAR_SQLITE && USAR_LOCALDB
#error Estao activas as duas bases de dados. No topo do Program.cs deixe UMA so linha '#define' (USAR_SQLITE ou USAR_LOCALDB).
#elif USAR_SQLITE
// SQLite: ficheiro Data/app.db (connection string "DefaultConnection" do appsettings.json)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// caminho absoluto para a BD SQLite: evita depender da directoria de trabalho
// (que e diferente quando a app e iniciada pelo scaffolder / ferramentas EF)
connectionString = connectionString.Replace(@"Data\app.db",
    Path.Combine(builder.Environment.ContentRootPath, "Data", "app.db"));

// para podermos usar o DBFactory
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
#elif USAR_LOCALDB
// SQL Server LocalDB: base de dados FrutariaPWeb (connection string "LocalDbConnection" do appsettings.json)
var connectionString = builder.Configuration.GetConnectionString("LocalDbConnection") ?? throw new InvalidOperationException("Connection string 'LocalDbConnection' not found.");

// para podermos usar o DBFactory
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
#else
#error Falta escolher a base de dados. No topo do Program.cs deixe activa UMA linha '#define' (USAR_SQLITE ou USAR_LOCALDB).
#endif



builder.Services.AddQuickGridEntityFrameworkAdapter();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()                       // activa as roles (Admin, Cliente): ver AppConfig.cs e Data/Inicializacao.cs
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.AddScoped<GestaoLoja.Services.ImagemService>();

// --- servicos usados apenas pelas paginas de demonstracao (Components/Pages/Demos) ---
builder.Services.AddSingleton<GestaoLoja.Services.Demos.DemoSingleton>();
builder.Services.AddScoped<GestaoLoja.Services.Demos.DemoScoped>();
builder.Services.AddTransient<GestaoLoja.Services.Demos.DemoTransient>();
builder.Services.AddScoped<GestaoLoja.Services.Demos.AvisoService>();
builder.Services.AddSingleton<GestaoLoja.Services.Demos.DemoDbService>();
builder.Services.AddSingleton<GestaoLoja.Components.Pages.Demos.Componentes.CodigoFonteService>();

// policy "SoAdmin": exige a role Admin (usada nas Razor Pages geradas pelo scaffold, em Pages/CategoriaPages)
builder.Services.AddAuthorizationBuilder()
    .AddPolicy(AppConfig.PolicySoAdmin, policy => policy.RequireRole(AppConfig.RoleAdmin));

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/CategoriaPages", AppConfig.PolicySoAdmin);
});

var app = builder.Build();

// var scope = app.Services.CreateScope();
using (var scope = app.Services.CreateScope())
{
    try
    {
        var services = scope.ServiceProvider;
        var dbContext = services.GetRequiredService<ApplicationDbContext>();
        Inicializacao.EscreveDados(dbContext);

        // roles (Admin, Cliente) e, so em Development, utilizadores de teste (ver appsettings.json, seccao "Seed")
        await Inicializacao.CriarRolesEUtilizadoresAsync(services, app.Configuration, app.Environment.IsDevelopment());
    }
    catch (Exception ex)
    {
        // nao impedir o arranque (ex.: scaffolder / ferramentas EF sem BD disponivel,
        // ou a primeira execucao antes de correr o Update-Database)
        app.Logger.LogError(ex, "Falha ao inicializar os dados base.");
    }
}

// pt-PT por omissao e unica cultura suportada: o Blazor (prerender e circuit) usa-a para formatar numeros, datas e moeda
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(culturaPt),
    SupportedCultures = [culturaPt],
    SupportedUICultures = [culturaPt]
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.MapRazorPages();

app.Run();

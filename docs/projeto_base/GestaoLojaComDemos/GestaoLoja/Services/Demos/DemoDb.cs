using System.ComponentModel.DataAnnotations;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;

namespace GestaoLoja.Services.Demos
{
    // ---------------------------------------------------------------------------------
    // Base de dados ISOLADA para as paginas de demonstracao (Data/demos.db).
    // Assim as demos nunca mexem nos dados reais da loja.
    //
    // NOTA: o contexto e generico (DemoDbContext<T>) e nao esta registado na DI de proposito:
    // as ferramentas de migrations (Add-Migration / Update-Database) so descobrem contextos
    // concretos, e assim continuam a ver apenas o ApplicationDbContext.
    // ---------------------------------------------------------------------------------

    public class DemoMarker { }

    public class DemoCategoria
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public List<DemoProduto> Produtos { get; set; } = new();
        public List<DemoProdutoR> ProdutosR { get; set; } = new();
    }

    // FK com apagar em CASCATA (comportamento por omissao para FK obrigatorias)
    public class DemoProduto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public int DemoCategoriaId { get; set; }
        public DemoCategoria? Categoria { get; set; }
    }

    // FK com apagar RESTRITO
    public class DemoProdutoR
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int DemoCategoriaId { get; set; }
        public DemoCategoria? Categoria { get; set; }
    }

    // com "token" de concorrencia optimista
    public class DemoItem
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        [ConcurrencyCheck]
        public int Versao { get; set; }
    }

    // sem controlo de concorrencia (o ultimo a gravar ganha)
    public class DemoItemSemToken
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
    }

    public class DemoDbContext<T>(DbContextOptions<DemoDbContext<T>> options) : DbContext(options)
    {
        public DbSet<DemoCategoria> Categorias => Set<DemoCategoria>();
        public DbSet<DemoProduto> Produtos => Set<DemoProduto>();
        public DbSet<DemoProdutoR> ProdutosR => Set<DemoProdutoR>();
        public DbSet<DemoItem> Itens => Set<DemoItem>();
        public DbSet<DemoItemSemToken> ItensSemToken => Set<DemoItemSemToken>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DemoProdutoR>()
                .HasOne(p => p.Categoria).WithMany(c => c.ProdutosR)
                .HasForeignKey(p => p.DemoCategoriaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class DemoDbService
    {
        private readonly string _caminho;
        private readonly object _lock = new();
        private bool _inicializada;

        public DemoDbService(IWebHostEnvironment env)
        {
            _caminho = Path.Combine(env.ContentRootPath, "Data", "demos.db");
        }

        /// <summary>Cria um contexto novo. Se "log" for indicado, recebe cada comando SQL executado.</summary>
        public DemoDbContext<DemoMarker> Criar(Action<string>? log = null)
        {
            Garantir();
            return CriarSemGarantir(log);
        }

        private DemoDbContext<DemoMarker> CriarSemGarantir(Action<string>? log)
        {
            var builder = new DbContextOptionsBuilder<DemoDbContext<DemoMarker>>()
                .UseSqlite($"DataSource={_caminho}");
            if (log is not null)
                builder.LogTo(log, new[] { RelationalEventId.CommandExecuted }, LogLevel.Information, DbContextLoggerOptions.None);
            return new DemoDbContext<DemoMarker>(builder.Options);
        }

        private void Garantir()
        {
            if (_inicializada) return;
            lock (_lock)
            {
                if (_inicializada) return;
                Directory.CreateDirectory(Path.GetDirectoryName(_caminho)!);
                using var ctx = CriarSemGarantir(null);
                ctx.Database.EnsureCreated();
                if (!ctx.Categorias.Any()) Semear(ctx);
                _inicializada = true;
            }
        }

        /// <summary>Apaga e recria a BD de demonstracao com os dados iniciais.</summary>
        public void Reiniciar()
        {
            lock (_lock)
            {
                using (var ctx = CriarSemGarantir(null))
                {
                    ctx.Database.EnsureDeleted();
                    ctx.Database.EnsureCreated();
                    Semear(ctx);
                }
                SqliteConnection.ClearAllPools();
                _inicializada = true;
            }
        }

        private static void Semear(DemoDbContext<DemoMarker> ctx)
        {
            var frutas = new DemoCategoria { Nome = "Frutas" };
            var legumes = new DemoCategoria { Nome = "Legumes" };
            var padaria = new DemoCategoria { Nome = "Padaria" };
            ctx.Categorias.AddRange(frutas, legumes, padaria);

            string[] nomes = ["Maçã", "Pera", "Banana", "Laranja", "Cenoura", "Alface", "Tomate", "Batata", "Pão", "Broa", "Bolo", "Pastel"];
            var cats = new[] { frutas, frutas, frutas, frutas, legumes, legumes, legumes, legumes, padaria, padaria, padaria, padaria };
            for (int i = 0; i < nomes.Length; i++)
                ctx.Produtos.Add(new DemoProduto { Nome = nomes[i], Preco = 0.5m + i * 0.25m, Categoria = cats[i] });

            ctx.ProdutosR.Add(new DemoProdutoR { Nome = "Produto protegido A", Categoria = frutas });
            ctx.ProdutosR.Add(new DemoProdutoR { Nome = "Produto protegido B", Categoria = legumes });

            ctx.Itens.Add(new DemoItem { Nome = "Item original", Versao = 1 });
            ctx.ItensSemToken.Add(new DemoItemSemToken { Nome = "Item original" });
            ctx.SaveChanges();
        }
    }
}

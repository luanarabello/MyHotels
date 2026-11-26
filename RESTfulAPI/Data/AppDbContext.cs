using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RESTfulAPI.Entities;

namespace RESTfulAPI.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Hotel> Hoteis => Set<Hotel>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<ModoDisponibilizacao> ModosDisponibilizacao => Set<ModoDisponibilizacao>();
    public DbSet<Alojamento> Alojamentos => Set<Alojamento>();
    public DbSet<Reserva> Reservas => Set<Reserva>();
    public DbSet<ItemReserva> ItensReserva => Set<ItemReserva>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Reserva>()
            .HasOne(r => r.Hotel)
            .WithMany(h => h.Reservas)
            .HasForeignKey(r => r.HotelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Reserva>()
            .HasOne(r => r.Cliente)
            .WithMany()
            .HasForeignKey(r => r.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ItemReserva>()
            .HasOne(i => i.Reserva)
            .WithMany(r => r.Itens)
            .HasForeignKey(i => i.ReservaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ItemReserva>()
            .HasOne(i => i.Alojamento)
            .WithMany(a => a.ItensReserva)
            .HasForeignKey(i => i.AlojamentoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Categoria>()
            .HasOne(c => c.CategoriaPai)
            .WithMany(c => c.Subcategorias)
            .HasForeignKey(c => c.CategoriaPaiId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Alojamento>()
            .HasOne(a => a.Hotel)
            .WithMany(h => h.Alojamentos)
            .HasForeignKey(a => a.HotelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Alojamento>()
            .HasOne(a => a.ModoDisponibilizacao)
            .WithMany(m => m.Alojamentos)
            .HasForeignKey(a => a.ModoDisponibilizacaoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Alojamento>()
            .HasOne(a => a.Fornecedor)
            .WithMany()
            .HasForeignKey(a => a.FornecedorId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyHotels.Entities;

namespace MyHotels.Data;

/// <summary>
/// Responsável por criar as roles, utilizadores de teste e dados iniciais da base de dados.
/// </summary>
public static class Inicializacao
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

        // 1. Criar Roles se não existirem
        foreach (var role in AppConfig.Roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // 2. Criar utilizadores de teste (apenas se ainda não existirem)
        await GarantirUtilizadorAsync(userManager,
            email: "admin@myhotels.pt",
            password: "Admin#123",
            nome: "Administrador",
            apelido: "MyHotels",
            role: AppConfig.RoleAdmin);

        await GarantirUtilizadorAsync(userManager,
            email: "func@myhotels.pt",
            password: "Func#123",
            nome: "Funcionário",
            apelido: "MyHotels",
            role: AppConfig.RoleFuncionario);

        await GarantirUtilizadorAsync(userManager,
            email: "cliente@myhotels.pt",
            password: "Cliente#123",
            nome: "Cliente",
            apelido: "Exemplo",
            role: AppConfig.RoleCliente);

        // 3. Seed de Hotéis / Aparthoteis
        if (!await context.Hoteis.AnyAsync())
        {
            context.Hoteis.AddRange(
                new Hotel
                {
                    Nome = "MyHotels Atlântico",
                    Tipo = "Hotel",
                    Estrelas = 5,
                    Localizacao = "Lisboa",
                    Descricao = "Hotel de 5 estrelas frente ao Tejo com spa e vista panorâmica.",
                    Ativo = true
                },
                new Hotel
                {
                    Nome = "MyHotels Ribeira",
                    Tipo = "Hotel",
                    Estrelas = 4,
                    Localizacao = "Porto",
                    Descricao = "Hotel boutique no coração da Ribeira do Porto.",
                    Ativo = true
                },
                new Hotel
                {
                    Nome = "MyHotels Sol Aparthotel",
                    Tipo = "Aparthotel",
                    Estrelas = 4,
                    Localizacao = "Algarve",
                    Descricao = "Aparthotel familiar com piscina e acesso direto à praia.",
                    Ativo = true
                });

            await context.SaveChangesAsync();
        }

        // 4. Seed de Categorias (incluindo subcategoria de exemplo)
        if (!await context.Categorias.AnyAsync())
        {
            var catQuartos = new Categoria { Nome = "Quartos", Ordem = 1 };
            var catApartamentos = new Categoria { Nome = "Apartamentos", Ordem = 2 };
            var catComodidades = new Categoria { Nome = "Comodidades", Ordem = 3 };

            context.Categorias.AddRange(catQuartos, catApartamentos, catComodidades);
            await context.SaveChangesAsync();

            context.Categorias.AddRange(
                new Categoria { Nome = "Quarto Standard", Ordem = 1, CategoriaPaiId = catQuartos.Id },
                new Categoria { Nome = "Suíte", Ordem = 2, CategoriaPaiId = catQuartos.Id },
                new Categoria { Nome = "Estúdio T0", Ordem = 1, CategoriaPaiId = catApartamentos.Id },
                new Categoria { Nome = "Apartamento T1", Ordem = 2, CategoriaPaiId = catApartamentos.Id },
                new Categoria { Nome = "Vista Mar", Ordem = 1, CategoriaPaiId = catComodidades.Id });

            await context.SaveChangesAsync();
        }

        // 5. Seed de Modos de Disponibilização
        if (!await context.ModosDisponibilizacao.AnyAsync())
        {
            context.ModosDisponibilizacao.AddRange(
                new ModoDisponibilizacao
                {
                    Nome = "Por Noite (Só Alojamento)",
                    Detalhe = "Disponível para reserva imediata apenas com alojamento.",
                    ParaReserva = true
                },
                new ModoDisponibilizacao
                {
                    Nome = "Por Noite (Pequeno-Almoço Incluído)",
                    Detalhe = "Disponível para reserva com pequeno-almoço buffet incluído.",
                    ParaReserva = true
                },
                new ModoDisponibilizacao
                {
                    Nome = "Apenas Listagem",
                    Detalhe = "Apresentação em catálogo (não disponível para reserva online).",
                    ParaReserva = false
                });

            await context.SaveChangesAsync();
        }

        // 6. Seed de Alojamentos (dependentes de Hotel, Categoria e ModoDisponibilizacao)
        if (!await context.Alojamentos.AnyAsync())
        {
            var hotelLisboa = await context.Hoteis.FirstAsync(h => h.Localizacao == "Lisboa");
            var hotelPorto = await context.Hoteis.FirstAsync(h => h.Localizacao == "Porto");
            var hotelAlgarve = await context.Hoteis.FirstAsync(h => h.Localizacao == "Algarve");

            var modoSoAloj = await context.ModosDisponibilizacao.FirstAsync(m => m.ParaReserva && m.Nome.Contains("Só Alojamento"));
            var modoPeqAlmoco = await context.ModosDisponibilizacao.FirstAsync(m => m.ParaReserva && m.Nome.Contains("Pequeno-Almoço"));
            var modoListagem = await context.ModosDisponibilizacao.FirstAsync(m => !m.ParaReserva);

            var catStandard = await context.Categorias.FirstAsync(c => c.Nome == "Quarto Standard");
            var catSuite = await context.Categorias.FirstAsync(c => c.Nome == "Suíte");
            var catT0 = await context.Categorias.FirstAsync(c => c.Nome == "Estúdio T0");
            var catT1 = await context.Categorias.FirstAsync(c => c.Nome == "Apartamento T1");
            var catVistaMar = await context.Categorias.FirstAsync(c => c.Nome == "Vista Mar");

            context.Alojamentos.AddRange(
                new Alojamento
                {
                    Nome = "Suíte Presidencial Tejo",
                    Descricao = "Suíte ampla com varanda privada e vista rio.",
                    CapacidadePessoas = 2,
                    PrecoBase = 200m,
                    PercentagemLucro = 20m,
                    PrecoFinal = 240m,
                    Stock = 3,
                    Disponivel = true,
                    Ativo = true,
                    HotelId = hotelLisboa.Id,
                    ModoDisponibilizacaoId = modoPeqAlmoco.Id,
                    Categorias = [catSuite, catVistaMar]
                },
                new Alojamento
                {
                    Nome = "Quarto Standard Lisboa",
                    Descricao = "Quarto confortável com cama de casal e secretária.",
                    CapacidadePessoas = 2,
                    PrecoBase = 90m,
                    PercentagemLucro = 15m,
                    PrecoFinal = 103.50m,
                    Stock = 8,
                    Disponivel = true,
                    Ativo = true,
                    HotelId = hotelLisboa.Id,
                    ModoDisponibilizacaoId = modoSoAloj.Id,
                    Categorias = [catStandard]
                },
                new Alojamento
                {
                    Nome = "Quarto Duplo Ribeira",
                    Descricao = "Vista para as casas típicas do Porto.",
                    CapacidadePessoas = 2,
                    PrecoBase = 110m,
                    PercentagemLucro = 10m,
                    PrecoFinal = 121m,
                    Stock = 5,
                    Disponivel = true,
                    Ativo = true,
                    HotelId = hotelPorto.Id,
                    ModoDisponibilizacaoId = modoPeqAlmoco.Id,
                    Categorias = [catStandard]
                },
                new Alojamento
                {
                    Nome = "Estúdio T0 Frente Mar",
                    Descricao = "Estúdio equipado com kitchenette e varanda.",
                    CapacidadePessoas = 2,
                    PrecoBase = 80m,
                    PercentagemLucro = 25m,
                    PrecoFinal = 100m,
                    Stock = 6,
                    Disponivel = true,
                    Ativo = true,
                    HotelId = hotelAlgarve.Id,
                    ModoDisponibilizacaoId = modoSoAloj.Id,
                    Categorias = [catT0, catVistaMar]
                },
                new Alojamento
                {
                    Nome = "Apartamento T1 Família (Brevemente)",
                    Descricao = "Apartamento em renovação, disponível apenas para consulta.",
                    CapacidadePessoas = 4,
                    PrecoBase = 140m,
                    PercentagemLucro = 20m,
                    PrecoFinal = 168m,
                    Stock = 0,
                    Disponivel = true,
                    Ativo = true,
                    HotelId = hotelAlgarve.Id,
                    ModoDisponibilizacaoId = modoListagem.Id,
                    Categorias = [catT1]
                });

            await context.SaveChangesAsync();
        }
    }

    private static async Task GarantirUtilizadorAsync(
        UserManager<ApplicationUser> userManager,
        string email,
        string password,
        string nome,
        string apelido,
        string role)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                Nome = nome,
                Apelido = apelido,
                Estado = "Ativo",
                DataRegisto = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                return;
            }
        }

        if (!await userManager.IsInRoleAsync(user, role))
        {
            await userManager.AddToRoleAsync(user, role);
        }
    }
}

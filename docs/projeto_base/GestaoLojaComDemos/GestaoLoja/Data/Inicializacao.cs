using GestaoLoja.Entities;
using Microsoft.AspNetCore.Identity;

namespace GestaoLoja.Data
{

    // classe para inicializar dados "base" 
    // para que a aplicaçao possa correr
    // já com esses dados

    // exemplo: duas ou tres categorias
    //          pelo menos um modo de entrega
    //          um utilizador de testes ? - talvez
    public static class Inicializacao
    {
        // Cria as roles (Admin, Cliente) e, em Development, dois utilizadores de teste:
        //     admin   -> role Admin    (gere produtos, categorias e modos de entrega)
        //     cliente -> role Cliente  (so consulta)
        // Os emails e palavras-passe estao em appsettings.json, secao "Seed".
        // E idempotente: pode correr em todos os arranques sem duplicar nada.
        public static async Task CriarRolesEUtilizadoresAsync(IServiceProvider services, IConfiguration configuracao, bool criarUtilizadoresDeTeste)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            foreach (var role in AppConfig.Roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // as palavras-passe de teste nunca devem existir fora de Development
            if (!criarUtilizadoresDeTeste) return;

            await CriarUtilizadorAsync(services, configuracao["Seed:AdminEmail"], configuracao["Seed:AdminPassword"], "Administrador", AppConfig.RoleAdmin);
            await CriarUtilizadorAsync(services, configuracao["Seed:ClienteEmail"], configuracao["Seed:ClientePassword"], "Cliente de Teste", AppConfig.RoleCliente);
        }

        private static async Task CriarUtilizadorAsync(IServiceProvider services, string? email, string? password, string nome, string role)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password)) return;

            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var user = await userManager.FindByEmailAsync(email);

            if (user is null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,   // o projecto exige conta confirmada para fazer login
                    Nome = nome
                };
                var resultado = await userManager.CreateAsync(user, password);
                if (!resultado.Succeeded)
                    throw new InvalidOperationException($"Nao foi possivel criar '{email}': {string.Join("; ", resultado.Errors.Select(e => e.Description))}");
            }

            if (!await userManager.IsInRoleAsync(user, role))
                await userManager.AddToRoleAsync(user, role);
        }

       public static void EscreveDados(ApplicationDbContext context)
        {
            if(context.Categorias.Any())
            {
                   // já temos categorias na base de dados
                   // nao vamos avançar com a criaçao do seed
            }else
            {
                // ainda nao há categorias na base de dados
                // vamos criar e inserir

                Categoria cat1 = new Categoria();
                cat1.Nome = "Fruta";
                Categoria cat2 = new Categoria();
                cat2.Nome = "Legumes";
                Categoria cat3 = new Categoria();
                cat3.Nome = "Padaria";

                context.Categorias.Add(cat1);
                context.Categorias.Add(cat2);
                context.Categorias.Add(cat3);
                context.SaveChanges();

            }

            if (!context.ModosEntrega.Any())
            {
                // nao existem modos de entrega na base de dados (VER IF)

                ModoEntrega modo1 = new ModoEntrega();
                modo1.Nome = "CTT Express";

                context.ModosEntrega.Add(modo1);
                context.SaveChanges() ; // guarda as alteracoes na BD!
            }

       


          


        }
    }
}

namespace GestaoLoja
{
    // Constantes de configuracao da aplicacao.
    // Os nomes das roles estao aqui (e nao como texto solto) para se poderem usar em atributos:
    //     @attribute [Authorize(Roles = AppConfig.RoleAdmin)]
    public static class AppConfig
    {
        public const string RoleAdmin = "Admin";       // gere produtos, categorias e modos de entrega
        public const string RoleCliente = "Cliente";   // utilizador normal (sem acesso a gestao)

        public static readonly string[] Roles = [RoleAdmin, RoleCliente];

        // nome da policy que exige a role Admin (usada nas Razor Pages geradas pelo scaffold)
        public const string PolicySoAdmin = "SoAdmin";
    }
}

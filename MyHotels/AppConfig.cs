namespace MyHotels;

/// <summary>
/// Constantes globais da aplicação (perfis/roles e políticas de autorização).
/// </summary>
public static class AppConfig
{
    public const string RoleAdmin = "Administrador";
    public const string RoleFuncionario = "Funcionario";
    public const string RoleCliente = "Cliente";
    public const string RoleFornecedor = "Fornecedor";

    public static readonly string[] Roles =
    [
        RoleAdmin,
        RoleFuncionario,
        RoleCliente,
        RoleFornecedor
    ];
}

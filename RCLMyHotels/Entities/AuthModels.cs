using System.ComponentModel.DataAnnotations;

namespace RCLMyHotels.Entities;

public class LoginModel
{
    [Required(ErrorMessage = "Indique o email.")]
    [EmailAddress(ErrorMessage = "Email inválido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Indique a palavra-passe.")]
    public string Password { get; set; } = string.Empty;
}

public class RegistoClienteModel
{
    [Required(ErrorMessage = "Indique o nome.")]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Apelido { get; set; }

    [Required(ErrorMessage = "Indique o NIF.")]
    [StringLength(20)]
    public string NIF { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Morada { get; set; }

    [StringLength(100)]
    public string? Localidade { get; set; }

    [Required(ErrorMessage = "Indique o email.")]
    [EmailAddress(ErrorMessage = "Email inválido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Indique a palavra-passe.")]
    [MinLength(6, ErrorMessage = "A palavra-passe deve ter pelo menos 6 caracteres.")]
    public string Password { get; set; } = string.Empty;
}

public class AuthResponse
{
    public bool Sucesso { get; set; }
    public string? Mensagem { get; set; }
    public string? Token { get; set; }
    public string? Email { get; set; }
    public string? Nome { get; set; }
    public List<string> Roles { get; set; } = new();
}

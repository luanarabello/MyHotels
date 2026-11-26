using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace RESTfulAPI.Data;

public class ApplicationUser : IdentityUser
{
    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Apelido { get; set; }

    [StringLength(20)]
    public string? NIF { get; set; }

    [StringLength(200)]
    public string? Morada { get; set; }

    [StringLength(100)]
    public string? Localidade { get; set; }

    [Required]
    [StringLength(20)]
    public string Estado { get; set; } = "Pendente";

    public DateTime DataRegisto { get; set; } = DateTime.UtcNow;
}

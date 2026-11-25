using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MyHotels.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Indique o nome.")]
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

        /// <summary>
        /// Estado da conta de utilizador ("Pendente" no registo de cliente, "Ativo" após aprovação).
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = "Pendente";

        public DateTime DataRegisto { get; set; } = DateTime.UtcNow;
    }

}

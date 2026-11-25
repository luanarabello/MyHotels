using Microsoft.AspNetCore.Identity;

namespace GestaoLoja.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string? Nome { get; set; }
        public string? Cidade { get; set; }
        public DateOnly? DataNascimento { get; set; }
    }

}

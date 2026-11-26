using RCLMyHotels.Entities;

namespace RCLMyHotels.Data.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginModel model);
    Task<AuthResponse> RegistoClienteAsync(RegistoClienteModel model);
    Task LogoutAsync();
    Task<string?> ObterTokenAsync();
}

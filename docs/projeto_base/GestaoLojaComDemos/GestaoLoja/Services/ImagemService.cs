using Microsoft.AspNetCore.Components.Forms;

namespace GestaoLoja.Services
{
    // guarda e apaga as imagens dos produtos em wwwroot/img
    public class ImagemService(IWebHostEnvironment env)
    {
        public const long TamanhoMaximo = 2 * 1024 * 1024; // 2 MB
        private static readonly string[] Extensoes = [".jpg", ".jpeg", ".png", ".gif", ".webp"];

        private string Pasta => Path.Combine(env.WebRootPath, "img");

        public async Task<(bool Ok, string? Nome, string? Erro)> GuardarAsync(IBrowserFile ficheiro)
        {
            var extensao = Path.GetExtension(ficheiro.Name).ToLowerInvariant();
            if (!Extensoes.Contains(extensao) || !ficheiro.ContentType.StartsWith("image/"))
                return (false, null, "Formato inválido. Use JPG, PNG, GIF ou WEBP.");
            if (ficheiro.Size > TamanhoMaximo)
                return (false, null, "A imagem não pode ter mais de 2 MB.");

            Directory.CreateDirectory(Pasta);
            // o nome original do utilizador nunca e usado: evita colisoes e path traversal
            var nome = $"{Guid.NewGuid():N}{extensao}";

            await using var destino = File.Create(Path.Combine(Pasta, nome));
            await using var origem = ficheiro.OpenReadStream(TamanhoMaximo);
            await origem.CopyToAsync(destino);
            return (true, nome, null);
        }

        public void Apagar(string? nome)
        {
            if (string.IsNullOrEmpty(nome)) return;
            var caminho = Path.Combine(Pasta, Path.GetFileName(nome));
            if (File.Exists(caminho)) File.Delete(caminho);
        }
    }
}

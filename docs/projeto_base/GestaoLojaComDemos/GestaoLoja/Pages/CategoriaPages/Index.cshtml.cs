using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GestaoLoja.Entities;
using GestaoLoja.Data;

namespace GestaoLoja.Pages.CategoriaPages;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Categoria> Categoria { get; set; } = default!;

    public async Task OnGetAsync()
    {
        Categoria = await _context.Categorias.ToListAsync();
    }
}

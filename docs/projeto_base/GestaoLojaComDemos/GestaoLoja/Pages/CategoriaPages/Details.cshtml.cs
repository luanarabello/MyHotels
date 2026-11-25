using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GestaoLoja.Entities;
using GestaoLoja.Data;

namespace GestaoLoja.Pages.CategoriaPages;

public class DetailsModel : PageModel
{
    private readonly ApplicationDbContext _context;
    public DetailsModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public Categoria Categoria { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var categoria = await _context.Categorias.FirstOrDefaultAsync(m => m.Id == id);
        if (categoria is null)
        {
            return NotFound();
        }
        else
        {
            Categoria = categoria;
        }

        return Page();
    }
}

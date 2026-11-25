using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GestaoLoja.Entities;
using GestaoLoja.Data;

namespace GestaoLoja.Pages.CategoriaPages;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
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

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria != null)
        {
            Categoria = categoria;
            _context.Categorias.Remove(Categoria);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.EF;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly IRepository<Cheep, string> _service;
    public List<Cheep> Cheeps { get; set; }

    public PublicModel(IRepository<Cheep, string> service)
    {
        _service = service;
    }

    [FromQuery(Name = "page")]
    public int PageNo {get; set;}
    
    public async Task<IActionResult> OnGetAsync()
{
    var cheeps = await _service.Get((PageNo - 1) * 32);
    Cheeps = cheeps.ToList();
    return Page();
}
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Infrastructure;
using Chirp.Core;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepRepository<CheepDTO, string> _service;
    public List<CheepDTO> Cheeps { get; set; }

    public PublicModel(ICheepRepository<CheepDTO, string> service)
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

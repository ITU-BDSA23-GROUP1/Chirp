using Chirp.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly IRepository<CheepDTO, string> _service;
    public List<CheepDTO> Cheeps { get; set; }

    public UserTimelineModel(IRepository<CheepDTO, string> service)
    {
        _service = service;
    }

    [FromQuery(Name = "page")]
    public int PageNo {get; set;}
    public async Task<IActionResult> OnGetAsync(string AuthorName)
    {
        var cheeps = await _service.GetByFilter(AuthorName, (PageNo-1)*32);
        Cheeps = cheeps.ToList();
        return Page();
    }
}

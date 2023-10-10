using Chirp.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly IRepository<Cheep, string> _service;
    public List<Cheep> Cheeps { get; set; }

    public UserTimelineModel(IRepository<Cheep, string> service)
    {
        _service = service;
    }

    [FromQuery(Name = "page")]
    public int PageNo {get; set;}
    public async Task<IActionResult> OnGetAsync(string AuthorID)
    {
        var cheeps = await _service.GetByFilter(AuthorID, (PageNo-1)*32);
        Cheeps = cheeps.ToList();
        return Page();
    }
}

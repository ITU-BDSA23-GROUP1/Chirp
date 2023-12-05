using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;
using System.ComponentModel.DataAnnotations;

namespace Chirp.Razor.Pages;

public class AboutMeModel : PageModel
{
    private readonly ICheepRepository<CheepDTO, string> _service;
    private readonly IAuthorRepository<AuthorDTO, string> _authorService;
    public List<CheepDTO> Cheeps { get; set; }
    public List<string> Following { get; set; }
    [BindProperty]
    public CheepDTO CheepDTO { get; set; }

    public AboutMeModel(ICheepRepository<CheepDTO, string> service, IAuthorRepository<AuthorDTO, string> authorService)
    {
        _service = service;
        _authorService = authorService;
    }

    [FromQuery(Name = "page")]
    public int PageNo { get; set; }
    public async Task<IActionResult> OnGetAsync()
    {
        if (User.Identity.IsAuthenticated)
        {
            var username = User.Identity.Name;
            IEnumerable<CheepDTO> cheeps = await _service.GetByFilter(username, (PageNo - 1) * 32);
            Cheeps = cheeps.ToList();

            AuthorDTO loggedInAuthor = await _authorService.FindAuthorByName(username);
            IEnumerable<string> followingUserNames = await _authorService.GetFollowingNames(loggedInAuthor);
            Following = followingUserNames.ToList();
        }
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteCheep()
    {
        if (CheepDTO != null)
        {
            await _service.DeleteCheep(CheepDTO.Id);
        }

        return RedirectToPage("/AboutMe");
    }
}

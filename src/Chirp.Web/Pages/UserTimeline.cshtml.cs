using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;
using System.ComponentModel.DataAnnotations;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepRepository<CheepDTO, string> _service;
    public List<CheepDTO> Cheeps { get; set; }
    [BindProperty]
    public CheepDTO CheepDTO { get; set; }

    readonly UserManager<Author> _userManager;

    public UserTimelineModel(ICheepRepository<CheepDTO, string> service, UserManager<Author> userManager)
    {
        _service = service;
        _userManager = userManager;

    }


    [FromQuery(Name = "page")]
    public int PageNo { get; set; }
    public async Task<IActionResult> OnGetAsync(string AuthorName)
    {
        var cheeps = await _service.GetByFilter(AuthorName, (PageNo - 1) * 32);
        Cheeps = cheeps.ToList();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {

        if (!ModelState.IsValid)
        {
            // Handle invalid model state if necessary
            Page();
        }

        // Existing logic to create a Cheep instance
        if (CheepDTO != null && CheepDTO.Text != null)
        {
            //Console.WriteLine("Identity: " + User.Identity.Name);
            //var author = await _authorService.FindAuthorByName(User.Identity.Name);
            var user = await _userManager.GetUserAsync(User);
            var author = new AuthorDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };
            //Console.WriteLine("id: " + user.UserName);
            var cheepDTO = new CheepDTO
            {
                Text = CheepDTO.Text,
                TimeStamp = DateTime.Now,
                Author = author
            };

            await _service.CreateCheep(cheepDTO);
        }
        return RedirectToPage("/UserTimeline");
    }
}

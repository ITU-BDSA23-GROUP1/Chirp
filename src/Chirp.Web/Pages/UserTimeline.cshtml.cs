using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;
using System.ComponentModel.DataAnnotations;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepRepository<CheepDTO, string> _service;
    private readonly IAuthorRepository<AuthorDTO, string> _authorService;
    public List<CheepDTO> Cheeps { get; set; }
    public List<string> Following { get; set; }

    [BindProperty]
    public CheepDTO CheepDTO { get; set; }
    [BindProperty]
    public AuthorDTO AuthorDTO { get; set; }

    readonly UserManager<Author> _userManager;

    public UserTimelineModel(ICheepRepository<CheepDTO, string> service, IAuthorRepository<AuthorDTO, string> authorService, UserManager<Author> userManager)
    {
        _service = service;
        _authorService = authorService;
        _userManager = userManager;
    }


    [FromQuery(Name = "page")]
    public int PageNo { get; set; }
    public async Task<IActionResult> OnGetAsync(string AuthorName)
    {
        IEnumerable<CheepDTO> cheeps = await _service.GetByFilter(AuthorName, (PageNo - 1) * 32);

        if (User.Identity.IsAuthenticated)
        {
            var user = await _userManager.GetUserAsync(User);
            var author = new AuthorDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };
            var following = await _authorService.GetFollowing(author);
            Following = following.ToList();
            if (User.Identity.Name == AuthorName)
            {
                var authorIds = following.ToList();
                authorIds.Add(user.Id);
                cheeps = await _service.GetByFollowers(authorIds, (PageNo - 1) * 32);
            }
        }
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
            var user = await _userManager.GetUserAsync(User);
            var author = new AuthorDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };
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

    public async Task<IActionResult> OnPostFollow()
    {
        if (AuthorDTO != null)
        {
            var user = await _userManager.GetUserAsync(User);
            var author = new AuthorDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };
            await _authorService.FollowAuthor(author, AuthorDTO);
        }

        return RedirectToPage("/UserTimeline");
    }

    public async Task<IActionResult> OnPostUnfollow()
    {
        if (AuthorDTO != null)
        {
            var user = await _userManager.GetUserAsync(User);
            var author = new AuthorDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };
            await _authorService.UnfollowAuthor(author, AuthorDTO);
        }

        return RedirectToPage("/UserTimeline");
    }
}

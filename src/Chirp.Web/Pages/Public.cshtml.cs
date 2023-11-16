using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Infrastructure;
using Chirp.Core;
using NuGet.Protocol;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepRepository<CheepDTO, string> _service;
    private readonly IAuthorRepository<AuthorDTO, string> _authorService;
    public List<CheepDTO> Cheeps { get; set; }
    public List<AuthorDTO> Following { get; set; }

    //Defines the CheepDTO property
    [BindProperty]
    public CheepDTO CheepDTO { get; set; }

    UserManager<Author> _userManager;

    public PublicModel(ICheepRepository<CheepDTO, string> service, IAuthorRepository<AuthorDTO, string> authorService, UserManager<Author> userManager)
    {
        _service = service;
        _authorService = authorService;
        _userManager = userManager;
    }

    [FromQuery(Name = "page")]
    public int PageNo { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var cheeps = await _service.Get((PageNo - 1) * 32);
        Cheeps = cheeps.ToList();
        if (User.Identity.IsAuthenticated)
        {
            var user = await _userManager.GetUserAsync(User);
            var author = new AuthorDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };
            var following = _authorService.GetFollowing(author);
            Following = following.ToList();
        }

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
        return RedirectToPage("/Public");
    }

    public void OnPostFollow(AuthorDTO authorToFollowDTO)
    {
        var user = _userManager.GetUserAsync(User).Result;
        var authorDTO = new AuthorDTO
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email
        };
        _authorService.FollowAuthor(authorDTO, authorToFollowDTO);
        RedirectToPage("/Public");
    }

    public void OnPostUnfollow(AuthorDTO authorToUnfollowDTO)
    {
        var user = _userManager.GetUserAsync(User).Result;
        var authorDTO = new AuthorDTO
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email
        };
        _authorService.UnfollowAuthor(authorDTO, authorToUnfollowDTO);
        RedirectToPage("/Public");
    }

}

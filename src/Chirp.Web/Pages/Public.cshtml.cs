using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Infrastructure;
using Chirp.Core;
using NuGet.Protocol;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepRepository<CheepDTO, string> _service;
    private readonly IAuthorRepository<AuthorDTO, string> _authorService;
    public List<CheepDTO> Cheeps { get; set; }

    //Defines the CheepDTO property
    [BindProperty]
    public CheepDTO CheepDTO { get; set; }

    public PublicModel(ICheepRepository<CheepDTO, string> service, IAuthorRepository<AuthorDTO, string> authorService)
    {
        _service = service;
        _authorService = authorService;
    }

    [FromQuery(Name = "page")]
    public int PageNo {get; set;}
    
    public async Task<IActionResult> OnGetAsync()
{
    var cheeps = await _service.Get((PageNo - 1) * 32);
    Cheeps = cheeps.ToList();
    return Page();
}
    public async void OnPost()
    {

        if (!ModelState.IsValid){
        // Handle invalid model state if necessary
         Page();
         }

    // Existing logic to create a Cheep instance
    if (CheepDTO != null && CheepDTO.Text != null)
    {
        Console.WriteLine("Identity: " + User.Identity.Name);
        var author = await _authorService.FindAuthorByName(User.Identity.Name);
        Console.WriteLine("test");
        var cheepDTO = new CheepDTO
        {
            Text = CheepDTO.Text,
            TimeStamp = DateTime.Now,
            Author = author
        };
             
        _service.CreateCheep(cheepDTO);
    }

  
    RedirectToPage("/Public"); 
        
    }

}

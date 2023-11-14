namespace Chirp.Core;
using System.ComponentModel.DataAnnotations;

public class CheepDTO
{
    [StringLength(160, MinimumLength = 1, ErrorMessage = "*message must be between 1 character and 280")]
    public required string Text { get; set; }
    public required DateTime TimeStamp { get; set; }
    public required AuthorDTO Author { get; set; }


}

public class AuthorDTO
{
    public required string Id { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public List<CheepDTO> Cheeps { get; } = new();

}

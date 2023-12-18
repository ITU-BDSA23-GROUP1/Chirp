namespace Chirp.Core;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// This class represents a data transfer object for a Cheep.
/// </summary>
public class CheepDTO
{
    public required string Id { get; set; }
    [StringLength(160, MinimumLength = 1, ErrorMessage = "*message must be between 1 character and 160")]
    public required string Text { get; set; }
    public required DateTime TimeStamp { get; set; }
    public required AuthorDTO Author { get; set; }
}

/// <summary>
/// This class represents a data transfer object for an Author.
/// </summary>
public class AuthorDTO
{
    public required string Id { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public List<CheepDTO> Cheeps { get; } = new();
    public List<AuthorDTO> Following { get; } = new();
    public List<AuthorDTO> Followers { get; } = new();
}

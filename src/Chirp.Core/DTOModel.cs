namespace Chirp.Core;

public class CheepDTO
{
    public required string text { get; set; }
    public required DateTime timeStamp { get; set; }
    public required AuthorDTO author { get; set; }

}

public class AuthorDTO
{
    public required int authorID { get; set; }
    public required string name { get; set; }
    public List<CheepDTO> cheeps { get; } = new();

}

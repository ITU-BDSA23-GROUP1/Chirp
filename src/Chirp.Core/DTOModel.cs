namespace Chirp.Core;

public class CheepDTO
{
    public string text { get; set; }
    public DateTime timeStamp { get; set; }
    public AuthorDTO author { get; set; }

}

public class AuthorDTO
{
    public int authorID { get; set; }
    public string name { get; set; }
    public List<CheepDTO> cheeps { get; } = new();

}

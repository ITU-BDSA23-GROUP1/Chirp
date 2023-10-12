namespace Chirp.EF;
using Microsoft.EntityFrameworkCore;

public class ChirpDBContext : DbContext
{
    public DbSet<Cheep> cheeps { get; set; }
    public DbSet<Author> authors { get; set; }

    public ChirpDBContext(DbContextOptions<ChirpDBContext> options) : base(options) { }

}

public class Cheep
{
    public int cheepID { get; set; }
    public string text { get; set; }
    public DateTime timeStamp { get; set; }
    public Author author { get; set; }

}

public class Author
{
    public int authorID { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public List<Cheep> cheeps { get; } = new();

}

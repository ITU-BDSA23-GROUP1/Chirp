namespace Chirp.EF;
using Microsoft.EntityFrameworkCore;

public class CheepContext : DbContext
{
    public DbSet<Cheep> cheeps { get; set; }
    public DbSet<Author> authors { get; set; }

    public string DbPath { get; }

    public CheepContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "cheep.db");
        System.Console.WriteLine(DbPath);
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
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

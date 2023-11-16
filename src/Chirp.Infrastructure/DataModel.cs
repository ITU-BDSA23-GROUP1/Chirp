using System.ComponentModel.DataAnnotations;

namespace Chirp.Infrastructure;

public class ChirpDBContext : IdentityDbContext<Author>
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }

    public ChirpDBContext(DbContextOptions<ChirpDBContext> options) : base(options) { }
}

public class Cheep
{
    public required Guid CheepId { get; set; }

    [StringLength(160, MinimumLength = 1, ErrorMessage = "*message must be between 1 character and 280")]
    public required string Text { get; set; }
    public required DateTime TimeStamp { get; set; }
    //public required Guid AuthorId { get; set; }
    public required Author Author { get; set; }

}

public class Author : IdentityUser
{
    //public Guid AuthorId { get; set; }
    //public required string Name { get; set; }
    override public string UserName { get; set; }
    public override string Email { get; set; }
    public required List<Cheep> Cheeps { get; set; }
    public List<Author> Following { get; } = new();
}

using System.ComponentModel.DataAnnotations;

namespace Chirp.Infrastructure;

/// <summary>
/// This class represents the database context for the Chirp-application.
/// The database context inherits from IdentityDbContext, which is a class 
/// from the Microsoft.AspNetCore.Identity.EntityFrameworkCore namespace.
/// </summary>
public class ChirpDBContext : IdentityDbContext<Author>
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }

    public ChirpDBContext(DbContextOptions<ChirpDBContext> options) : base(options) { }
}

/// <summary>
/// This class represents a Cheep.
/// A Cheep is a message that is posted by an Author to the Chirp-application.
/// </summary>
public class Cheep
{
    public required Guid CheepId { get; set; }

    [StringLength(160, MinimumLength = 1, ErrorMessage = "*message must be between 1 character and 160")]
    public required string Text { get; set; }
    public required DateTime TimeStamp { get; set; }
    public required Author Author { get; set; }

}

/// <summary>
/// This class represents an Author, which is an authenticated user of the Chirp-application.
/// Author inherits from IdentityUser, which is a class from the Microsoft.AspNetCore.Identity.EntityFrameworkCore namespace.
/// From IdentityUser, Author inherits the properties Id, UserName and Email.
/// Author has a list of Cheeps that the Author has posted. 
/// Author has a list of Authors that the Author is following, and a list of Authors that are following the Author.
/// </summary>
public class Author : IdentityUser
{
    override public string UserName { get; set; }
    public override string Email { get; set; }
    public required List<Cheep> Cheeps { get; set; }
    public List<Author> Following { get; } = new();
    public List<Author> Followers { get; } = new();
}

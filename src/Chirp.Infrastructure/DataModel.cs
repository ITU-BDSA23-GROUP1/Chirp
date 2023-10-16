﻿namespace Chirp.Infrastructure;

public class ChirpDBContext : DbContext
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }

    public ChirpDBContext(DbContextOptions<ChirpDBContext> options) : base(options) { }

}

public class Cheep
{
    public required int CheepId { get; set; }
    public required string Text { get; set; }
    public required DateTime TimeStamp { get; set; }
    public required int AuthorId { get; set; }
    public required Author Author { get; set; }

}

public class Author
{
    public int AuthorId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required List<Cheep> Cheeps { get; set; }
}

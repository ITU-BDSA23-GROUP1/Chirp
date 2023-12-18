namespace Chirp.Tests.Infrastructure;

using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

// Inspiration for IDisposable from:
// https://xunit.net/docs/shared-context
public class IntegrationTest : IDisposable
{
    public SqliteConnection _connection;
    public DbContextOptionsBuilder<ChirpDBContext> _builder;
    public ChirpDBContext _context;
    public CheepRepository _cheepRepo;
    public AuthorRepository _authorRepo;
    public IntegrationTest()
    {

        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        _builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(_connection);
        _context = new ChirpDBContext(_builder.Options);
        _context.Database.EnsureCreatedAsync(); // Applies the schema to the database
        _cheepRepo = new CheepRepository(_context);
        _authorRepo = new AuthorRepository(_context);
    }

    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();
        _context.Dispose();
    }

    [Fact]
    public async void FindAuthorByName_FollowAuthor_CreateCheep_GetByFollowers_CheckIfCheepsFromFollowersAreReturned()
    {
        //Arrange
        Author author1 = new Author
        {
            UserName = "John Doe",
            Email = "john@email.dk",
            Cheeps = new List<Cheep>(),
        };

        Author author2 = new Author
        {
            UserName = "Janet Doe",
            Email = "janet@email.dk",
            Cheeps = new List<Cheep>(),
        };

        Author author3 = new Author
        {
            UserName = "Joe Smith",
            Email = "joe@email.dk",
            Cheeps = new List<Cheep>(),
        };

        _context.Add(author1);
        _context.Add(author2);
        _context.Add(author3);
        _context.SaveChanges();

        var author1ByName = await _authorRepo.FindAuthorByName("John Doe");
        var author3ByName = await _authorRepo.FindAuthorByName("Joe Smith");

        await _authorRepo.FollowAuthor(author3ByName, author1ByName);

        Guid cheepGuid1 = Guid.NewGuid();
        Guid cheepGuid2 = Guid.NewGuid();
        Guid cheepGuid3 = Guid.NewGuid();

        CheepDTO cheep1 = new CheepDTO
        {
            Id = cheepGuid1.ToString(),
            Text = "Hello World",
            TimeStamp = DateTime.Now,
            Author = author1ByName,
        };

        CheepDTO cheep2 = new CheepDTO
        {
            Id = cheepGuid2.ToString(),
            Text = "Hello to you",
            TimeStamp = DateTime.Now,
            Author = author1ByName,
        };

        CheepDTO cheep3 = new CheepDTO
        {
            Id = cheepGuid3.ToString(),
            Text = "Hello John",
            TimeStamp = DateTime.Now,
            Author = author3ByName,
        };

        //Act
        await _cheepRepo.CreateCheep(cheep1);
        await _cheepRepo.CreateCheep(cheep2);
        await _cheepRepo.CreateCheep(cheep3);

        List<string> authornames = new List<string>
        {
            author1ByName.Id
        };

        var cheepResult = await _cheepRepo.GetByFollowers(authornames, 0);

        //Assert
        Assert.Equal(2, cheepResult.Count());
    }

}
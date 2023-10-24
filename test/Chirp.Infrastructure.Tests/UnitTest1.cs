namespace Chirp.Tests.Infrastructure;

using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

// Inspiration for IDisposable from:
// https://xunit.net/docs/shared-context
public class UnitTestsInfrastructure : IDisposable
{
    public SqliteConnection _connection;
    public DbContextOptionsBuilder<ChirpDBContext> _builder;
    public ChirpDBContext _context;
    public CheepRepository _cheepRepo;
    public AuthorRepository _authorRepo;
    public UnitTestsInfrastructure()
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
    public void AddCheep_CheckCheepEntity()
    {
        //Arrange
        Guid authorGuid = Guid.NewGuid();
        Guid cheepGuid = Guid.NewGuid();

        Cheep cheep = new Cheep
        {
            Text = "Hello World",
            TimeStamp = DateTime.Now,
            Author = new Author
            {
                Name = "John Doe",
                Email = "john@email.dk",
                Cheeps = new List<Cheep>(),
                AuthorId = authorGuid
            },
            CheepId = cheepGuid,
            AuthorId = authorGuid
        };

        //Act
        _context.Add(cheep);
        _context.SaveChanges();

        //Assert
        Assert.Equal(cheep, _context.Cheeps.Find(cheepGuid));
    }

    [Fact]
    public void AddCheep_CheckCheepEntity_Fail()
    {
        //Arrange
        Guid authorGuid = Guid.NewGuid();
        Guid cheepGuid = Guid.NewGuid();

        Cheep cheep = new Cheep
        {
            Text = "Hello World",
            TimeStamp = DateTime.Now,
            Author = new Author
            {
                Name = "John Doe",
                Email = "john@email.dk",
                Cheeps = new List<Cheep>(),
                AuthorId = authorGuid
            },
            CheepId = cheepGuid,
            AuthorId = authorGuid
        };

        //Act
        _context.Add(cheep);
        _context.SaveChanges();

        //Assert
        Assert.NotEqual(cheep, _context.Cheeps.Find(Guid.NewGuid()));
    }

    [Fact]
    public void GetByFilter_OnlyFetchCheepsFromSpecificAuthor()
    {
        //Arrange
        Guid authorGuid1 = Guid.NewGuid();
        Guid authorGuid2 = Guid.NewGuid();

        Author author1 = new Author
        {
            Name = "John Doe",
            Email = "john@email.dk",
            Cheeps = new List<Cheep>(),
            AuthorId = authorGuid1
        };
        Author author2 = new Author
        {
            Name = "Janet Doe",
            Email = "janet@email.dk",
            Cheeps = new List<Cheep>(),
            AuthorId = authorGuid2
        };

        Guid cheepGuid1 = Guid.NewGuid();
        Guid cheepGuid2 = Guid.NewGuid();
        Guid cheepGuid3 = Guid.NewGuid();

        Cheep cheep1 = new Cheep
        {
            Text = "Hello World",
            TimeStamp = DateTime.Now,
            Author = author1,
            CheepId = cheepGuid1,
            AuthorId = author1.AuthorId
        };

        Cheep cheep2 = new Cheep
        {
            Text = "Hello Janet",
            TimeStamp = DateTime.Now,
            Author = author1,
            CheepId = cheepGuid2,
            AuthorId = author1.AuthorId
        };

        Cheep cheep3 = new Cheep
        {
            Text = "Hello John",
            TimeStamp = DateTime.Now,
            Author = author2,
            CheepId = cheepGuid3,
            AuthorId = author2.AuthorId
        };

        //Act
        _context.Add(cheep1);
        _context.Add(cheep2);
        _context.Add(cheep3);
        _context.SaveChanges();

        //Assert
        Assert.Equal(2, _context.Cheeps.Where(c => c.Author.Name == "John Doe").Count());
    }

    [Fact]
    public async void FindAuthorByName()
    {
        //Arrange
        Author author1 = new Author
        {
            Name = "John Doe",
            Email = "john@john.dk",
            Cheeps = new List<Cheep>(),
            AuthorId = Guid.NewGuid()
        };

        Author author2 = new Author
        {
            Name = "Janet Doe",
            Email = "janet@janet.dk",
            Cheeps = new List<Cheep>(),
            AuthorId = Guid.NewGuid()
        };

        //Act
        _context.Add(author1);
        _context.Add(author2);
        _context.SaveChanges();

        var authorResult = await _authorRepo.FindAuthorByName("John Doe");

        //Assert
        Assert.Equal(author1.Name, authorResult.Name);
        Assert.Equal(author1.AuthorId, authorResult.AuthorId);
    }

    [Fact]
    public async void FindAuthorByName_NotFound()
    {
        //Arrange


        //Act
        var authorResult = await _authorRepo.FindAuthorByName("John Boe");

        //Assert
        Assert.Null(authorResult);
    }

    [Fact]
    public async void FindAuthorByEmail()
    {

        //Arrange
        Author author1 = new Author
        {
            Name = "John Doe",
            Email = "john@john.dk",
            Cheeps = new List<Cheep>(),
            AuthorId = Guid.NewGuid()
        };

        Author author2 = new Author
        {
            Name = "Janet Doe",
            Email = "janet@janet.dk",
            Cheeps = new List<Cheep>(),
            AuthorId = Guid.NewGuid()
        };

        //Act
        _context.Add(author1);
        _context.Add(author2);
        _context.SaveChanges();

        var authorResult = await _authorRepo.FindAuthorByEmail("john@john.dk");

        //Assert
        Assert.Equal(author1.Name, authorResult.Name);
        Assert.Equal(author1.AuthorId, authorResult.AuthorId);
    }

    [Fact]
    public void CreateAuthor_CheckIfAuthorCreateCorrectly()
    {
        //Arrange
        AuthorDTO authorDTO = new AuthorDTO
        {
            AuthorId = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john@john.dk"
        };

        //Act
        _authorRepo.CreateAuthor(authorDTO);

        //Assert
        Assert.Equal(1, _context.Authors.Where(a => a.Name == "John Doe").Count());

    }

    [Fact]
    public void CreateCheep_CheckIfCheepCreateCorrectly()
    {
        //Arrange
        DateTime timeStamp = DateTime.Now;
        AuthorDTO authorDTO = new AuthorDTO
        {
            AuthorId = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john@john.dk"
        };
        CheepDTO cheepDTO = new CheepDTO
        {
            Text = "Hello World",
            TimeStamp = timeStamp,
            Author = authorDTO
        };

        //Act
        _cheepRepo.CreateCheep(cheepDTO);

        //Assert
        Assert.Equal(1, _context.Cheeps.Where(c => c.Author.Name == "John Doe").Count());
        Assert.Equal(1, _context.Cheeps.Where(c => c.Text == "Hello World").Count());
        Assert.Equal(1, _context.Cheeps.Where(c => c.TimeStamp == timeStamp).Count());
    }

}
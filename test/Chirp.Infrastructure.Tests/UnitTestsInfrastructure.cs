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
                UserName = "John Doe",
                Email = "john@email.dk",
                Cheeps = new List<Cheep>(),
                Id = authorGuid.ToString()
            },
            CheepId = cheepGuid
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
                UserName = "John Doe",
                Email = "john@email.dk",
                Cheeps = new List<Cheep>(),
                Id = authorGuid.ToString()
            },
            CheepId = cheepGuid,
        };

        //Act
        _context.Add(cheep);
        _context.SaveChanges();

        //Assert
        Assert.NotEqual(cheep, _context.Cheeps.Find(Guid.NewGuid()));
    }

    [Fact]
    public async Task GetByFilter_OnlyFetchCheepsFromSpecificAuthorAsync()
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

        Guid cheepGuid1 = Guid.NewGuid();
        Guid cheepGuid2 = Guid.NewGuid();
        Guid cheepGuid3 = Guid.NewGuid();

        Cheep cheep1 = new Cheep
        {
            Text = "Hello World",
            TimeStamp = DateTime.Now,
            Author = author1,
            CheepId = cheepGuid1,
        };

        Cheep cheep2 = new Cheep
        {
            Text = "Hello Janet",
            TimeStamp = DateTime.Now,
            Author = author1,
            CheepId = cheepGuid2,
        };

        Cheep cheep3 = new Cheep
        {
            Text = "Hello John",
            TimeStamp = DateTime.Now,
            Author = author2,
            CheepId = cheepGuid3,
        };

        //Act
        _context.Add(cheep1);
        _context.Add(cheep2);
        _context.Add(cheep3);
        _context.SaveChanges();

        var cheepResult = await _cheepRepo.GetByFilter("John Doe", 0);

        //Assert
        Assert.Equal(2, cheepResult.Count());
    }

    [Fact]
    public async void GetByFollowers_CheckIfCheepsFromFollowersAreReturned()
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

        author3.Following.Add(author1);

        _context.Add(author1);
        _context.Add(author2);
        _context.Add(author3);
        _context.SaveChanges();

        Guid cheepGuid1 = Guid.NewGuid();
        Guid cheepGuid2 = Guid.NewGuid();
        Guid cheepGuid3 = Guid.NewGuid();

        Cheep cheep1 = new Cheep
        {
            Text = "Hello World",
            TimeStamp = DateTime.Now,
            Author = author1,
            CheepId = cheepGuid1,
        };

        Cheep cheep2 = new Cheep
        {
            Text = "Hello to you",
            TimeStamp = DateTime.Now,
            Author = author1,
            CheepId = cheepGuid2,
        };

        Cheep cheep3 = new Cheep
        {
            Text = "Hello John",
            TimeStamp = DateTime.Now,
            Author = author3,
            CheepId = cheepGuid3,
        };

        //Act
        _context.Add(cheep1);
        _context.Add(cheep2);
        _context.Add(cheep3);
        _context.SaveChanges();

        List<string> authorIds = new List<string>
        {
            author1.Id
        };

        var cheepResult = await _cheepRepo.GetByFollowers(authorIds, 0);

        //Assert
        Assert.Equal(2, cheepResult.Count());
    }

    [Fact]
    public async void FindAuthorByName_CheckIfIdAndNameIsCorrect()
    {
        //Arrange
        Author author1 = new Author
        {
            UserName = "John Doe",
            Email = "john@john.dk",
            Cheeps = new List<Cheep>(),
            Id = Guid.NewGuid().ToString()
        };

        Author author2 = new Author
        {
            UserName = "Janet Doe",
            Email = "janet@janet.dk",
            Cheeps = new List<Cheep>(),
            Id = Guid.NewGuid().ToString()
        };

        //Act
        _context.Add(author1);
        _context.Add(author2);
        _context.SaveChanges();

        var authorResult = await _authorRepo.FindAuthorByName("John Doe");

        //Assert
        Assert.Equal(author1.UserName, authorResult.UserName);
        Assert.Equal(author1.Id, authorResult.Id);
    }

    [Fact]
    public async void FindAuthorByName_CheckIfNullReturnedIfNoAuthorFound()
    {
        //Arrange


        //Act
        var authorResult = await _authorRepo.FindAuthorByName("John Boe");

        //Assert
        Assert.Null(authorResult);
    }

    [Fact]
    public async void CreateCheep_CheckIfCheepCreateCorrectly()
    {
        //Arrange
        DateTime timeStamp = DateTime.Now;
        AuthorDTO authorDTO = new AuthorDTO
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "John Doe",
            Email = "john@john.dk"
        };

        CheepDTO cheepDTO = new CheepDTO
        {
            Id = Guid.NewGuid().ToString(),
            Text = "Hello World",
            TimeStamp = timeStamp,
            Author = authorDTO
        };

        //Act
        await _cheepRepo.CreateCheep(cheepDTO);

        //Assert
        Assert.Equal(1, _context.Cheeps.Where(c => c.Text == "Hello World").Count());
        Assert.Equal(1, _context.Cheeps.Where(c => c.TimeStamp == timeStamp).Count());
    }

    [Fact]
    public async void DeleteCheep_CheckIfCheepAreDeletedOnRequest()
    {
        //Arrange
        Author author = new Author
        {
            UserName = "John Doe",
            Email = "john@email.dk",
            Cheeps = new List<Cheep>(),
        };

        _context.Add(author);
        _context.SaveChanges();

        Guid cheepGuid = Guid.NewGuid();

        Cheep cheep = new Cheep
        {
            Text = "Hello World",
            TimeStamp = DateTime.Now,
            Author = author,
            CheepId = cheepGuid,
        };

        //Act
        _context.Add(cheep);
        _context.SaveChanges();

        var deletedCheep = await _cheepRepo.DeleteCheep(cheepGuid.ToString().ToUpper());

        //Assert
        Assert.True(deletedCheep);
        Assert.False(_context.Cheeps.Any());
    }

    [Fact]
    public async void FollowAuthor_CheckFollowingListContainsOneAuthor()
    {
        //Arrange

        var authorWillFollowID = Guid.NewGuid().ToString();
        var authorToBeFollowedID = Guid.NewGuid().ToString();

        Author authorWillFollow = new Author
        {
            UserName = "John Doe",
            Email = "john@john.dk",
            Id = authorWillFollowID,
            Cheeps = new List<Cheep>()
        };

        Author authorToBeFollowed = new Author
        {
            UserName = "Janet Doe",
            Email = "janet@janet.dk",
            Id = authorToBeFollowedID,
            Cheeps = new List<Cheep>()
        };

        AuthorDTO authorWillFollowDTO = new AuthorDTO
        {
            UserName = "John Doe",
            Email = "john@john.dk",
            Id = authorWillFollowID
        };

        AuthorDTO authorToBeFollowedDTO = new AuthorDTO
        {
            UserName = "Janet Doe",
            Email = "janet@janet.dk",
            Id = authorToBeFollowedID
        };

        //Act

        _context.Add(authorWillFollow);
        _context.Add(authorToBeFollowed);
        _context.SaveChanges();

        await _authorRepo.FollowAuthor(authorWillFollowDTO, authorToBeFollowedDTO);

        //Assert
        Assert.Equal(1, _context.Authors.Where(a => a.Following.Contains(authorToBeFollowed)).Count());
    }

}
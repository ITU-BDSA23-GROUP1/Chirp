namespace Chirp.Tests.Infrastructure;

using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public class UnitTestsInfrastructure
{
    [Fact]
    public async void AddCheep_CheckCheepEntity()
    {
        //Arrange
        using var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);
        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync(); // Applies the schema to the database
        var repository = new CheepRepository(context);

        //Act
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

        context.Add(cheep);
        context.SaveChanges();

        //Assert
        Assert.Equal(cheep, context.Cheeps.Find(cheepGuid));

    }

    [Fact]
    public async void AddCheep_CheckCheepEntity_Fail()
    {
        //Arrange
        using var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);
        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync(); // Applies the schema to the database
        var repository = new CheepRepository(context);

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
        context.Add(cheep);
        context.SaveChanges();

        //Assert
        Assert.NotEqual(cheep, context.Cheeps.Find(Guid.NewGuid()));
    }

    [Fact]
    public async void GetByFilter_OnlyFetchCheepsFromSpecificAuthor()
    {
        //Arrange
        using var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);
        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync(); // Applies the schema to the database
        var repository = new CheepRepository(context);


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
        context.Add(cheep1);
        context.Add(cheep2);
        context.Add(cheep3);
        context.SaveChanges();

        //Assert
        Assert.Equal(2, context.Cheeps.Where(c => c.Author.Name == "John Doe").Count());
    }

    [Fact]
    public async void FindAuthorByName()
    {

        //Arrange
        using var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);
        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync(); // Applies the schema to the database
        var repository = new CheepRepository(context);

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
        context.Add(author1);
        context.Add(author2);
        context.SaveChanges();

        var authorResult = await repository.FindAuthorByName("John Doe");

        //Assert
        Assert.Equal(author1.Name, authorResult.Name);
        Assert.Equal(author1.AuthorId, authorResult.AuthorId);
    }

    [Fact]
    public async void FindAuthorByName_NotFound()
    {

        //Arrange
        using var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);
        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync(); // Applies the schema to the database
        var repository = new CheepRepository(context);

        //Act
        var authorResult = await repository.FindAuthorByName("John Boe");

        //Assert
        Assert.Null(authorResult);
    }

    [Fact]
    public async void FindAuthorByEmail()
    {

        //Arrange
        using var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);
        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync(); // Applies the schema to the database
        var repository = new CheepRepository(context);

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
        context.Add(author1);
        context.Add(author2);
        context.SaveChanges();

        var authorResult = await repository.FindAuthorByEmail("john@john.dk");

        //Assert
        Assert.Equal(author1.Name, authorResult.Name);
        Assert.Equal(author1.AuthorId, authorResult.AuthorId);
    }

    [Fact]
    public async void CreateAuthor_CheckIfAuthorCreateCorrectly()
    {

        //Arrange
        using var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);
        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync(); // Applies the schema to the database
        var repository = new CheepRepository(context);

        //Act
        repository.CreateAuthor("John Doe", "john@john.dk");

        //Assert
        Assert.Equal(1, context.Authors.Where(a => a.Name == "John Doe").Count());

    }

    [Fact]
    public async void CreateCheep_CheckIfCheepCreateCorrectly()
    {
        //Arrange
        using var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(connection);
        using var context = new ChirpDBContext(builder.Options);
        await context.Database.EnsureCreatedAsync(); // Applies the schema to the database
        var repository = new CheepRepository(context);

        DateTime timeStamp = DateTime.Now;
        AuthorDTO authorDTO = new AuthorDTO
        {
            AuthorId = Guid.NewGuid(),
            Name = "John Doe",
            Email = "john@john.dk"
        };

        //Act
        repository.CreateCheep("Hello World", timeStamp, authorDTO);

        //Assert
        Assert.Equal(1, context.Cheeps.Where(c => c.Author.Name == "John Doe").Count());
        Assert.Equal(1, context.Cheeps.Where(c => c.Text == "Hello World").Count());
        Assert.Equal(1, context.Cheeps.Where(c => c.TimeStamp == timeStamp).Count());
    }

}
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
        Cheep cheep = new Cheep
        {
            Text = "Hello World",
            TimeStamp = DateTime.Now,
            Author = new Author
            {
                Name = "John Doe",
                Email = "john@email.dk",
                Cheeps = new List<Cheep>(),
                AuthorId = 1
            },
            CheepId = 1,
            AuthorId = 1
        };

        context.Add(cheep);
        context.SaveChanges();

        //Assert
        Assert.Equal(cheep, context.Cheeps.Find(1));

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

        Cheep cheep = new Cheep
        {
            Text = "Hello World",
            TimeStamp = DateTime.Now,
            Author = new Author
            {
                Name = "John Doe",
                Email = "john@email.dk",
                Cheeps = new List<Cheep>(),
                AuthorId = 1
            },
            CheepId = 1,
            AuthorId = 1
        };

        //Act
        context.Add(cheep);
        context.SaveChanges();

        //Assert
        Assert.NotEqual(cheep, context.Cheeps.Find(2));
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

        Author author1 = new Author
            {
                Name = "John Doe",
                Email = "john@email.dk",
                Cheeps = new List<Cheep>(),
                AuthorId = 1
            };
        Author author2 = new Author
            {
                Name = "Janet Doe",
                Email = "janet@email.dk",
                Cheeps = new List<Cheep>(),
                AuthorId = 2
            };

        Cheep cheep1 = new Cheep
        {
            Text = "Hello World",
            TimeStamp = DateTime.Now,
            Author = author1,
            CheepId = 1,
            AuthorId = 1
        };

        Cheep cheep2 = new Cheep
        {
            Text = "Hello Janet",
            TimeStamp = DateTime.Now,
            Author = author1,
            CheepId = 2,
            AuthorId = 1
        };

        Cheep cheep3 = new Cheep
        {
            Text = "Hello John",
            TimeStamp = DateTime.Now,
            Author = author2,
            CheepId = 3,
            AuthorId = 2
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
    public async void FindAuthorByName() {

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
                AuthorId = 1
            };

        Author author2 = new Author
            {
                Name = "Janet Doe",
                Email = "janet@janet.dk",
                Cheeps = new List<Cheep>(),
                AuthorId = 2
            };

        //Act
        context.Add(author1);
        context.Add(author2);
        context.SaveChanges();

        var authorResult = await repository.FindAuthorByName("John Doe");

        //Assert
        Assert.Equal(author1.Name, authorResult.name);
        Assert.Equal(author1.AuthorId, authorResult.authorID);
    }

    [Fact]
    public async void FindAuthorByName_NotFound() {

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
    public async void FindAuthorByEmail() {

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
                AuthorId = 1
            };

        Author author2 = new Author
            {
                Name = "Janet Doe",
                Email = "janet@janet.dk",
                Cheeps = new List<Cheep>(),
                AuthorId = 2
            };

        //Act
        context.Add(author1);
        context.Add(author2);
        context.SaveChanges();

        var authorResult = await repository.FindAuthorByEmail("john@john.dk");

        //Assert
        Assert.Equal(author1.Name, authorResult.name);
        Assert.Equal(author1.AuthorId, authorResult.authorID);
    }

    [Fact]
    public async void CreateAuthor_CheckIfAuthorDTOCreateCorrectly() {

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
                AuthorId = 1
            };

        //Act
        context.Add(author1);
        context.SaveChanges();
        var authorDTO = repository.CreateAuthor(1, "John Doe");

        //Assert
        Assert.Equal(author1.Name, authorDTO.name);
        Assert.Equal(author1.AuthorId, authorDTO.authorID);

    }

    [Fact]
    public async void CreateCheep_CheckIfCheepDTOCreateCorrectly() {
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
                AuthorId = 1
            };
        DateTime timeStamp = DateTime.Now;

        Cheep cheep1 = new Cheep
        {
            Text = "Hello World",
            TimeStamp = timeStamp,
            Author = author1,
            CheepId = 1,
            AuthorId = 1
        };

        AuthorDTO authorDTO = new AuthorDTO
        {
            authorID = 1,
            name = "John Doe"
        };

        //Act
        context.Add(cheep1);
        context.SaveChanges();
        var cheepDTO = repository.CreateCheep("Hello World", timeStamp, authorDTO);

        //Assert
        Assert.Equal(cheep1.Text, cheepDTO.text);
        Assert.Equal(cheep1.TimeStamp, cheepDTO.timeStamp);
        Assert.Equal(cheep1.Author.AuthorId, cheepDTO.author.authorID);
    }

}
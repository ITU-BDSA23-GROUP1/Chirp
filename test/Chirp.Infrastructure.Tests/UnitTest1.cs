namespace Chirp.Tests.Infrastructure;
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

        //Assert

    }
}
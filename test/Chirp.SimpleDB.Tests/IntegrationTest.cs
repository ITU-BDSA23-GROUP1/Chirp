using System.ComponentModel.Design;
using SimpleDB;
using FluentAssertions;


namespace Chirp.SimpleDB.Tests;

public class IntegrationTest
{
    [Fact]
    public void StoreAndRead_EntryFromDataBase_CorrectCheep()
    {
        // Arrange

        CSVDatabase<Cheep> testDB = CSVDatabase<Cheep>.Instance;
        testDB.fileName = "../../../test_chirp.csv";

        Cheep testCheep = new Cheep { Author = "Username", Message = "Test123", Timestamp = 1694349000 };

        // Act
        testDB.Store(testCheep);
        IEnumerable<Cheep> cheeps = testDB.Read(1);
        Cheep storedCheep = cheeps.First();

        // Assert

        Assert.Equal(testCheep,storedCheep);

    }

    [Fact]
    public void StoreAndRead_EntryFromDataBase_WrongCheep()
    {

        // Arrange
        
        CSVDatabase<Cheep> testDB = CSVDatabase<Cheep>.Instance;
        testDB.fileName = "../../../test_chirp.csv";

        Cheep testCheep = new Cheep { Author = "Username", Message = "Test123", Timestamp = 1694349000 };
        Cheep testCheep2 = new Cheep { Author = "UsernameWrong", Message = "Test123Wrong", Timestamp = 1694349000 };
        
        // Act

        testDB.Store(testCheep);
        IEnumerable<Cheep> cheeps = testDB.Read(1);
        Cheep storedCheep = cheeps.First();

        // Assert

        Assert.NotEqual(testCheep2,storedCheep);

    }
}
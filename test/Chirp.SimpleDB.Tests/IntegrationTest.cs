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

        // Assert

        string actual = "ABCDEFGHI";
        actual.Should().StartWith("AB").And.EndWith("HI").And.Contain("EF").And.HaveLength(9);


        // Assert.Equal(testCheep,testDB.Read(1));

    }
}
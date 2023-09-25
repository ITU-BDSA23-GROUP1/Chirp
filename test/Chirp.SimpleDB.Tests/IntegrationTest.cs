using SimpleDB;

namespace Chirp.SimpleDB.Tests;

public class IntegrationTest
{

    private static void CreateFile()
    {
        if (File.Exists("../../../test_chirp.csv"))
        {
            File.Delete("../../../test_chirp.csv");
        }
        File.WriteAllText("../../../test_chirp.csv", "Author,Message,Timestamp");
    }

    private static void DeleteFile()
    {
        File.Delete("../../../test_chirp.csv");
    }


    [Fact]
    public void StoreAndRead_EntryFromDataBase_CorrectCheep()
    {
        // Arrange
        CreateFile();
        CSVDatabase<Cheep> testDB = CSVDatabase<Cheep>.Instance;
        testDB.fileName = "../../../test_chirp.csv";

        Cheep testCheep = new Cheep { Author = "Username", Message = "\"Test123\"", Timestamp = 1694349000 };

        // Act
        testDB.Store(testCheep);
        IEnumerable<Cheep> cheeps = testDB.Read(1);
        Cheep storedCheep = cheeps.First();

        // Assert
        Assert.Equal(testCheep.Author, storedCheep.Author);
        Assert.Equal(testCheep.Message, $"\"{storedCheep.Message}\"");
        Assert.Equal(testCheep.Timestamp, storedCheep.Timestamp);
        DeleteFile();

    }
}
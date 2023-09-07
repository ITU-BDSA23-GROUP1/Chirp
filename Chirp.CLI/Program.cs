using System;
using System.IO;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using SimpleDB;

// Reading the CSV file is inspired by:
// - https://stackoverflow.com/questions/3507498/reading-csv-files-using-c-sharp/34265869#34265869
// - https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-read-text-from-a-file

// Open the text file using a stream reader.

IDatabaseRepository<Cheep> cd = new CSVDatabase<Cheep>();
cd.fileName = "../Chirp.CLI/chirp_cli_db.csv";

try
{
    if (args[0] == "read")
    {
        Read();
    } else if (args[0] == "cheep") 
    {
        Write(args[1]);
    } 
}
catch (IOException E)
{
    Console.WriteLine("The file could not be read:");
    Console.WriteLine(E.Message);
}

void Read() {
    IEnumerable<Cheep> Records = cd.Read(10);
    foreach (Cheep CurCheep in Records)
    {
        // Following lines inspired by https://peterdaugaardrasmussen.com/2022/11/26/csharp-convert-datetimeoffset-to-and-from-unix-timestamp/#:~:text=In%20order%20to%20get%20a%20DateTime%20form%20a,DateTimeOffset.FromUnixTimeMilliseconds%281669321628392%29%3B%20var%20dateTime%20%3D%20dateTimeOffset.DateTime%3B%20That%20is%20all%21
        DateTimeOffset Time = DateTimeOffset.FromUnixTimeSeconds(CurCheep.Timestamp);
        // Following lines inspired by  https://learn.microsoft.com/en-us/dotnet/api/system.datetime.tolocaltime?view=net-7.0
        var LocalTime = Time.DateTime.ToLocalTime();
        Console.WriteLine($"{CurCheep.Author} @ {LocalTime}: {CurCheep.Message}");
    }
}


void Write(string CheepMsg) {
    DateTime CurrentTime = DateTime.Now;
    long UnixTime = ((DateTimeOffset)CurrentTime).ToUnixTimeSeconds();
    
    Cheep cheep = new Cheep {Author = Environment.UserName, Message = CheepMsg, Timestamp = UnixTime};
    
    cd.Store(cheep);
}

public record Cheep {
    public string Author { get; set; } = null!;
    public string Message { get; set; } = null!;
    public long Timestamp { get; set; }
}

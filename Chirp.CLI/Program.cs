using Chirp.CLI;
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
        UserInterface.PrintCheeps(cd.Read(10));
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

void Write(string CheepMsg) {
    DateTime CurrentTime = DateTime.Now;
    long UnixTime = ((DateTimeOffset)CurrentTime).ToUnixTimeSeconds();
    
    Cheep cheep = new Cheep {Author = Environment.UserName, Message = $"\"{CheepMsg}\"", Timestamp = UnixTime};
    
    cd.Store(cheep);
}

public record Cheep {
    public string Author { get; set; } = null!;
    public string Message { get; set; } = null!;
    public long Timestamp { get; set; }
}

using Chirp.CLI;
using SimpleDB;
using System.CommandLine;

IDatabaseRepository<Cheep> cd = new CSVDatabase<Cheep>();
cd.fileName = "../../data/chirp_cli_db.csv";

// The following is inspired by:
//  https://learn.microsoft.com/en-us/dotnet/standard/commandline/define-commands
//  https://learn.microsoft.com/en-us/dotnet/standard/commandline/get-started-tutorial
var rootCommand = new RootCommand();
var readCommand = new Command("read", "Read subcommand");
var cheepCommand = new Command("cheep", "Cheep subcommand");
var cheepArgument = new Argument<string>
    ("cheepMsg", "The message written to the csv file");

rootCommand.Add(readCommand);
rootCommand.Add(cheepCommand);
cheepCommand.Add(cheepArgument);

readCommand.SetHandler(() =>
{
    UserInterface.PrintCheeps(cd.Read(10));
});
cheepCommand.SetHandler((cheepArgumentValue) =>
{
    Write(cheepArgumentValue);
}, cheepArgument);

await rootCommand.InvokeAsync(args);

static long getUnixTime(DateTime dateTime)
{
    long UnixTime = ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
    return UnixTime;
}

void Write(string CheepMsg)
{
    long UnixTime = getUnixTime(DateTime.Now);

    Cheep cheep = new Cheep { Author = Environment.UserName, Message = $"\"{CheepMsg}\"", Timestamp = UnixTime };

    cd.Store(cheep);
}

public record Cheep
{
    public string Author { get; set; } = null!;
    public string Message { get; set; } = null!;
    public long Timestamp { get; set; }
}

public partial class Program {

}

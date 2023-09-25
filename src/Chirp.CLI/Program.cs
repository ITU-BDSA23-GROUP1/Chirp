using Chirp.CLI;
using System.CommandLine;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

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

var baseURL = "http://localhost:5000";
using HttpClient client = new();
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
client.BaseAddress = new Uri(baseURL);

readCommand.SetHandler(async () =>
{
    var cheeps = await client.GetFromJsonAsync<IEnumerable<Cheep>>("cheeps");
    UserInterface.PrintCheeps(cheeps);
});
cheepCommand.SetHandler(async (cheepArgumentValue) =>
{
    await WriteAsync(cheepArgumentValue);
}, cheepArgument);

await rootCommand.InvokeAsync(args);


async Task WriteAsync(string CheepMsg)
{
    long UnixTime = UserInterface.getUnixTime(DateTime.Now);

    Cheep cheep = new Cheep { Author = Environment.UserName, Message = $"\"{CheepMsg}\"", Timestamp = UnixTime };

    JsonContent content = JsonContent.Create(cheep);

    var response = await client.PostAsync("/cheep", content);

     if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("POST request successful.");
            }
            else
            {
                Console.WriteLine("POST request failed with status code: " + response.StatusCode);
            }


    //var responseString = await response.Content.ReadAsStringAsync();
}

public record Cheep
{
    public string Author { get; set; } = null!;
    public string Message { get; set; } = null!;
    public long Timestamp { get; set; }
}


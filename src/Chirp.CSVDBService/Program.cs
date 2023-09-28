using SimpleDB;
using Chirp.CLI;


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

IDatabaseRepository<Cheep> cd = CSVDatabase<Cheep>.Instance;
string csvFileName = "../../data/chirp_cli_db.csv";
if (!File.Exists(csvFileName))
        {
            File.WriteAllText(csvFileName, "Author,Message,Timestamp");

        }
cd.fileName = csvFileName;

app.MapGet("/cheeps", () => cd.Read(10));
app.MapPost("/cheep", (Cheep cheep) => cd.Store(cheep));

app.Run();
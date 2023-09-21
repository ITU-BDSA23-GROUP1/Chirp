using SimpleDB;


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

IDatabaseRepository<Cheep> cd = CSVDatabase<Cheep>.Instance;
cd.fileName = "../../data/chirp_cli_db.csv";

app.MapGet("/cheeps", () => cd.Read(10));
app.MapPost("/cheep", (Cheep cheep) => cd.Store(cheep));

app.Run();

public record Cheep(string Author, string Message, long Timestamp);
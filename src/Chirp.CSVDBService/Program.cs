using SimpleDB;
using Chirp.CLI;


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

IDatabaseRepository<Cheep> cd = CSVDatabase<Cheep>.Instance;
cd.fileName = "../../data/chirp_cli_db.csv";

app.MapGet("/cheeps", () => cd.Read(10));
app.MapPost("/cheep", (Cheep cheep) => cd.Store(cheep));

app.Run();
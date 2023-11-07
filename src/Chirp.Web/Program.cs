
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();

// Add services to the container.
var dbFolder = Path.Join(Path.GetTempPath(), "Chirp");
if (!Directory.Exists(dbFolder))
{
    Directory.CreateDirectory(dbFolder);
}

var chirpKey = builder.Configuration["Chirp:ConnectionStrings"];

if (string.IsNullOrEmpty(chirpKey))
{
    throw new InvalidOperationException("The 'Chirp:ConnectionStrings' configuration is missing.");
}

var connection = new SqlConnection(chirpKey);
builder.Services.AddDbContext<ChirpDBContext>(options =>
    options.UseSqlServer(chirpKey));

// The following lines are inspired by: ASP.NET Core in action 3. edition by Andrew Lock
builder.Services.AddDefaultIdentity<Author>(options =>
        options.SignIn.RequireConfirmedAccount = true)
    //.AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ChirpDBContext>();

builder.Services.AddRazorPages();
//builder.Services.AddSingleton<ICheepService, CheepService>();
builder.Services.AddScoped<ICheepRepository<CheepDTO, string>, CheepRepository>();

var app = builder.Build();
using (var sp = app.Services.CreateScope())
using (var context = sp.ServiceProvider.GetRequiredService<ChirpDBContext>())
{
    if (context.Database.IsRelational())
    {
        context.Database.Migrate();
    }
    DbInitializer.SeedDatabase(context);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


// Middleware for authentication and authorization:
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();

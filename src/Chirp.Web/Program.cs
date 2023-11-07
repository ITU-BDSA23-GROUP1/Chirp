
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();

// Add services to the container.
var dbFolder = Path.Join(Path.GetTempPath(), "Chirp");
if (!Directory.Exists(dbFolder))
{
    Directory.CreateDirectory(dbFolder);
}

var chirpKey = builder.Configuration["Chirp_ConnectionStrings"];

if (string.IsNullOrEmpty(chirpKey))
{
    throw new InvalidOperationException("The 'Chirp_ConnectionStrings' configuration is missing.");
}

builder.Services.AddDbContext<ChirpDBContext>(options =>
    options.UseSqlServer(chirpKey));

// The following lines are inspired by: ASP.NET Core in action 3. edition by Andrew Lock
builder.Services.AddDefaultIdentity<Author>(options =>
        options.SignIn.RequireConfirmedAccount = true)
    //.AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ChirpDBContext>();

builder.Services.AddRazorPages();
builder.Services.AddScoped<ICheepRepository<CheepDTO, string>, CheepRepository>();


// Next two lines inspired by:
// https://stackoverflow.com/questions/31886779/asp-net-mvc-6-aspnet-session-errors-unable-to-resolve-service-for-type
builder.Services.AddMvc().AddSessionStateTempDataProvider();
builder.Services.AddSession();

/*builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".Chirp.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});*/



builder.Services.AddAuthentication(/*options =>
    {
        // options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        // options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        // options.DefaultChallengeScheme = "GitHub";
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(*/)
    .AddGitHub(o =>
    {
        o.SignInScheme = IdentityConstants.ExternalScheme;
        o.ClientId = builder.Configuration["authentication_github_clientId"];
        o.ClientSecret = builder.Configuration["GITHUB_PROVIDER_AUTHENTICATION_SECRET"];
        o.CallbackPath = "/.auth/login/github/callback";
    });


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
app.UseSession();

app.MapRazorPages();

app.Run();

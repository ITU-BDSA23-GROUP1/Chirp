using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();

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
    .AddEntityFrameworkStores<ChirpDBContext>()
    .AddSignInManager<SignInManager<Author>>();

builder.Services.AddRazorPages();

// The next lines are inspired by: 
// https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-7.0
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

builder.Services.AddScoped<ICheepRepository<CheepDTO, string>, CheepRepository>();
builder.Services.AddScoped<IAuthorRepository<AuthorDTO, string>, AuthorRepository>();

// The next lines are inspired by the supplementary slides from lecture 8:
// https://github.com/itu-bdsa/lecture_notes/blob/0531bc2647bff2be90ac254b18f6b74c22096788/sessions/session_08/Supplementary_Slides.md
builder.Services.AddAuthentication()
    .AddGitHub(o =>
    {
        o.ClientId = builder.Configuration["authentication_github_clientId"];
        o.ClientSecret = builder.Configuration["GITHUB_PROVIDER_AUTHENTICATION_SECRET"];
        o.CallbackPath = "/signin-github";
    });


var app = builder.Build();

// The next line is inspired by: 
// https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-7.0
app.UseForwardedHeaders();

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

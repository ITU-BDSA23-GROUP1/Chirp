using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.IO;
using Chirp.EF;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
//builder.Services.AddSingleton<ICheepService, CheepService>();
builder.Services.AddSingleton<IRepository<Cheep, Author>, CheepRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

/*string scriptPath = "../Chirp.SQLite/scripts/initDB.sh";

        // Check if the script file exists
        if (File.Exists(scriptPath))
        {
            // Create a process to run the script
            Process process = new Process();
            process.StartInfo.FileName = "bash"; // Use the appropriate shell (e.g., "bash" for a Bash script)
            process.StartInfo.Arguments = scriptPath;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;

            // Start the process
            process.Start();

            // Optionally, read and log the script's output
            string output = process.StandardOutput.ReadToEnd();
            Console.WriteLine(output);
            // Log or handle the output as needed

            // Wait for the process to finish
            process.WaitForExit();

            // Close the process
            process.Close();
        }
        else
        {
            // Handle the case where the script file doesn't exist
}*/



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.Run();

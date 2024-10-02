using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure Firestore
builder.Services.AddSingleton(provider =>
{
    var builder = new FirestoreDbBuilder
    {
        ProjectId = "fir-demo-8e41f",
        JsonCredentials=File.ReadAllText("D:/School/2024 Fall/Internship/DemoDPGJobData/DemoDPGJobData/.vscode/demo-8e41f-firebase-adminsdk.json")
    };
    return builder.Build();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

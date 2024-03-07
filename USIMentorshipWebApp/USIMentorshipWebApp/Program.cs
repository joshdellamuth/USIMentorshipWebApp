using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using USIMentorshipWebApp.Data;
using USIMentorshipWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

#region Adding Services 
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
// AddScoped means the service is created once per HTTP request 
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();

// AddSingleton means it is created once and used throughout the entire application 
builder.Services.AddSingleton<UserService>();
// make sure you're using https base address if you configured for https 
builder.Services.AddHttpClient<IUserService, UserService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7216/");
});
#endregion


var app = builder.Build();

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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

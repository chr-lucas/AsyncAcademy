using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AsyncAcademy.Data;
using Microsoft.Extensions.DependencyInjection.Extensions;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AsyncAcademyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FirstLastAppContext") ?? throw new InvalidOperationException("Connection string 'FirstLastAppContext' not found.")));

// From Bash
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
//

builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>(); // The Last Part

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

app.UseAuthorization();

app.UseSession(); // From Bash

app.MapRazorPages();

app.Run();
using Microsoft.EntityFrameworkCore;
using MSIT150Site.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DemoContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DemoConnection"))
    );

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
//pattern: "{controller=Home}/{action=Index}/{id?}");
//pattern: "{controller=Home}/{action=GetDemo}/{id?}");
//pattern: "{controller=Home}/{action=First}/{id?}");
pattern: "{controller=Home}/{action=Register}/{id?}");
//pattern: "{controller=Home}/{action=Address}/{id?}");
//pattern: "{controller=Home}/{action=Promise}/{id?}");
//pattern: "{controller=Home}/{action=Fetch}/{id?}");

app.Run();

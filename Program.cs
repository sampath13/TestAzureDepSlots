using Microsoft.EntityFrameworkCore;
using TestAzureDepSlots.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<Repository>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("EmployeeDb")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using var scope = app.Services.CreateScope();
var logger = scope.ServiceProvider.GetService<ILogger<Repository>>()!;
var dbContext = scope.ServiceProvider.GetService<Repository>()!;
try
{
    dbContext.ApplyPendingMigrations(logger);
}
catch (Exception e)
{
    logger.LogError(e, "Error applying migrations");
    throw;
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
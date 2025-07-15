using LV3_Cong.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5106", "https://localhost:7174");
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDataBaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var App = builder.Build();

if (!App.Environment.IsDevelopment())
{
    App.UseExceptionHandler("/Home/Error");
    App.UseHsts();
}

App.UseHttpsRedirection();
App.UseStaticFiles();
App.UseRouting();
App.UseAuthorization();

App.MapControllerRoute(
    name: "default",
    pattern: "{controller=BirthdaysCRUD}/{action=Index}/{id?}");

App.Run();

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// TODO: Register your services, repositories, and dependencies here, e.g.:
// builder.Services.AddScoped<IAdventureService, AdventureService>();
// builder.Services.AddScoped<IEnemyRepository, EnemyRepository>();
// etc.

// If you want to use SQLite with Entity Framework Core, add something like:
// builder.Services.AddDbContext<MyDbContext>(options =>
//     options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Adventure/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// MVC default route: {controller=Home}/{action=Index}/{id?}
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Adventure}/{action=Index}/{id?}");

app.Run();
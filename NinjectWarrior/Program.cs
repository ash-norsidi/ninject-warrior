using NinjectWarrior.Repositories;
using NinjectWarrior.Services;
using NinjectWarrior.Services.Strategies;
// Add your using statements for services, repositories, etc.

var builder = WebApplication.CreateBuilder(args);

// --- Register services, repositories, strategies, etc. ---

// TODO: add Logger later
//builder.Services.AddScoped<IGameLogger, ApiGameLogger>();

// Repositories
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>();
builder.Services.AddScoped<IEnemyRepository, EnemyRepository>();
builder.Services.AddScoped<IPuzzleRepository, PuzzleRepository>();
builder.Services.AddScoped<IQuestRepository, QuestRepository>();

// Services
builder.Services.AddScoped<IDiceService, DiceService>();
builder.Services.AddScoped<ILevelUpService, LevelUpService>();
builder.Services.AddScoped<IStoryService, StoryService>();
builder.Services.AddScoped<IPuzzleService, PuzzleService>();
builder.Services.AddScoped<IAdventureService, AdventureService>();

// Strategies (add as needed)
builder.Services.AddScoped<IBattleStrategy, DualAttackStrategy>();
builder.Services.AddScoped<IBattleStrategy, PlayerAttackStrategy>();
builder.Services.AddScoped<IBattleStrategy, EnemyAttackStrategy>();
builder.Services.AddScoped<IBattleStrategy, StalemateStrategy>();
builder.Services.AddScoped<IBattleStrategyProcessor, BattleStrategyProcessor>();

// MVC support
builder.Services.AddControllersWithViews();

// (other configuration...)

var app = builder.Build();

// Pipeline configuration (as before)
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Adventure/Error");
	app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Adventure}/{action=Index}/{id?}");

app.Run();
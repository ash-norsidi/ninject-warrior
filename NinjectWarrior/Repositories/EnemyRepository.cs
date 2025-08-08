using Newtonsoft.Json;
using NinjectWarrior.Models;

namespace NinjectWarrior.Repositories
{
	public class EnemyRepository : IEnemyRepository
	{
		private static readonly List<Enemy>? _enemies;

		static EnemyRepository()
		{
			// Use AppDomain.CurrentDomain.BaseDirectory to resolve the path
			var jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "enemies.json");
			var json = File.ReadAllText(jsonPath);
			_enemies = JsonConvert.DeserializeObject<List<Enemy>>(json);
		}

		public Enemy? GetEnemyByName(string name)
		{
			// Return a copy to prevent modifying the cached list
			var enemy = (_enemies ?? Enumerable.Empty<Enemy>()).FirstOrDefault(e => e.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
			if (enemy != null)
			{
				// Simple clone to avoid shared references in a web request
				return new Enemy
				{
					Name = enemy.Name,
					Health = enemy.Health,
					Level = enemy.Level,
					Strength = enemy.Strength,
					Defense = enemy.Defense,
					Evasion = enemy.Evasion,
					Luck = enemy.Luck,
					ExperienceAwarded = enemy.ExperienceAwarded
				};
			}
			return null;
		}
	}
}

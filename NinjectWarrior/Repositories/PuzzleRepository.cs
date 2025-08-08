using Newtonsoft.Json;
using NinjectWarrior.Models;

namespace NinjectWarrior.Repositories
{
	public class PuzzleRepository : IPuzzleRepository
	{
		private readonly Dictionary<string, Puzzle> _puzzles;
		private readonly IWebHostEnvironment _hostingEnvironment;

		public PuzzleRepository(IWebHostEnvironment hostEnvironment)
		{
			_hostingEnvironment = hostEnvironment;
			var puzzlesPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Data", "puzzle.json");
			var puzzlesJson = File.ReadAllText(puzzlesPath);
			var puzzles = JsonConvert.DeserializeObject<List<Puzzle>>(puzzlesJson) ?? [];
			_puzzles = puzzles.ToDictionary(p => p.Id, p => p);
		}

		public Puzzle GetPuzzle(string id)
		{
			return _puzzles.TryGetValue(id, out var puzzle) ? puzzle : 
				throw new KeyNotFoundException($"Puzzle with id '{id}' not found.");
		}
	}
}

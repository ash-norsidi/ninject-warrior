using NinjectWarrior.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace NinjectWarrior.Repositories
{
    public class PuzzleRepository : IPuzzleRepository
    {
        private readonly Dictionary<string, Puzzle> _puzzles;

        public PuzzleRepository()
        {
            var puzzlesPath = HostingEnvironment.MapPath("~/App_Data/puzzle.json");
            var puzzlesJson = File.ReadAllText(puzzlesPath);
            var puzzles = JsonConvert.DeserializeObject<List<Puzzle>>(puzzlesJson);
            _puzzles = puzzles.ToDictionary(p => p.Id, p => p);
        }

        public Puzzle GetPuzzle(string id)
        {
            return _puzzles.TryGetValue(id, out var puzzle) ? puzzle : null;
        }
    }
}

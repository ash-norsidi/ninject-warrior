using NinjectWarrior.Repositories;
using System;

namespace NinjectWarrior.Services
{
    public class PuzzleService : IPuzzleService
    {
        private readonly IPuzzleRepository _puzzleRepository;

        public PuzzleService(IPuzzleRepository puzzleRepository)
        {
            _puzzleRepository = puzzleRepository;
        }

        public bool CheckSolution(string puzzleId, string solution)
        {
            var puzzle = _puzzleRepository.GetPuzzle(puzzleId);
            if (puzzle == null)
            {
                return false;
            }
            return string.Equals(puzzle.Solution, solution, StringComparison.OrdinalIgnoreCase);
        }
    }
}

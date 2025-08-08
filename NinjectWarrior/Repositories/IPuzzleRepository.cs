using NinjectWarrior.Models;

namespace NinjectWarrior.Repositories
{
    public interface IPuzzleRepository
    {
        Puzzle GetPuzzle(string id);
    }
}

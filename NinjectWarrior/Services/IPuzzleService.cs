namespace NinjectWarrior.Services
{
    public interface IPuzzleService
    {
        bool CheckSolution(string puzzleId, string solution);
    }
}

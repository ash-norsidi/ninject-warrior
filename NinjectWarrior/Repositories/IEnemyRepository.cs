using NinjectWarrior.Models;

namespace NinjectWarrior.Repositories
{
    public interface IEnemyRepository
    {
        Enemy GetEnemyByName(string name);
    }
}

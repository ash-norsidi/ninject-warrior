using NinjectWarrior.Models;

namespace NinjectWarrior.Services
{
    public interface ILevelUpService
    {
        void CheckAndApplyLevelUp(Player player);
    }
}

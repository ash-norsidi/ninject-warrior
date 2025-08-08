using NinjectWarrior.Models;

namespace NinjectWarrior.Services.Strategies
{
    public interface IBattleStrategy
    {
        bool CanHandle(bool playerAttacks, bool enemyAttacks);
        BattleRoundResult Execute(Player player, int playerRoll, Enemy enemy, int enemyRoll);
    }
}

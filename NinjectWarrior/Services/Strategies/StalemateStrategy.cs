using NinjectWarrior.Models;

namespace NinjectWarrior.Services.Strategies
{
    public class StalemateStrategy : IBattleStrategy
    {
        public bool CanHandle(bool playerAttacks, bool enemyAttacks)
        {
            return !playerAttacks && !enemyAttacks;
        }

        public BattleRoundResult Execute(Player player, int playerRoll, Enemy enemy, int enemyRoll)
        {
            string resultMsg = $"{player.Name} and {enemy.Name} both defend. It's a stalemate!";
            return new BattleRoundResult(resultMsg, 0);
        }
    }
}

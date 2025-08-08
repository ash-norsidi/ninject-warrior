using NinjectWarrior.Models;
using NinjectWarrior.Services.Strategies;
using System.Collections.Generic;
using System.Linq;

namespace NinjectWarrior.Services
{
    public class BattleStrategyProcessor : IBattleStrategyProcessor
    {
        private readonly IEnumerable<IBattleStrategy> _strategies;

        public BattleStrategyProcessor(IEnumerable<IBattleStrategy> strategies)
        {
            _strategies = strategies;
        }

        public BattleRoundResult Resolve(Player player, int playerRoll, Enemy enemy, int enemyRoll)
        {
            bool playerAttacks = playerRoll % 2 != 0;
            bool enemyAttacks = enemyRoll % 2 != 0;

            var strategy = _strategies.First(s => s.CanHandle(playerAttacks, enemyAttacks));
            return strategy.Execute(player, playerRoll, enemy, enemyRoll);
        }
    }
}

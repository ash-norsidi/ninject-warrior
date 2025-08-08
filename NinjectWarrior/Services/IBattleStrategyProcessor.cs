using NinjectWarrior.Models;
using System.Collections.Generic;

namespace NinjectWarrior.Services
{
    public interface IBattleStrategyProcessor
    {
        BattleRoundResult Resolve(Player player, int playerRoll, Enemy enemy, int enemyRoll);
    }
}

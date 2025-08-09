using NinjectWarrior.Models;

namespace NinjectWarrior.Services.Strategies
{
    public class PlayerAttackStrategy : IBattleStrategy
    {
        public bool CanHandle(bool playerAttacks, bool enemyAttacks)
        {
            return playerAttacks && !enemyAttacks;
        }

        public BattleRoundResult Execute(Player player, int playerRoll, Enemy enemy, int enemyRoll)
        {
            int damage = CalculateDamage(player, playerRoll, enemy, enemyRoll);
            enemy.TakeDamage(damage);
            string resultMsg = $"{player.Name} attacks and {enemy.Name} defends. {player.Name} deals {damage} damage.";
            return new BattleRoundResult(resultMsg, damage);
        }

        private static int CalculateDamage(Player attacker, int attackerRoll, Enemy defender, int defenderRoll)
        {
            int baseDamage = attacker.Strength + attacker.StrengthBonus + (int)attacker.Weapon;
            int totalDefense = defender.Defense + defenderRoll;
            int damage = baseDamage + attackerRoll - totalDefense;
            return Math.Max(0, damage);
        }
    }
}

using NinjectWarrior.Models;

namespace NinjectWarrior.Services.Strategies
{
    public class DualAttackStrategy : IBattleStrategy
    {
        public bool CanHandle(bool playerAttacks, bool enemyAttacks)
        {
            return playerAttacks && enemyAttacks;
        }

        public BattleRoundResult Execute(Player player, int playerRoll, Enemy enemy, int enemyRoll)
        {
            int damage = CalculateDamage(player, playerRoll, enemy, enemyRoll);
            string resultMsg;

            if (damage > 0)
            {
                enemy.TakeDamage(damage);
                resultMsg = $"{player.Name} and {enemy.Name} both attack! {player.Name} overpowers {enemy.Name} and deals {damage} damage.";
            }
            else
            {
                player.TakeDamage(-damage);
                resultMsg = $"{player.Name} and {enemy.Name} both attack! {enemy.Name} overpowers {player.Name} and deals {-damage} damage.";
            }

            return new BattleRoundResult(resultMsg, damage);
        }

        private static int CalculateDamage(Player attacker, int attackerRoll, Enemy defender, int defenderRoll)
        {
            int baseDamage = attacker.Strength + attacker.StrengthBonus + (int)attacker.Weapon;
            int totalDefense = defender.Defense + defenderRoll;
            int damage = baseDamage + attackerRoll - totalDefense;
            return damage;
        }
    }
}

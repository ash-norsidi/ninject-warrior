using NinjectWarrior.Models;
using System;

namespace NinjectWarrior.Services.Strategies
{
    public class EnemyAttackStrategy : IBattleStrategy
    {
        public bool CanHandle(bool playerAttacks, bool enemyAttacks)
        {
            return !playerAttacks && enemyAttacks;
        }

        public BattleRoundResult Execute(Player player, int playerRoll, Enemy enemy, int enemyRoll)
        {
            int damage = CalculateDamage(enemy, enemyRoll, player, playerRoll);
            player.TakeDamage(damage);
            string resultMsg = $"{player.Name} defends and {enemy.Name} attacks. {enemy.Name} deals {damage} damage.";
            return new BattleRoundResult(resultMsg, damage);
        }

        private int CalculateDamage(Enemy attacker, int attackerRoll, Player defender, int defenderRoll)
        {
            int baseDamage = attacker.Strength;
            int totalDefense = defender.Defense + defender.DefenseBonus + defenderRoll;
            int damage = baseDamage + attackerRoll - totalDefense;
            return Math.Max(0, damage);
        }
    }
}

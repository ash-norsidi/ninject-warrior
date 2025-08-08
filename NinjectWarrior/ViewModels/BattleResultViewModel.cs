using NinjectWarrior.Models;

namespace NinjectWarrior.ViewModels
{
    public class BattleResultViewModel
    {
        public Player? Player { get; set; }
        public Enemy? Enemy { get; set; }
        public string? ResultMessage { get; set; }
        public int PlayerRoll { get; set; }
        public int EnemyRoll { get; set; }
        public int DamageDealt { get; set; }
    }
}

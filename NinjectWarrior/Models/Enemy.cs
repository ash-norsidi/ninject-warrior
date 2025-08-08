using System.ComponentModel.DataAnnotations;

namespace NinjectWarrior.Models
{
    public class Enemy : ICombatant
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Health { get; set; }
        public int Level { get; set; }
        public int Strength { get; set; }
        public int Defense { get; set; }
        public int Evasion { get; set; }
        public int Luck { get; set; }
        public int ExperienceAwarded { get; set; }

        public void TakeDamage(int amount)
        {
            this.Health -= amount;
            if (this.Health < 0)
            {
                this.Health = 0;
            }
        }
    }
}

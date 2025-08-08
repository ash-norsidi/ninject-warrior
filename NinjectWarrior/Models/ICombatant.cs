namespace NinjectWarrior.Models
{
    public interface ICombatant
    {
        string Name { get; }
        int Health { get; set; }
        int Strength { get; }
        int Defense { get; }
        int Evasion { get; }
        int Luck { get; }
        void TakeDamage(int amount);
    }
}

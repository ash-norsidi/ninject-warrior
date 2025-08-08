namespace NinjectWarrior.Models
{
    public class PlayerFaction
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public Faction Faction { get; set; }
        public int Reputation { get; set; }
    }
}

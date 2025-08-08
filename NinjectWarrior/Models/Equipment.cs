namespace NinjectWarrior.Models
{
    public class Equipment : Item, IEquipment
    {
        public EquipmentSlot Slot { get; set; }
        public int StrengthBonus { get; set; }
        public int DefenseBonus { get; set; }
        public int EvasionBonus { get; set; }
        public int LuckBonus { get; set; }
        public int HealthBonus { get; set; }
        public bool IsAcquired { get; set; }
    }
}

namespace NinjectWarrior.Models
{
    public interface IEquipment : IItem
    {
        EquipmentSlot Slot { get; set; }
        int StrengthBonus { get; set; }
        int DefenseBonus { get; set; }
        int EvasionBonus { get; set; }
        int LuckBonus { get; set; }
        int HealthBonus { get; set; }
    }
}

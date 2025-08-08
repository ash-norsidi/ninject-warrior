using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NinjectWarrior.Models
{
    public class Player : ICombatant
    {
        public Player()
        {
            CompletedQuestIds = new List<string>();
            PlayerFactions = new List<PlayerFaction>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int Health { get; set; }
        public int Strength { get; set; }
        public int Defense { get; set; }
        public int Evasion { get; set; }
        public int Luck { get; set; }
        public int Experience { get; set; }
        public int ExperienceToNextLevel { get; set; }
        public int Gold { get; set; }

        public GameState CurrentGameState { get; set; }
        public string CurrentMainQuestId { get; set; }
        public string ActiveQuestId { get; set; }
        public string ActivePuzzleId { get; set; }
        public virtual ICollection<string> CompletedQuestIds { get; set; }
        public virtual ICollection<PlayerFaction> PlayerFactions { get; set; }

        public IInventory Inventory { get; set; }
        public IDictionary<EquipmentSlot, IEquipment> EquippedItems { get; set; }

        // Added for demo purposes
        public WeaponType Weapon { get; set; }

        public int StrengthBonus { get; private set; }
        public int DefenseBonus { get; private set; }
        public int EvasionBonus { get; private set; }
        public int LuckBonus { get; private set; }
        public int HealthBonus { get; private set; }

        public void CalculateBonuses()
        {
            if (EquippedItems == null) return;

            StrengthBonus = 0;
            DefenseBonus = 0;
            EvasionBonus = 0;
            LuckBonus = 0;
            HealthBonus = 0;

            foreach (var item in EquippedItems.Values)
            {
                StrengthBonus += item.StrengthBonus;
                DefenseBonus += item.DefenseBonus;
                EvasionBonus += item.EvasionBonus;
                LuckBonus += item.LuckBonus;
                HealthBonus += item.HealthBonus;
            }
        }

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

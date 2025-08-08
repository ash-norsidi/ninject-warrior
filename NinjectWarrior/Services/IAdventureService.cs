using NinjectWarrior.Models;
using NinjectWarrior.ViewModels;
using System.Collections.Generic;

namespace NinjectWarrior.Services
{
    public interface IAdventureService
    {
        Player GetCurrentPlayer();
        BattleResultViewModel PerformBattle(string enemyName, WeaponType weaponType);
        void UpdatePlayerEquipment(int playerId, IDictionary<EquipmentSlot, int> equippedItems);
        void ProcessQuestChoice(string questId, string choiceId);
        bool SolvePuzzle(string solution);
    }
}

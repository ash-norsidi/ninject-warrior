using NinjectWarrior.Models;
using System.Collections.Generic;

namespace NinjectWarrior.Repositories
{
    public interface IPlayerRepository
    {
        Player GetPlayerById(int id);
        void UpdatePlayerEquipment(int playerId, IDictionary<EquipmentSlot, int> equippedItems);
    }
}

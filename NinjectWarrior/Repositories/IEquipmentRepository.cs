using NinjectWarrior.Models;
using System.Collections.Generic;

namespace NinjectWarrior.Repositories
{
    public interface IEquipmentRepository
    {
        IEnumerable<Equipment> GetAllEquipment();
        Equipment GetEquipmentById(int id);
    }
}

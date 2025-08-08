using System.Collections.Generic;

namespace NinjectWarrior.Models
{
    public interface IInventory
    {
        IList<IItem> Items { get; }
        void AddItem(IItem item);
        void RemoveItem(IItem item);
    }
}

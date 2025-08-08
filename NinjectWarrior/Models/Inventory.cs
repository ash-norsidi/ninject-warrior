using System.Collections.Generic;
using System.Linq;

namespace NinjectWarrior.Models
{
    public class Inventory : IInventory
    {
        private readonly IList<IItem> _items = new List<IItem>();

        public IList<IItem> Items
        {
            get { return _items; }
        }

        public void AddItem(IItem item)
        {
            _items.Add(item);
        }

        public void RemoveItem(IItem item)
        {
            _items.Remove(item);
        }
    }
}

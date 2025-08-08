using System.ComponentModel.DataAnnotations;

namespace NinjectWarrior.Models
{
    public class Item : IItem
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

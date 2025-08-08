using System.Collections.Generic;

namespace NinjectWarrior.Models
{
    public class Puzzle
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Prompt { get; set; }
        public string Solution { get; set; }
        public List<string> Hints { get; set; }
    }
}

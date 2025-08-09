using System.Collections.Generic;

namespace NinjectWarrior.Models
{
    public class Quest
    {
        public Quest()
        {
            Id = string.Empty;
            Title = string.Empty;
            Script = string.Empty;
            Choices = new Dictionary<string, string>();
            Rewards = new QuestReward();
            Battle = string.Empty;
            PuzzleId = string.Empty;
            Extra = new object();
            Enemy = string.Empty;
            FactionImpact = new FactionImpact();
        }
        public QuestType QuestType { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public string Script { get; set; }
        public Dictionary<string, string> Choices { get; set; }
        public QuestReward Rewards { get; set; }
        public string Battle { get; set; }
        public string PuzzleId { get; set; }
        public object Extra { get; set; }
        public string Enemy { get; set; }
        public FactionImpact FactionImpact { get; set; }
    }

    public class QuestReward
    {
        public QuestReward()
        {
            Items = new List<string>();
        }
        public int Experience { get; set; }
        public int Gold { get; set; }
        public List<string> Items { get; set; }
    }

    public class FactionImpact
    {
        public FactionImpact()
        {
            CityRogues = 0;
            EmberforgedGuild = 0;
        }
        public int CityRogues { get; set; }
        public int EmberforgedGuild { get; set; }
    }
}

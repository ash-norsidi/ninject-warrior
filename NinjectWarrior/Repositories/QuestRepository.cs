using NinjectWarrior.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace NinjectWarrior.Repositories
{
    public class QuestRepository : IQuestRepository
    {
        private readonly Dictionary<string, Quest> _quests;

        public QuestRepository()
        {
            var mainQuestsPath = HostingEnvironment.MapPath("~/App_Data/main_quest.json");
            var subQuestsPath = HostingEnvironment.MapPath("~/App_Data/sub_quest.json");

            var mainQuestsJson = File.ReadAllText(mainQuestsPath);
            var subQuestsJson = File.ReadAllText(subQuestsPath);

            var mainQuests = JsonConvert.DeserializeObject<List<Quest>>(mainQuestsJson);
            var subQuests = JsonConvert.DeserializeObject<List<Quest>>(subQuestsJson);

            foreach (var quest in mainQuests)
            {
                quest.QuestType = QuestType.Main;
            }

            foreach (var quest in subQuests)
            {
                quest.QuestType = QuestType.Sub;
            }

            _quests = mainQuests.Concat(subQuests).ToDictionary(q => q.Id, q => q);
        }

        public Quest GetQuest(string id)
        {
            return _quests.TryGetValue(id, out var quest) ? quest : null;
        }

        public IEnumerable<Quest> GetQuests(QuestType type)
        {
            return _quests.Values.Where(q => q.QuestType == type);
        }
    }
}

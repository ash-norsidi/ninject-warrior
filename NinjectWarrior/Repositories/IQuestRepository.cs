using NinjectWarrior.Models;
using System.Collections.Generic;

namespace NinjectWarrior.Repositories
{
    public interface IQuestRepository
    {
        Quest GetQuest(string id);
        IEnumerable<Quest> GetQuests(QuestType type);
    }
}

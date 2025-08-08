using NinjectWarrior.Models;
using System.Collections.Generic;

namespace NinjectWarrior.Services
{
    public interface IStoryService
    {
        Quest? GetCurrentMainQuest(Player player);
        IEnumerable<Quest> GetAvailableSubQuests(Player player);
        void ProcessQuestChoice(Player player, string questId, string choiceId);
    }
}

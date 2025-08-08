using NinjectWarrior.Models;
using NinjectWarrior.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace NinjectWarrior.Services
{
    public class StoryService : IStoryService
    {
        private readonly IQuestRepository _questRepository;

        public StoryService(IQuestRepository questRepository)
        {
            _questRepository = questRepository;
        }

        public Quest GetCurrentMainQuest(Player player)
        {
            if (string.IsNullOrEmpty(player.CurrentMainQuestId))
            {
                var firstQuest = _questRepository.GetQuests(QuestType.Main).FirstOrDefault();
                if (firstQuest != null)
                {
                    player.CurrentMainQuestId = firstQuest.Id;
                    InitiateQuestSequence(player, firstQuest);
                    return firstQuest;
                }
                return null;
            }
            return _questRepository.GetQuest(player.CurrentMainQuestId);
        }

        public IEnumerable<Quest> GetAvailableSubQuests(Player player)
        {
            var allSubQuests = _questRepository.GetQuests(QuestType.Sub);
            var completedQuestIds = new HashSet<string>(player.CompletedQuestIds);
            return allSubQuests.Where(q => !completedQuestIds.Contains(q.Id));
        }

        public void ProcessQuestChoice(Player player, string questId, string choiceId)
        {
            var quest = _questRepository.GetQuest(questId);
            if (quest == null) return;

            // Apply rewards for the completed quest
            if (quest.Rewards != null)
            {
                player.Experience += quest.Rewards.Experience;
                player.Gold += quest.Rewards.Gold;
                if (quest.Rewards.Items != null)
                {
                    // TODO: Add items to inventory
                }
            }

            player.CompletedQuestIds.Add(questId);
            player.ActiveQuestId = null;

            if (quest.QuestType == QuestType.Main)
            {
                // Advance to the next main quest
                var currentQuestIdNum = int.Parse(quest.Id);
                var nextQuestId = (currentQuestIdNum + 1).ToString();
                var nextQuest = _questRepository.GetQuest(nextQuestId);

                if (nextQuest != null && nextQuest.QuestType == QuestType.Main)
                {
                    player.CurrentMainQuestId = nextQuest.Id;
                    InitiateQuestSequence(player, nextQuest);
                }
                else
                {
                    player.CurrentMainQuestId = null; // Story finished
                }
            }
        }

        private void InitiateQuestSequence(Player player, Quest quest)
        {
            player.ActiveQuestId = quest.Id;
            if (!string.IsNullOrEmpty(quest.PuzzleId))
            {
                player.CurrentGameState = GameState.Puzzle;
                player.ActivePuzzleId = quest.PuzzleId;
            }
            else if (!string.IsNullOrEmpty(quest.Battle) || !string.IsNullOrEmpty(quest.Enemy))
            {
                player.CurrentGameState = GameState.Battle;
            }
            else
            {
                player.CurrentGameState = GameState.Adventure;
            }
        }
    }
}

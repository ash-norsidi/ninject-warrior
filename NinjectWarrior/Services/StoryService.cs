using NinjectWarrior.Models;
using NinjectWarrior.Repositories;

namespace NinjectWarrior.Services
{
    public class StoryService(IQuestRepository questRepository) : IStoryService
    {
        private readonly IQuestRepository _questRepository = questRepository;

		public Quest? GetCurrentMainQuest(Player player)
		{
			if (string.IsNullOrEmpty(player.CurrentMainQuestId))
			{
				// This is a new game, start the first quest.
				var firstQuest = _questRepository.GetQuests(QuestType.Main).FirstOrDefault();
				if (firstQuest != null)
				{
					player.CurrentMainQuestId = firstQuest.Id;
					// A new quest is now current, so initiate it.
					InitiateQuestSequence(player, firstQuest);
					return firstQuest;
				}
				return null;
			}

			var currentQuest = _questRepository.GetQuest(player.CurrentMainQuestId);

			// If the player's active quest is not the current main quest,
			// it means we have advanced to a new quest and it needs to be initiated.
			if (currentQuest != null && player.ActiveQuestId != player.CurrentMainQuestId)
			{
				// Only initiate if it's not already completed.
				if (!player.CompletedQuestIds.Contains(currentQuest.Id))
				{
					InitiateQuestSequence(player, currentQuest);
				}
			}

			return currentQuest;
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

            if (quest.FactionImpact != null)
            {
                UpdateFactionReputation(player, Faction.EmberforgedGuild, quest.FactionImpact.EmberforgedGuild);
                UpdateFactionReputation(player, Faction.CityRogues, quest.FactionImpact.CityRogues);
            }

            player.CompletedQuestIds.Add(questId);
            player.ActiveQuestId = null;

            if (quest.QuestType == QuestType.Main)
            {
                var subQuest = GetAvailableSubQuests(player).FirstOrDefault();
                if (subQuest != null)
                {
                    InitiateQuestSequence(player, subQuest);
                }
                else
                {
                    AdvanceToNextMainQuest(player, quest.Id);
                }
            }
            else // Sub-quest or other quest type
            {
                AdvanceToNextMainQuest(player, player.CurrentMainQuestId);
            }
        }

        private void AdvanceToNextMainQuest(Player player, string? currentMainQuestId)
        {
            if (string.IsNullOrEmpty(currentMainQuestId))
            {
                player.CurrentMainQuestId = null;
                return;
            }

            if (!int.TryParse(currentMainQuestId, out var currentQuestIdNum))
            {
                player.CurrentMainQuestId = null;
                return;
            }

            var nextQuestId = (currentQuestIdNum + 1).ToString();
            var nextQuest = _questRepository.GetQuest(nextQuestId);

            if (nextQuest != null && nextQuest.QuestType == QuestType.Main)
            {
                // Set the ID for the next quest.
                // The initiation of the quest (puzzle, battle, etc.) will happen
                // when GetCurrentMainQuest is called on the next request.
                player.CurrentMainQuestId = nextQuest.Id;
            }
            else
            {
                player.CurrentMainQuestId = null; // Story finished
            }
        }

        private static void InitiateQuestSequence(Player player, Quest quest)
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

        private void UpdateFactionReputation(Player player, Faction faction, int reputationChange)
        {
            if (reputationChange == 0) return;

            var playerFaction = player.PlayerFactions.FirstOrDefault(f => f.Faction == faction);
            if (playerFaction != null)
            {
                playerFaction.Reputation += reputationChange;
            }
            else
            {
                player.PlayerFactions.Add(new PlayerFaction
                {
                    PlayerId = player.Id,
                    Faction = faction,
                    Reputation = reputationChange
                });
            }
        }
    }
}

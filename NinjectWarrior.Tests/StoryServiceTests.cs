using System.Collections.Generic;
using Moq;
using NinjectWarrior.Models;
using NinjectWarrior.Repositories;
using NinjectWarrior.Services;
using Xunit;

namespace NinjectWarrior.Tests
{
    public class StoryServiceTests
    {
        [Fact]
        public void ProcessQuestChoice_AppliesFactionReputation_WhenQuestHasFactionImpact()
        {
            // Arrange
            var player = new Player
            {
                Id = 1,
                PlayerFactions = new List<PlayerFaction>()
            };
            var questId = "test_quest";
            var choiceId = "1";

            var quest = new Quest
            {
                Id = questId,
                FactionImpact = new FactionImpact
                {
                    EmberforgedGuild = 10,
                    CityRogues = -5
                },
                Rewards = new QuestReward() // Add empty rewards to avoid null reference
            };

            var mockQuestRepository = new Mock<IQuestRepository>();
            mockQuestRepository.Setup(repo => repo.GetQuest(questId)).Returns(quest);

            var storyService = new StoryService(mockQuestRepository.Object);

            // Act
            storyService.ProcessQuestChoice(player, questId, choiceId);

            // Assert
            var guildFaction = player.PlayerFactions.FirstOrDefault(f => f.Faction == Faction.EmberforgedGuild);
            Assert.NotNull(guildFaction);
            Assert.Equal(10, guildFaction.Reputation);

            var rogueFaction = player.PlayerFactions.FirstOrDefault(f => f.Faction == Faction.CityRogues);
            Assert.NotNull(rogueFaction);
            Assert.Equal(-5, rogueFaction.Reputation);
        }
    }
}

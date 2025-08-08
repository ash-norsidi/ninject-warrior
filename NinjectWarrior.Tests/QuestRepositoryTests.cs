using NinjectWarrior.Models;
using NinjectWarrior.Repositories;
using Xunit;

namespace NinjectWarrior.Tests
{
    public class QuestRepositoryTests
    {
        [Fact]
        public void GetQuests_LoadsMainQuests_Successfully()
        {
            // Arrange
            var questRepository = new QuestRepository();

            // Act
            var mainQuests = questRepository.GetQuests(QuestType.Main);

            // Assert
            Assert.NotNull(mainQuests);
            Assert.NotEmpty(mainQuests);
            Assert.Equal("1", mainQuests.First().Id);
        }

        [Fact]
        public void GetQuest_ReturnsCorrectQuest_WhenIdExists()
        {
            // Arrange
            var questRepository = new QuestRepository();

            // Act
            var quest = questRepository.GetQuest("2");

            // Assert
            Assert.NotNull(quest);
            Assert.Equal("2", quest.Id);
            Assert.Equal("Whispers Beneath Vaeloria", quest.Title);
        }
    }
}

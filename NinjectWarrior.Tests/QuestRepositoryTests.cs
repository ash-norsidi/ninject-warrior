using NinjectWarrior.Models;
using NinjectWarrior.Repositories;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Hosting;

namespace NinjectWarrior.Tests
{
    public class QuestRepositoryTests
    {
        [Fact]
        public void GetQuests_LoadsMainQuests_Successfully()
        {
            // Arrange
            var mockEnv = new Mock<IWebHostEnvironment>();
            mockEnv.Setup(e => e.ContentRootPath).Returns("TestData");
            var questRepository = new QuestRepository(mockEnv.Object);

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
            var mockEnv = new Mock<IWebHostEnvironment>();
            mockEnv.Setup(e => e.ContentRootPath).Returns("TestData");
            var questRepository = new QuestRepository(mockEnv.Object);

            // Act
            var quest = questRepository.GetQuest("2");

            // Assert
            Assert.NotNull(quest);
            Assert.Equal("2", quest.Id);
            Assert.Equal("Whispers Beneath Vaeloria", quest.Title);
        }
    }
}

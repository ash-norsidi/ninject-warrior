using Moq;
using NinjectWarrior.Models;
using NinjectWarrior.Repositories;
using NinjectWarrior.Services;
using Xunit;

namespace NinjectWarrior.Tests
{
    public class AdventureServiceTests
    {
        private readonly Mock<IPlayerRepository> _mockPlayerRepository;
        private readonly Mock<IEnemyRepository> _mockEnemyRepository;
        private readonly Mock<IQuestRepository> _mockQuestRepository;
        private readonly Mock<IPuzzleService> _mockPuzzleService;
        private readonly Mock<IBattleStrategyProcessor> _mockBattleProcessor;
        private readonly Mock<ILevelUpService> _mockLevelUpService;
        private readonly Mock<IStoryService> _mockStoryService;
        private readonly Mock<IDiceService> _mockDiceService;
        private readonly AdventureService _adventureService;

        public AdventureServiceTests()
        {
            _mockPlayerRepository = new Mock<IPlayerRepository>();
            _mockEnemyRepository = new Mock<IEnemyRepository>();
            _mockQuestRepository = new Mock<IQuestRepository>();
            _mockPuzzleService = new Mock<IPuzzleService>();
            _mockBattleProcessor = new Mock<IBattleStrategyProcessor>();
            _mockLevelUpService = new Mock<ILevelUpService>();
            _mockStoryService = new Mock<IStoryService>();
            _mockDiceService = new Mock<IDiceService>();

            _adventureService = new AdventureService(
                _mockPlayerRepository.Object,
                _mockEnemyRepository.Object,
                _mockQuestRepository.Object,
                _mockPuzzleService.Object,
                _mockBattleProcessor.Object,
                _mockLevelUpService.Object,
                _mockStoryService.Object,
                _mockDiceService.Object
            );
        }

        [Fact]
        public void PerformBattle_PlayerWins_GrantsExperienceAndSetsState()
        {
            // Arrange
            var player = new Player { Id = 1, Health = 100, Name = "Hero" };
            var enemy = new Enemy { Name = "Goblin", Health = 0, ExperienceAwarded = 10 };
            var battleOutcome = new BattleRoundResult { ResultMessage = "Win", DamageDealt = 10 };

            _mockPlayerRepository.Setup(r => r.GetPlayerById(1)).Returns(player);
            _mockEnemyRepository.Setup(r => r.GetEnemyByName("Goblin")).Returns(enemy);
            _mockDiceService.Setup(s => s.RollD20()).Returns(15);
            _mockBattleProcessor.Setup(p => p.Resolve(It.IsAny<Player>(), It.IsAny<int>(), It.IsAny<Enemy>(), It.IsAny<int>()))
                .Returns(battleOutcome);

            // Act
            var result = _adventureService.PerformBattle("Goblin", WeaponType.Sword);

            // Assert
            Assert.Contains("Hero defeated Goblin", result.ResultMessage);
            _mockLevelUpService.Verify(s => s.CheckAndApplyLevelUp(player), Times.Once);
            Assert.Equal(GameState.Adventure, player.CurrentGameState);
            Assert.Equal(10, player.Experience);
        }

        [Fact]
        public void SolvePuzzle_CorrectSolution_ReturnsTrueAndUpdatesState()
        {
            // Arrange
            var player = new Player { Id = 1, ActivePuzzleId = "puzzle1" };
            _mockPlayerRepository.Setup(r => r.GetPlayerById(1)).Returns(player);
            _mockPuzzleService.Setup(s => s.CheckSolution("puzzle1", "solution")).Returns(true);

            // Act
            var result = _adventureService.SolvePuzzle("solution");

            // Assert
            Assert.True(result);
            Assert.Null(player.ActivePuzzleId);
            Assert.Equal(GameState.Adventure, player.CurrentGameState);
        }
    }
}

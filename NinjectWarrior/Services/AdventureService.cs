using NinjectWarrior.Models;
using NinjectWarrior.Repositories;
using NinjectWarrior.ViewModels;
using System.Collections.Generic;
using NinjectWarrior.ApiClients;

namespace NinjectWarrior.Services
{
    public class AdventureService : IAdventureService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IEnemyRepository _enemyRepository;
        private readonly IQuestRepository _questRepository;
        private readonly IPuzzleService _puzzleService;
        private readonly IBattleStrategyProcessor _battleStrategyProcessor;
        private readonly ILevelUpService _levelUpService;
        private readonly IStoryService _storyService;
        private readonly IDiceService _diceService;
        private readonly IGameLogger _gameLogger;

        public AdventureService(
            IPlayerRepository playerRepository,
            IEnemyRepository enemyRepository,
            IQuestRepository questRepository,
            IPuzzleService puzzleService,
            IBattleStrategyProcessor battleStrategyProcessor,
            ILevelUpService levelUpService,
            IStoryService storyService,
            IDiceService diceService,
            IGameLogger gameLogger)
        {
            _playerRepository = playerRepository;
            _enemyRepository = enemyRepository;
            _questRepository = questRepository;
            _puzzleService = puzzleService;
            _battleStrategyProcessor = battleStrategyProcessor;
            _levelUpService = levelUpService;
            _storyService = storyService;
            _diceService = diceService;
            _gameLogger = gameLogger;
        }

        public Player GetCurrentPlayer()
        {
            return _playerRepository.GetPlayerById(1);
        }

        public void ProcessQuestChoice(string questId, string choiceId)
        {
            var player = GetCurrentPlayer();
            _storyService.ProcessQuestChoice(player, questId, choiceId);
        }

        public bool SolvePuzzle(string solution)
        {
            var player = GetCurrentPlayer();
            if (string.IsNullOrEmpty(player.ActivePuzzleId)) return false;

            bool isCorrect = _puzzleService.CheckSolution(player.ActivePuzzleId, solution);

            if (isCorrect)
            {
                var quest = _questRepository.GetQuest(player.ActiveQuestId);
                player.ActivePuzzleId = null;

                if (!string.IsNullOrEmpty(quest?.Battle) || !string.IsNullOrEmpty(quest?.Enemy))
                {
                    player.CurrentGameState = GameState.Battle;
                }
                else
                {
                    player.CurrentGameState = GameState.Adventure;
                }
            }

            return isCorrect;
        }

        public BattleResultViewModel PerformBattle(string enemyName, WeaponType weaponType)
        {
            var player = GetCurrentPlayer();
            player.Weapon = weaponType;

            var enemy = _enemyRepository.GetEnemyByName(enemyName);
            if (enemy == null)
            {
                enemy = new Enemy { Name = "Default Goblin", Health = 20, ExperienceAwarded = 5 };
            }

            var (playerRoll, enemyRoll) = RollForCombatants(player, enemy);
            var battleOutcome = _battleStrategyProcessor.Resolve(player, playerRoll, enemy, enemyRoll);
            string finalMessage = battleOutcome.ResultMessage;

            if (enemy.Health <= 0)
            {
                finalMessage += $" {player.Name} defeated {enemy.Name}!";
                GrantExperience(player, enemy);

                // If battle was part of a quest, return to adventure to see choices
                if (!string.IsNullOrEmpty(player.ActiveQuestId))
                {
                    player.CurrentGameState = GameState.Adventure;
                }
                else
                {
                    // Handle non-quest battle completion if necessary
                    player.CurrentGameState = GameState.Adventure;
                }
            }
            else if (player.Health <= 0)
            {
                finalMessage += $" {enemy.Name} defeated {player.Name}!";
                player.CurrentGameState = GameState.Adventure;
            }

            _gameLogger.LogEvent(finalMessage);

            return new BattleResultViewModel
            {
                Player = player,
                Enemy = enemy,
                ResultMessage = finalMessage,
                PlayerRoll = playerRoll,
                EnemyRoll = enemyRoll,
                DamageDealt = battleOutcome.DamageDealt
            };
        }

        private void GrantExperience(Player player, Enemy enemy)
        {
            player.Experience += enemy.ExperienceAwarded;
            _levelUpService.CheckAndApplyLevelUp(player);
        }

        private (int playerRoll, int enemyRoll) RollForCombatants(ICombatant player, ICombatant enemy)
        {
            int playerRoll = _diceService.RollD20() + (player.Luck / 2);
            int enemyRoll = _diceService.RollD20() + (enemy.Luck / 2);
            return (playerRoll, enemyRoll);
        }

        public void UpdatePlayerEquipment(int playerId, IDictionary<EquipmentSlot, int> equippedItems)
        {
            _playerRepository.UpdatePlayerEquipment(playerId, equippedItems);
        }
    }
}

using NinjectWarrior.Models;
using NinjectWarrior.Repositories;
using NinjectWarrior.ViewModels;

namespace NinjectWarrior.Services
{
    public class AdventureService(
		IPlayerRepository playerRepository,
		IEnemyRepository enemyRepository,
		IQuestRepository questRepository,
		IPuzzleService puzzleService,
		IBattleStrategyProcessor battleStrategyProcessor,
		ILevelUpService levelUpService,
		IStoryService storyService,
		IDiceService diceService) : IAdventureService
    {
        private readonly IPlayerRepository _playerRepository = playerRepository;
        private readonly IEnemyRepository _enemyRepository = enemyRepository;
        private readonly IQuestRepository _questRepository = questRepository;
        private readonly IPuzzleService _puzzleService = puzzleService;
        private readonly IBattleStrategyProcessor _battleStrategyProcessor = battleStrategyProcessor;
        private readonly ILevelUpService _levelUpService = levelUpService;
        private readonly IStoryService _storyService = storyService;
        private readonly IDiceService _diceService = diceService;

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
				Quest? quest = null;
				if (!string.IsNullOrEmpty(player.ActiveQuestId))
				{
					quest = _questRepository.GetQuest(player.ActiveQuestId);
				}
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

            if (player.CurrentEnemy == null || player.CurrentEnemy.Name != enemyName)
            {
                player.CurrentEnemy = _enemyRepository.GetEnemyByName(enemyName);
            }

            var enemy = player.CurrentEnemy;

            var (playerRoll, enemyRoll) = RollForCombatants(player, enemy);
            var battleOutcome = _battleStrategyProcessor.Resolve(player, playerRoll, enemy, enemyRoll);
            string finalMessage = battleOutcome.ResultMessage;

            if (enemy.Health <= 0)
            {
                finalMessage += $" {player.Name} defeated {enemy.Name}!";
                GrantExperience(player, enemy);
                player.CurrentEnemy = null; // Clear the enemy after battle

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
                player.CurrentEnemy = null; // Clear the enemy after battle
                player.CurrentGameState = GameState.Adventure;
            }

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

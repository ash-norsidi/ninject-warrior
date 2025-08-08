using NinjectWarrior.Models;

namespace NinjectWarrior.Services
{
    public class LevelUpService : ILevelUpService
    {
        public void CheckAndApplyLevelUp(Player player)
        {
            if (player.Experience >= player.ExperienceToNextLevel)
            {
                player.Level++;
                player.Experience -= player.ExperienceToNextLevel;

                // Increase stats
                player.Health += 10; // Simple stat increase, can be more complex
                player.Strength += 2;
                player.Defense += 1;

                // Set next level's experience requirement
                player.ExperienceToNextLevel = (int)(player.ExperienceToNextLevel * 1.5);
            }
        }
    }
}

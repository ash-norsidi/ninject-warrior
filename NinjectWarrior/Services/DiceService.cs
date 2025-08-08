using System;

namespace NinjectWarrior.Services
{
    public class DiceService : IDiceService
    {
        private static readonly Random _random = new Random();

        public int RollD20()
        {
            return _random.Next(1, 21);
        }
    }
}

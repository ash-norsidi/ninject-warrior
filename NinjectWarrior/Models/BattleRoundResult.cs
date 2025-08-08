namespace NinjectWarrior.Models
{
    public class BattleRoundResult
    {
        public string ResultMessage { get; }
        public int DamageDealt { get; }

        public BattleRoundResult(string resultMessage, int damageDealt)
        {
            ResultMessage = resultMessage;
            DamageDealt = damageDealt;
        }
    }
}

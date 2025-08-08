using System.Diagnostics;

namespace FuzzyNinjectWarrior.ApiClients
{
    public class ApiGameLogger : IGameLogger
    {
        public void LogEvent(string message)
        {
            // For demonstration: log to Debug output.
            Debug.WriteLine($"[GameLog] {message}");
            // Here, you could call an external API or service.
        }
    }
}

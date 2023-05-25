using Modules.BotSpawn.Data;

namespace Modules.BotSpawn.Models
{
    public interface IBotModel
    {
        void InitBots();
        void OnBotDestroyed(BotType botType);
        void OnBotSpawned(BotType botType);
    }
}
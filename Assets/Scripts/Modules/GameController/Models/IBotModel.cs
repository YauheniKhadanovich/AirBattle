using Modules.GameController.Data;

namespace Modules.GameController.Models
{
    public interface IBotModel
    {
        void InitBots();
        void OnBotDestroyed(BotType botType, bool byPlayer);
        void OnBotSpawned(BotType botType);
    }
}
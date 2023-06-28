using System;
using Modules.GameController.Data;

namespace Modules.GameController.Facade
{
    public interface IGameControllerFacade
    {
        event Action<BotInfo> SpawnBotRequested;
        event Action GameFailed;
        event Action GameStarted;

        void StartGame(bool needInitBots);
        void OnBotDestroyed(BotType botType);
        void OnBotSpawned(BotType botType);
        void OnGameFailed();
        void RequestSpawn(BotInfo botType);
    }
}
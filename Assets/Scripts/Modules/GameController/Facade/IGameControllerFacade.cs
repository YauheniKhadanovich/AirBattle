using System;
using Modules.GameController.Data;

namespace Modules.GameController.Facade
{
    public interface IGameControllerFacade
    {
        event Action<BotInfo> SpawnBotRequested;
        event Action GameFailed;
        event Action GameStarted;
        event Action<int> PointUpdated; 

        void StartGame(bool needInitBots);
        void OnBotDestroyed(BotType botType, bool byPlayer);
        void OnBotSpawned(BotType botType);
        void OnGameFailed();
        void RequestSpawn(BotInfo botType);
        void OnPointUpdated(int points);
    }
}
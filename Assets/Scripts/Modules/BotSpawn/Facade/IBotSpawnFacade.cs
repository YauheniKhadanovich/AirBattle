using System;
using Modules.BotSpawn.Data;

namespace Modules.BotSpawn.Facade
{
    public interface IBotSpawnFacade
    {
        event Action<BotInfo> SpawnBotRequested;
        event Action GameFailed;
        event Action GameStarted;

        void StartGame(bool needInitBots);
        void OnBotDestroyed(BotType botType);
        void OnBotSpawned(BotType botType);
        void OnGameFailed();
        void NeedSpawn(BotInfo botType);
    }
}
using System;
using Modules.BotSpawn.Data;
using Modules.BotSpawn.Models;
using Zenject;

namespace Modules.BotSpawn.Facade.Impl
{
    public class BotSpawnFacade : IBotSpawnFacade
    {
        public event Action<BotInfo> SpawnBotRequested = delegate { };
        public event Action GameStarted = delegate { };
        public event Action GameFailed = delegate { };

        [Inject]
        private readonly IBotModel _botModel;

        public void StartGame(bool needInitBots)
        {
            if (needInitBots)
            {
                _botModel.InitBots();
            }
            GameStarted.Invoke();
        }

        public void OnBotDestroyed(BotType botType)
        {
            _botModel.OnBotDestroyed(botType);
        }

        public void OnBotSpawned(BotType botType)
        {
            _botModel.OnBotSpawned(botType);
        }

        public void NeedSpawn(BotInfo botType)
        {
            SpawnBotRequested(botType);
        }

        public void OnGameFailed()
        {
            GameFailed.Invoke();
        }
    }
}
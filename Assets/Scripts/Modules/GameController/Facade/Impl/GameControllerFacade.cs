using System;
using Modules.GameController.Data;
using Modules.GameController.Models;
using Zenject;

namespace Modules.GameController.Facade.Impl
{
    public class GameControllerFacade : IGameControllerFacade
    {
        public event Action<BotInfo> SpawnBotRequested = delegate { };
        public event Action GameStarted = delegate { };
        public event Action GameFailed = delegate { };
        public event Action<int> PointUpdated = delegate { };

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

        public void OnBotDestroyed(BotType botType, bool byPlayer)
        {
            _botModel.OnBotDestroyed(botType, byPlayer);
        }

        public void OnBotSpawned(BotType botType)
        {
            _botModel.OnBotSpawned(botType);
        }

        public void RequestSpawn(BotInfo botType)
        {
            SpawnBotRequested(botType);
        }

        public void OnGameFailed()
        {
            GameFailed.Invoke();
        }
        
        public void OnPointUpdated(int points)
        {
            PointUpdated.Invoke(points);
        }
    }
}
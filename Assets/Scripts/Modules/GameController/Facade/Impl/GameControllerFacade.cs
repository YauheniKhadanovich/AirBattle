using System;
using Modules.GameController.Data;
using Modules.GameController.Models;
using Zenject;

namespace Modules.GameController.Facade.Impl
{
    public class GameControllerFacade : IGameControllerFacade
    {
        public event Action<BotInfo> SpawnBotRequested = delegate { };
        public event Action DestroyBotsRequested = delegate { };
        public event Action GameStarted = delegate { };
        public event Action GameFailed = delegate { };
        public event Action<int> PointUpdated = delegate { };

        [Inject]
        private readonly IGameModel _gameModel;

        public void StartGame(bool isRestart)
        {
            if (isRestart)
            {
                DestroyBotsImmediately();
            }
            else
            {
                _gameModel.InitBots();
            }

            GameStarted.Invoke();
        }

        public void OnBotDestroyed(BotType botType, bool byPlayer)
        {
            _gameModel.OnBotDestroyed(botType, byPlayer);
        }

        public void OnBotSpawned(BotType botType)
        {
            _gameModel.OnBotSpawned(botType);
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

        public void DestroyBotsImmediately()
        {
            DestroyBotsRequested.Invoke();
        }
    }
}
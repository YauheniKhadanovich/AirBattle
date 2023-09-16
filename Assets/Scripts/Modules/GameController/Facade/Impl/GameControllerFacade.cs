using System;
using System.Collections.Generic;
using Modules.GameController.Data;
using Modules.GameController.Models;
using Zenject;

namespace Modules.GameController.Facade.Impl
{
    public class GameControllerFacade : IGameControllerFacade
    {
        [Inject] 
        private readonly IGameModel _gameModel;
        
        public event Action DestroyBotsRequested = delegate { };
        public event Action GameStarted = delegate { };
        public event Action GameFailed = delegate { };
        public event Action<int> PointUpdated = delegate { };

        public Dictionary<string, BotInfo> Bots => _gameModel.Bots;

        public void StartGame(bool isRestart)
        {
            _gameModel.StartGame(isRestart);
        }

        public void OnBotDestroyed(bool byPlayer)
        {
            _gameModel.OnBotDestroyed(byPlayer);
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

        public void OnGameStarted()
        {
            GameStarted.Invoke();
        }
    }
}
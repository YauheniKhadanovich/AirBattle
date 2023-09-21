using System;
using System.Collections.Generic;
using Modules.GameController.Data;
using Modules.GameController.Models;
using Modules.GameController.Models.Impl;
using Zenject;

namespace Modules.GameController.Facade.Impl
{
    public class GameControllerFacade : IGameControllerFacade, IInitializable, IDisposable
    {
        [Inject] 
        private readonly IGameModel _gameModel;

        public event Action DestroyBotsRequested = delegate { };
        public event Action GameStarted = delegate { };
        public event Action GameFailed = delegate { };
        public event Action<Level> LevelUpdated = delegate { };
        public event Action<int> PointsUpdated = delegate { };

        public Dictionary<string, BotInfo> Bots => _gameModel.Bots;

        public void Initialize()
        {
            _gameModel.LevelUpdated += OnLevelUpdated;
            _gameModel.DestroyBotsRequested += OnDestroyBotsRequested;
            _gameModel.GameStarted += OnGameStarted;
            _gameModel.PointsUpdated += OnPointUpdated;
            _gameModel.GameFailed += OnGameFailed;
        }

        public void Dispose()
        {
            _gameModel.LevelUpdated -= OnLevelUpdated;
            _gameModel.DestroyBotsRequested -= OnDestroyBotsRequested;
            _gameModel.GameStarted -= OnGameStarted;
            _gameModel.PointsUpdated -= OnPointUpdated;
            _gameModel.GameFailed -= OnGameFailed;
        }

        public void StartGame(bool isRestart)
        {
            _gameModel.StartGame(isRestart);
        }

        public void DestroyBot(string botId, bool byPlayer)
        {
            _gameModel.DestroyBot(botId, byPlayer);
        }

        public void FailGame()
        {
            _gameModel.FailGame();
        }

        private void OnPointUpdated(int points)
        {
            PointsUpdated.Invoke(points);
        }

        private void OnDestroyBotsRequested()
        {
            DestroyBotsRequested.Invoke();
        }

        private void OnGameStarted()
        {
            GameStarted.Invoke();
        }

        private void OnLevelUpdated(Level level)
        {
            LevelUpdated.Invoke(level);
        }
        
        private void OnGameFailed()
        {
            GameFailed.Invoke();
        }
    }
}
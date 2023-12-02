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

        public event Action ClearLevelRequested = delegate { };
        public event Action GameStarted = delegate { };
        public event Action GameFailed = delegate { };
        public event Action<Level, bool> LevelUpdated = delegate { };
        public event Action<int> PointsUpdated = delegate { };

        public Dictionary<string, BotInfo> Bots => _gameModel.Bots;

        public void Initialize()
        {
            _gameModel.LevelUpdated += OnLevelUpdated;
            _gameModel.ClearLevelRequested += OnClearLevelRequested;
            _gameModel.GameStarted += OnGameStarted;
            _gameModel.PointsUpdated += OnPointUpdated;
            _gameModel.GameFailed += OnGameFailed;
        }

        public void Dispose()
        {
            _gameModel.LevelUpdated -= OnLevelUpdated;
            _gameModel.ClearLevelRequested -= OnClearLevelRequested;
            _gameModel.GameStarted -= OnGameStarted;
            _gameModel.PointsUpdated -= OnPointUpdated;
            _gameModel.GameFailed -= OnGameFailed;
        }

        public void ReportStartClicked(bool isRestart)
        {
            _gameModel.ReportStartClicked(isRestart);
        }

        public void ReportBotDestroyed(string botId, bool byPlayer)
        {
            _gameModel.ReportBotDestroyed(botId, byPlayer);
        }

        public void ReportPlayerDestroyed()
        {
            _gameModel.ReportPlayerDestroyed();
        }

        public void ReportCoinTaken()
        {
            _gameModel.ReportCoinTaken();
        }

        private void OnPointUpdated(int points)
        {
            PointsUpdated.Invoke(points);
        }

        private void OnClearLevelRequested()
        {
            ClearLevelRequested.Invoke();
        }

        private void OnGameStarted()
        {
            GameStarted.Invoke();
        }

        private void OnLevelUpdated(Level level, bool onlyProgressUpdated)
        {
            LevelUpdated.Invoke(level, onlyProgressUpdated);
        }
        
        private void OnGameFailed()
        {
            GameFailed.Invoke();
        }
    }
}
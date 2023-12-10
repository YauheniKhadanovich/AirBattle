using System;
using System.Collections.Generic;
using Features.Aircraft.Components;
using Modules.GameController.Models;
using Modules.GameController.Repositories;
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
        public event Action<int> LevelUpdated = delegate { };
        public event Action<int, int> LevelProgressUpdated= delegate { };
        public event Action<int> PointsUpdated = delegate { };

        public IReadOnlyDictionary<string, BotIngameState> Bots => _gameModel.BotStates;
        public bool GameInProgress => _gameModel.GameInProgress;

        public void Initialize()
        {
            _gameModel.LevelUpdated += OnLevelUpdated;
            _gameModel.LevelProgressUpdated += OnLevelProgressUpdated;
            _gameModel.ClearLevelRequested += OnClearLevelRequested;
            _gameModel.GameStarted += OnGameStarted;
            _gameModel.PointsUpdated += OnPointUpdated;
            _gameModel.GameFailed += OnGameFailed;
        }

        public void Dispose()
        {
            _gameModel.LevelUpdated -= OnLevelUpdated;
            _gameModel.LevelProgressUpdated -= OnLevelProgressUpdated;
            _gameModel.ClearLevelRequested -= OnClearLevelRequested;
            _gameModel.GameStarted -= OnGameStarted;
            _gameModel.PointsUpdated -= OnPointUpdated;
            _gameModel.GameFailed -= OnGameFailed;
        }

        public void ReportStartClicked(bool isRestart)
        {
            _gameModel.ReportStartClicked(isRestart);
        }

        public void ReportBotDestroyed(int reward, string botId, bool wasDestroyedByPlayer)
        {
            _gameModel.ReportBotDestroyed(reward, botId, wasDestroyedByPlayer);
        }

        public void ReportPlayerDestroyed()
        {
            _gameModel.ReportPlayerDestroyed();
        }

        public void ReportCoinTaken()
        {
            _gameModel.ReportCoinTaken();
        }

        public AircraftBody GetCurrentAircraftBodyPrefab()
        {
            return _gameModel.GetCurrentAircraftBodyPrefab();
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
        
        
        private void OnLevelUpdated(int levelId)
        {
             LevelUpdated.Invoke(levelId);
        }
        
        private void OnLevelProgressUpdated(int currentPoints, int targetPoints)
        {
            LevelProgressUpdated.Invoke(currentPoints, targetPoints);
        }
        
        private void OnGameFailed()
        {
            GameFailed.Invoke();
        }
    }
}
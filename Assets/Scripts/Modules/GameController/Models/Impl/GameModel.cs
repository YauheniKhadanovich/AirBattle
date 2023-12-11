using System;
using System.Collections.Generic;
using Features.Aircraft.Components;
using Modules.GameController.Repositories;
using Modules.GameController.Repositories.Impl;
using UnityEngine;
using Zenject;

namespace Modules.GameController.Models.Impl
{
    public class GameModel : IGameModel, IInitializable, IDisposable
    {
        private readonly LevelsRepository _levelsRepository;
        private readonly AircraftRepository _aircraftRepository;

        public event Action<int> LevelUpdated = delegate { };
        public event Action<int, int> LevelProgressUpdated = delegate { };
        public event Action<int> PointsUpdated = delegate { };
        public event Action GameStarted = delegate { };
        public event Action ClearLevelRequested = delegate { };
        public event Action GameFailed = delegate { };

        private int _coins;

        public Dictionary<string, BotIngameState> BotStates { get; } = new();
        public bool WasStarted { get; private set; } // TODO: refactoring
        public bool GameInProgress { get; private set; } // TODO: refactoring

        private int Coins
        {
            get => _coins;
            set
            {
                _coins = Mathf.Clamp(value, 0, int.MaxValue);
                PointsUpdated.Invoke(_coins);
            }
        }
        
        public GameModel(LevelsRepository levelsRepository, AircraftRepository aircraftRepository)
        {
            _levelsRepository = levelsRepository;
            _aircraftRepository = aircraftRepository;
        }

        public void Initialize()
        {
            _levelsRepository.LevelUpdated += OnLevelUpdated;
            _levelsRepository.LevelProgressUpdated += OnLevelProgressUpdated;
            _levelsRepository.FirstLevel();
            WasStarted = false;
        }

        public void Dispose()
        {
            _levelsRepository.LevelUpdated -= OnLevelUpdated;
            _levelsRepository.LevelProgressUpdated -= OnLevelProgressUpdated;
        }
        
        private void InitBotsForLevel()
        {
            BotStates.Clear();
            foreach (var bot in _levelsRepository.IngameLevel.LevelInfo.Bots)
            {
                BotStates.Add(bot.Prefab.ID, new BotIngameState(bot));
            }
        }
        
        public void ReportStartClicked(bool isRestart)
        {
            if (isRestart)
            {
                ClearLevelRequested.Invoke();
            }

            WasStarted = true;
            GameInProgress = true;
            _levelsRepository.FirstLevel();
            InitBotsForLevel();
            GameStarted.Invoke();
        }

        public void ReportBotDestroyed(int reward, string botId, bool wasDestroyedByPlayer)
        {
            if (wasDestroyedByPlayer)
            {
                AddLevelProgress(reward);
            }

            if (BotStates.TryGetValue(botId, out var bot))
            {
                bot.ReduceSpawnedBotsCount();
            }
        }
        
        public void ReportPlayerDestroyed()
        {
            if (!GameInProgress)
            {
                return;
            }

            GameInProgress = false;
            GameFailed.Invoke();
        }

        public void ReportCoinTaken()
        {
            Coins += 1;
        }

        public AircraftBody GetCurrentAircraftBodyPrefab()
        {
            return _aircraftRepository.GetCurrentBody();
        }

        private void AddLevelProgress(int reward)
        {
            _levelsRepository.AddReward(reward);
            if (_levelsRepository.IngameLevel.IsFinished)
            {
                _levelsRepository.NextLevel();
                InitBotsForLevel();
            }
        }
        
        private void OnLevelProgressUpdated(int currentPoints, int targetPoints)
        {
            LevelProgressUpdated.Invoke(currentPoints, targetPoints);
        }

        private void OnLevelUpdated(int levelIs)
        {
            if (GameInProgress)
            {
                ClearLevelRequested.Invoke();
                LevelUpdated.Invoke(levelIs);
            }
        }
    }
}
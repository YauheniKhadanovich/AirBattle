using System;
using System.Collections.Generic;
using System.Linq;
using Modules.GameController.Data;
using Modules.GameController.Data.Impl;
using UnityEngine;

namespace Modules.GameController.Models.Impl
{
    public class GameModel : IGameModel
    {
        private readonly BotsScriptableObject _botsScriptableObject;

        // TODO: serialize
        private readonly Dictionary<int, float> _levelsToMaxProgressMapping = new()
        {
            { 1, 5 },
            { 2, 20 },
            { 3, 30 },
            { 4, 50 },
            { 5, 50 },
        };

        public event Action<Level> LevelUpdated = delegate { };
        public event Action<int> PointsUpdated = delegate { };
        public event Action GameStarted = delegate { };
        public event Action ClearLevelRequested = delegate { };
        public event Action GameFailed = delegate { };

        private int _points;
        private Level _currentLevel;
        private bool _gameInProgress;

        public Dictionary<string, BotInfo> Bots { get; } = new();

        private int Points
        {
            get => _points;
            set
            {
                _points = Mathf.Clamp(value, 0, int.MaxValue);
                PointsUpdated.Invoke(_points);
            }
        }
        
        public GameModel(BotsScriptableObject botsScriptableObject)
        {
            _currentLevel = new Level(1, 0, 0);
            _botsScriptableObject = botsScriptableObject;
        }
        
        public void ReportStartClicked(bool isRestart)
        {
            if (isRestart)
            {
                ClearLevelRequested.Invoke();
            }
            else
            {
                Bots.Clear();
                foreach (var bot in _botsScriptableObject.Bots)
                {
                    Bots.Add(bot.BotConfig.BotId, new BotInfo(bot));
                }
            }

            _currentLevel = new Level(1, 0, _levelsToMaxProgressMapping[1]);
            LevelUpdated.Invoke(_currentLevel);
            InitBots();
            _gameInProgress = true;
            GameStarted.Invoke();
        }

        public void ReportBotDestroyed(string botId, bool wasDestroyedByPlayer)
        {
            if (wasDestroyedByPlayer)
            {
                var reward = Bots[botId].BotTo.BotConfig.Reward;
                AddLevelProgress(reward);
            }
        }
        
        public void ReportPlayerDestroyed()
        {
            if (!_gameInProgress)
            {
                return;
            }

            _gameInProgress = false;
            GameFailed.Invoke();
        }

        public void ReportCoinTaken()
        {
            Points += 1;
        }

        private void InitBots()
        {
            Bots.Values.ToList().ForEach(botInfo =>
            {
                var isEnable = botInfo.BotTo.BotConfig.AppearFromLevel <= _currentLevel.LevelNum && botInfo.BotTo.BotConfig.AppearToLevel >= _currentLevel.LevelNum;
                botInfo.SetBotEnable(isEnable);
            });
        }

        private void AddLevelProgress(float progress)
        {
            _currentLevel.Progress += progress;

            if (_currentLevel.Progress >= _currentLevel.MaxProgress)
            {
                var nextLevelNum = ++_currentLevel.LevelNum;
                if (!_levelsToMaxProgressMapping.TryGetValue(nextLevelNum, out var newProgress))
                {
                    newProgress = 100;
                }

                _currentLevel = new Level(nextLevelNum, 0, newProgress);
                InitBots();
            }

            LevelUpdated.Invoke(_currentLevel);
        }
    }

    public struct Level
    {
        public int LevelNum;
        public float Progress;
        public float MaxProgress;

        public Level(int levelNum, float progress, float maxProgress)
        {
            LevelNum = levelNum;
            Progress = progress;
            MaxProgress = maxProgress;
        }
    }
}
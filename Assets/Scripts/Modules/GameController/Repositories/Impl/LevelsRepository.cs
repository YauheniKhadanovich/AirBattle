using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Modules.GameController.Repositories.Impl
{
    // TODO: Refactoring
    [CreateAssetMenu(fileName = "LevelsData", menuName = "AirBattle/Levels/Generate Data", order = 1)]
    public class LevelsRepository : ScriptableObject
    {
        public event Action<int> LevelUpdated = delegate { };
        public event Action<int, int> LevelProgressUpdated = delegate { };

        [SerializeField]
        private List<LevelTo> _levels;
        
        private int _currentLevelIndex = -1;
        private IngameLevel _ingameLevel;

        public List<LevelTo> Levels => _levels;
        public IngameLevel IngameLevel => _ingameLevel;

        public void FirstLevel()
        {
            if (TryGetFirstLevel(out var level))
            {
                _ingameLevel = new IngameLevel(level, 0);
                LevelUpdated.Invoke(_currentLevelIndex + 1);
                LevelProgressUpdated.Invoke(0, 1);
            }
        }

        public void NextLevel()
        {
            if (TryGetNextLevel(out var level))
            {
                _ingameLevel = new IngameLevel(level, 0);
                LevelUpdated.Invoke(_currentLevelIndex + 1);
                LevelProgressUpdated.Invoke(_ingameLevel.CurrentPoints, _ingameLevel.LevelInfo.PointsToFinish);
            }
        }

        public void AddReward(int reward)
        {
            _ingameLevel.AddReward(reward);
            LevelProgressUpdated.Invoke(_ingameLevel.CurrentPoints, _ingameLevel.LevelInfo.PointsToFinish);
        }
        
        private bool TryGetFirstLevel(out LevelTo level)
        {
            level = _levels?.FirstOrDefault();

            if (level != null)
            {
                _currentLevelIndex = _levels.IndexOf(level);
                return true;
            }

            return false;
        }

        private bool TryGetNextLevel(out LevelTo level)
        {
            if (_currentLevelIndex >= 0 && _currentLevelIndex < _levels.Count - 1)
            {
                _currentLevelIndex++;
                level = _levels[_currentLevelIndex];
                return true;
            }

            level = null;
            return false;
        }
    }

    public class IngameLevel
    {
        public LevelTo LevelInfo;
        public int CurrentPoints { get; private set; }

        public bool IsFinished => CurrentPoints >= LevelInfo.PointsToFinish;
        
        public IngameLevel(LevelTo levelInfo, int currentPoints)
        {
            LevelInfo = levelInfo;
            CurrentPoints = currentPoints;
        }

        public void AddReward(int reward)
        {
            CurrentPoints += reward;
        }
    }
}
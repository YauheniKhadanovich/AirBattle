using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Modules.GameController.Data.Impl
{
    [CreateAssetMenu(fileName = "LevelsData", menuName = "Levels/Generate Levels Data", order = 1)]
    public class LevelsRepository : ScriptableObject
    {
        public event Action<int> LevelUpdated = delegate { };
        public event Action<int, int> LevelProgressUpdated = delegate { };

        public List<LevelTo> Levels;

        private int currentLevelIndex = -1;
        private IngameLevel _ingameLevel;

        public IngameLevel IngameLevel => _ingameLevel;

        public void FirstLevel()
        {
            if (TryGetFirstLevel(out var level))
            {
                _ingameLevel = new IngameLevel(level, 0);
                LevelUpdated.Invoke(currentLevelIndex);
            }
        }

        public void NextLevel()
        {
            if (TryGetNextLevel(out var level))
            {
                _ingameLevel = new IngameLevel(level, 0);
                LevelUpdated.Invoke(currentLevelIndex);
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
            level = Levels?.FirstOrDefault();

            if (level != null)
            {
                currentLevelIndex = Levels.IndexOf(level);
                return true;
            }

            return false;
        }

        private bool TryGetNextLevel(out LevelTo level)
        {
            if (currentLevelIndex >= 0 && currentLevelIndex < Levels.Count - 1)
            {
                currentLevelIndex++;
                level = Levels[currentLevelIndex];
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
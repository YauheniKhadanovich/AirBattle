using System;
using System.Collections.Generic;
using Modules.GameController.Data;

namespace Modules.GameController.Models
{
    public interface IGameModel
    {
        public event Action<int> LevelUpdated;
        public event Action<int, int> LevelProgressUpdated;
        event Action<int> PointsUpdated;
        event Action GameStarted;
        event Action ClearLevelRequested;
        event Action GameFailed;
        
        Dictionary<string, BotIngameState> BotStates { get; }
        bool GameInProgress { get; }
        
        void ReportStartClicked(bool isRestart);
        void ReportBotDestroyed(int reward, string botId, bool wasDestroyedByPlayer);
        void ReportPlayerDestroyed();
        void ReportCoinTaken();
    }
}
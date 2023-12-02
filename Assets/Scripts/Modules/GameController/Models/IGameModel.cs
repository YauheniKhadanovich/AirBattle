using System;
using System.Collections.Generic;
using Modules.GameController.Data;
using Modules.GameController.Models.Impl;

namespace Modules.GameController.Models
{
    public interface IGameModel
    {
        event Action<Level, bool> LevelUpdated;
        event Action<int> PointsUpdated;
        event Action GameStarted;
        event Action ClearLevelRequested;
        event Action GameFailed;
        
        Dictionary<string, BotInfo> Bots { get; }

        void ReportStartClicked(bool isRestart);
        void ReportBotDestroyed(string botId, bool byPlayer);
        void ReportPlayerDestroyed();
        void ReportCoinTaken();
    }
}
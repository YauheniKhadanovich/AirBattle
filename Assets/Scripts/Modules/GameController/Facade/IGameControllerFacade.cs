using System;
using System.Collections.Generic;
using Modules.GameController.Data;
using Modules.GameController.Models.Impl;

namespace Modules.GameController.Facade
{
    public interface IGameControllerFacade
    {
        event Action ClearLevelRequested;
        event Action GameFailed;
        event Action GameStarted;
        event Action<Level> LevelUpdated;
        event Action<int> PointsUpdated;

        Dictionary<string, BotInfo> Bots { get; }

        void ReportStartClicked(bool needInitBots);
        void ReportBotDestroyed(string botId, bool byPlayer);
        void ReportPlayerDestroyed();
        void ReportCoinTaken();
    }
}
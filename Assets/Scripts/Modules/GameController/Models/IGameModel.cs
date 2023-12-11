using System;
using System.Collections.Generic;
using Features.Aircraft.Components;
using Modules.GameController.Repositories;

namespace Modules.GameController.Models
{
    public interface IGameModel
    {
        event Action<int> LevelUpdated;
        event Action<int, int> LevelProgressUpdated;
        event Action<int> PointsUpdated;
        event Action GameStarted;
        event Action ClearLevelRequested;
        event Action GameFailed;
        
        Dictionary<string, BotIngameState> BotStates { get; }
        bool WasStarted { get; }
        bool GameInProgress { get; }
        
        void ReportStartClicked(bool isRestart);
        void ReportBotDestroyed(int reward, string botId, bool wasDestroyedByPlayer);
        void ReportPlayerDestroyed();
        void ReportCoinTaken();
        AircraftBody GetCurrentAircraftBodyPrefab();
    }
}
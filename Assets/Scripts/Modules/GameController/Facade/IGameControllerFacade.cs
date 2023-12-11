using System;
using System.Collections.Generic;
using Features.Aircraft.Components;
using Modules.GameController.Repositories;

namespace Modules.GameController.Facade
{
    public interface IGameControllerFacade
    {
        event Action ClearLevelRequested;
        event Action GameFailed;
        event Action GameStarted;
        event Action<int> LevelUpdated;
        event Action<int, int> LevelProgressUpdated;
        event Action<int> PointsUpdated;

        IReadOnlyDictionary<string, BotIngameState> Bots { get; }
        bool WasStarted { get; } // TODO: refactoring
        bool GameInProgress { get; } // TODO: refactoring

        void ReportStartClicked(bool needInitBots);
        void ReportBotDestroyed(int reward, string botId, bool byPlayer);
        void ReportPlayerDestroyed();
        void ReportCoinTaken();
        AircraftBody GetCurrentAircraftBodyPrefab();
    }
}
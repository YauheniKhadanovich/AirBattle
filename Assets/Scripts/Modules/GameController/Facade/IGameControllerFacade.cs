using System;
using System.Collections.Generic;
using Modules.GameController.Data;

namespace Modules.GameController.Facade
{
    public interface IGameControllerFacade
    {
        event Action DestroyBotsRequested;
        event Action GameFailed;
        event Action GameStarted;
        event Action<float> EarthUpdated;
        event Action<int> PointsUpdated;

        Dictionary<string, BotInfo> Bots { get; }

        void StartGame(bool needInitBots);
        void DestroyBot(bool byPlayer);
        void FailGame();
        void DamageEarth(float value);
    }
}
using System;
using System.Collections.Generic;
using Modules.GameController.Data;

namespace Modules.GameController.Models
{
    public interface IGameModel
    {
        event Action<float> EarthUpdated;
        event Action<int> PointsUpdated;
        event Action GameStarted;
        event Action DestroyBotsRequested;
        event Action GameFailed;
        
        Dictionary<string, BotInfo> Bots { get; }

        void StartGame(bool isRestart);
        void DestroyBot(bool byPlayer);
        void DamageEarth(float value);
        void FailGame();
    }
}
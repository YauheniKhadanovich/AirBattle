using System;
using System.Collections.Generic;
using Modules.GameController.Data;
using Modules.GameController.Models.Impl;

namespace Modules.GameController.Models
{
    public interface IGameModel
    {
        event Action<Level> LevelUpdated;
        event Action<int> PointsUpdated;
        event Action GameStarted;
        event Action DestroyBotsRequested;
        event Action GameFailed;
        
        Dictionary<string, BotInfo> Bots { get; }

        void StartGame(bool isRestart);
        void DestroyBot(string botId, bool byPlayer);
        void FailGame();
    }
}
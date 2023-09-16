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
        event Action<int> PointUpdated;

        Dictionary<string, BotInfo> Bots { get; }

        void StartGame(bool needInitBots);
        void OnBotDestroyed(bool byPlayer);
        void OnGameFailed();
        void OnPointUpdated(int points);
        void DestroyBotsImmediately();
        void OnGameStarted();
    }
}
using System.Collections.Generic;
using Modules.GameController.Data;

namespace Modules.GameController.Models
{
    public interface IGameModel
    {
        Dictionary<string, BotInfo> Bots { get; }

        void StartGame(bool isRestart);
        void OnBotDestroyed(bool byPlayer);
    }
}
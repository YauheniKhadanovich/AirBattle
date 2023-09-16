using System;
using Features.Bots.Impl;

namespace Modules.GameController.Data
{
    [Serializable]
    public class BotTo
    {
        public BotConfig BotConfig;
        public Bot BotPrefab;
    }
}
using System;
using Features.Bots.Impl;

namespace Modules.GameController.Repositories
{
    [Serializable]
    public class BotConfig
    {
        public int MaxCount;
        public float SpawnDelay;
        public Bot Prefab;

        public BotConfig()
        {
            MaxCount = 0;
            SpawnDelay = 0;
            Prefab = null;
        }
    }
}
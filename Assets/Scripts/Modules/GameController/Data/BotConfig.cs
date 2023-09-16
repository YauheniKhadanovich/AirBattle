using System;

namespace Modules.GameController.Data
{
    [Serializable]
    public struct BotConfig
    {
        public string BotId;
        public int MaxCount;
        public float SpawnDelay;

        public BotConfig(string botId, int maxCount, float spawnDelay)
        {
            BotId = botId;
            MaxCount = maxCount;
            SpawnDelay = spawnDelay;
        }
    }
}
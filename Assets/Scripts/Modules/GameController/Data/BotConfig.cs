using System;

namespace Modules.GameController.Data
{
    [Serializable]
    public struct BotConfig
    {
        public string BotId;
        public int MaxCount;
        public float SpawnDelay;
        public int Reward;

        public BotConfig(string botId, int maxCount, float spawnDelay, int reward)
        {
            BotId = botId;
            MaxCount = maxCount;
            SpawnDelay = spawnDelay;
            Reward = reward;
        }
    }
}
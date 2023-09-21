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
        public int AppearFromLevel;
        public int AppearToLevel;

        public BotConfig(string botId, int maxCount, float spawnDelay, int reward, int appearFromLevel, int appearToLevel)
        {
            BotId = botId;
            MaxCount = maxCount;
            SpawnDelay = spawnDelay;
            Reward = reward;
            AppearFromLevel = appearFromLevel;
            AppearToLevel = appearToLevel;
        }
    }
}
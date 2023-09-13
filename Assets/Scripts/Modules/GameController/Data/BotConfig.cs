namespace Modules.GameController.Data
{
    public struct BotConfig
    {
        public BotType BotType { get; private set; }
        public int MaxCount { get; private set; }
        public float SpawnDelay { get; private set; }

        public BotConfig(BotType botType, int maxCount, float spawnDelay)
        {
            MaxCount = maxCount;
            BotType = botType;
            SpawnDelay = spawnDelay;
        }
    }
}
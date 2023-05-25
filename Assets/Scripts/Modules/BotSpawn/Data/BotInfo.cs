using System;

namespace Modules.BotSpawn.Data
{
    public struct BotInfo
    {
        private int MaxCount { get; }

        public BotType BotType { get; private set; }
        public int AwaitingBotsCount { get; private set; }
        public int SpawnedBotsCount { get; private set; }
        public float SpawnDelay { get; private set; }

        public BotInfo(BotType botType, int maxCount, float spawnDelay)
        {
            MaxCount = maxCount;
            BotType = botType;
            SpawnDelay = spawnDelay;
            AwaitingBotsCount = 0;
            SpawnedBotsCount = 0;
        }

        public bool IsNeedSpawn()
        {
            return AwaitingBotsCount < MaxCount;
        }

        public void ReduceCounts()
        {
            SpawnedBotsCount = Math.Clamp(--SpawnedBotsCount, 0, 100);
            AwaitingBotsCount = Math.Clamp(--AwaitingBotsCount, 0, 100);
        }

        public void IncSpawnedBotsCount()
        {
            SpawnedBotsCount++;
        }

        public void IncAwaitingBotsCount()
        {
            AwaitingBotsCount++;
        }
    }
}
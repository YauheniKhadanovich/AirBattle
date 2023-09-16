namespace Modules.GameController.Data
{
    public class BotInfo
    {
        public string BotId { get; private set; }
        public int SpawnedBotsCount { get; private set; }
        public float SpawnTimer { get; private set; }
        public BotTo BotTo { get; private set; }

        public bool BotEnabled { get; private set; }

        public BotInfo(BotTo botTo)
        {
            BotId = botTo.BotConfig.BotId;
            BotTo = botTo;
        }

        public void SetBotEnable(bool value)
        {
            BotEnabled = value;
        }

        public void ReduceSpawnedBotsCount()
        {
            SpawnedBotsCount--;
        }

        public void IncSpawnedBotsCount()
        {
            SpawnedBotsCount++;
        }
        
        public void TimerTick(float timeDelay)
        {
            SpawnTimer += timeDelay;
        }
        
        public bool IsNeedSpawn()
        {
            return SpawnTimer > BotTo.BotConfig.SpawnDelay && SpawnedBotsCount < BotTo.BotConfig.MaxCount && BotEnabled;
        }
        
        public void Spawned()
        {
            SpawnTimer = 0;
            SpawnedBotsCount++;
        }
    }
}
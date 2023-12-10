using Features.Bots.Impl;

namespace Modules.GameController.Data
{
    public class BotIngameState
    {
        public string BotId { get; private set; }
        private readonly int _maxCount;
        private readonly float _spawnDelay;
        private readonly Bot _bot;

        public Bot BotPrefab => _bot;

        private int _spawnedBotsCount;
        private float _spawnTimer;

        public bool IsNeedSpawnByCount => _spawnedBotsCount < _maxCount;
        public bool IsNeedSpawnByTime => _spawnTimer > _spawnDelay;
        
        public BotIngameState(BotConfig botConfig)
        {
            BotId = botConfig.Prefab.ID;
            _maxCount = botConfig.MaxCount;
            _spawnDelay = botConfig.SpawnDelay;
            _bot = botConfig.Prefab;
        }

        public void ReduceSpawnedBotsCount()
        {
            _spawnedBotsCount--;
        }

        public void TimerTick(float timeDelay)
        {
            _spawnTimer += timeDelay;
        }

        public void Spawned()
        {
            _spawnTimer = 0;
            _spawnedBotsCount++;
        }
    }
}
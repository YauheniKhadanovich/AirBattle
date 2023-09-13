using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Modules.GameController.Data
{
    public class BotInfo
    {
        private readonly BotConfig _botConfig;
        
        private int MaxCount => _botConfig.MaxCount;
        private int AwaitingBotsCount { get; set; }
        private int SpawnedBotsCount { get; set; }

        public BotType BotType => _botConfig.BotType;
        public float SpawnDelay => _botConfig.SpawnDelay;

        public BotInfo(BotType botType, int maxCount, float spawnDelay)
        {
            _botConfig = new BotConfig(botType, maxCount, spawnDelay);
            AwaitingBotsCount = 0;
            SpawnedBotsCount = 0;
        }

        public bool IsNeedSpawn()
        {
            return AwaitingBotsCount < MaxCount;
        }

        public void ReduceCounts()
        {
            var reduceCountsResult = new NativeArray<int>(2, Allocator.Persistent);
            reduceCountsResult[0] = SpawnedBotsCount;
            reduceCountsResult[1] = AwaitingBotsCount;
            var reduceCountsJob = new ReduceCountsJob(reduceCountsResult);
            var handler = reduceCountsJob.Schedule();
            handler.Complete();
            SpawnedBotsCount = reduceCountsResult[0];
            AwaitingBotsCount = reduceCountsResult[1];
            reduceCountsResult.Dispose();
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
    
    [BurstCompile]
    public struct ReduceCountsJob : IJob
    {
        private NativeArray<int> _result;

        public ReduceCountsJob(NativeArray<int> result)
        {
            _result = result;
        }

        public void Execute()
        {
            if (_result[0]>0)
            {
                _result[0]--;
            }
            if (_result[1]>0)
            {
                _result[1]--;
            }
        }
    }
}
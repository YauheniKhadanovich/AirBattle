using System.Collections.Generic;
using System.Linq;
using Modules.GameController.Data;
using Modules.GameController.Facade;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Zenject;

namespace Modules.GameController.Models.Impl
{
    public class GameModel : IGameModel
    {
        [Inject] 
        private readonly IGameControllerFacade _gameControllerFacade;

        /*
         * TODO: add serialization with BotConfig
         */
        private readonly Dictionary<BotType, BotInfo> _botCounts = new()
        {
            { BotType.GreenBalloon, new BotInfo(BotType.GreenBalloon, 15, 2f) },
            { BotType.RedBalloon, new BotInfo(BotType.RedBalloon, 5, 3f) },
            { BotType.YellowBalloon, new BotInfo(BotType.YellowBalloon, 9, 5f) }
        };

        private int _points;
        
        private int Points
        {
            get => _points;
            set
            {
                _points = value;
                _gameControllerFacade.OnPointUpdated(_points);
            }
        }

        public void InitBots()
        {
            _botCounts.Keys.ToList().ForEach(SpawnIfNeed);
        }

        public void OnBotDestroyed(BotType botType, bool wasDestroyedByPlayer)
        {
            var incPointsJobResult = new NativeArray<int>(1, Allocator.Persistent);
            incPointsJobResult[0] = Points;
            var incPointsJob = new IncPointsJob
            {
                IsNeedIncreasePoints = wasDestroyedByPlayer,
                Result = incPointsJobResult
            };
            var incPointsJobHandle = incPointsJob.Schedule();
            
            ReduceBotsCount(botType);
            SpawnIfNeed(botType);
            
            incPointsJobHandle.Complete();
            Points = incPointsJobResult[0];
            incPointsJobResult.Dispose();
        }

        public void OnBotSpawned(BotType botType)
        {
            UpSpawnedBotsCount(botType);
            SpawnIfNeed(botType);
        }

        private void ReduceBotsCount(BotType botType)
        {
            _botCounts[botType].ReduceCounts();
        }

        private void SpawnIfNeed(BotType botType)
        {
            var botInfo = _botCounts[botType];
            if (botInfo.IsNeedSpawn())
            {
                _gameControllerFacade.RequestSpawn(botInfo);
                botInfo.IncAwaitingBotsCount();
            }
        }

        private void UpSpawnedBotsCount(BotType botType)
        {
            _botCounts[botType].IncSpawnedBotsCount();
        }
    }

    [BurstCompile]
    public struct IncPointsJob : IJob
    {
        [ReadOnly] public bool IsNeedIncreasePoints;
        public NativeArray<int> Result;
        
        public void Execute()
        {
            if (IsNeedIncreasePoints)
            {
                Result[0]++;
            }
        }
    }
}
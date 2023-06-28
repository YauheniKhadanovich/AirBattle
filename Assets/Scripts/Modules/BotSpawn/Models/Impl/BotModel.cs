using System.Collections.Generic;
using System.Linq;
using Modules.BotSpawn.Data;
using Modules.BotSpawn.Facade;
using Zenject;

namespace Modules.BotSpawn.Models.Impl
{
    public class BotModel : IBotModel
    {
        [Inject] 
        private readonly IBotSpawnFacade _botSpawnFacade;

        // TODO: add serialization
        private Dictionary<BotType, BotInfo> _botCounts = new()
        {
            { BotType.GreenBalloon, new BotInfo(BotType.GreenBalloon, 3, 3f) },
            { BotType.RedBalloon, new BotInfo(BotType.RedBalloon, 1, 4f) },
            { BotType.YellowBalloon, new BotInfo(BotType.YellowBalloon, 2, 3f) }
        };

        public void InitBots()
        {
            var v = _botCounts.Keys.ToList();
            v.ForEach(SpawnIfNeed);
        }

        public void OnBotDestroyed(BotType botType)
        {
            ReduceBotsCount(botType);
            SpawnIfNeed(botType);
        }

        public void OnBotSpawned(BotType botType)
        {
            UpSpawnedBotsCount(botType);
            SpawnIfNeed(botType);
        }

        private void ReduceBotsCount(BotType botType)
        {
            var botInfo = _botCounts[botType];
            botInfo.ReduceCounts();
            _botCounts[botInfo.BotType] = botInfo;
        }

        private void SpawnIfNeed(BotType botType)
        {
            var botInfo = _botCounts[botType];
            if (botInfo.IsNeedSpawn())
            {
                _botSpawnFacade.NeedSpawn(botInfo);
                botInfo.IncAwaitingBotsCount();
            }

            _botCounts[botInfo.BotType] = botInfo;
        }

        private void UpSpawnedBotsCount(BotType botType)
        {
            var botInfo = _botCounts[botType];
            botInfo.IncSpawnedBotsCount();
            _botCounts[botInfo.BotType] = botInfo;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Modules.GameController.Data;
using Modules.GameController.Facade;
using Zenject;

namespace Modules.GameController.Models.Impl
{
    public class BotModel : IBotModel
    {
        [Inject] 
        private readonly IGameControllerFacade _gameControllerFacade;

        // TODO: add serialization
        private readonly Dictionary<BotType, BotInfo> _botCounts = new()
        {
            { BotType.GreenBalloon, new BotInfo(BotType.GreenBalloon, 10, 3f) },
            { BotType.RedBalloon, new BotInfo(BotType.RedBalloon, 3, 4f) },
            { BotType.YellowBalloon, new BotInfo(BotType.YellowBalloon, 7, 3f) }
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
            var v = _botCounts.Keys.ToList();
            v.ForEach(SpawnIfNeed);
        }

        public void OnBotDestroyed(BotType botType, bool byPlayer)
        {
            ReduceBotsCount(botType);
            SpawnIfNeed(botType);
            if (byPlayer)
            {
                Points++;
            }
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
                _gameControllerFacade.RequestSpawn(botInfo);
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
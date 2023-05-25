using System;
using Modules.BotSpawn.Data;
using Modules.BotSpawn.Models;
using Zenject;

namespace Modules.BotSpawn.Facade.Impl
{
    public class BotSpawnFacade : IBotSpawnFacade
    {
        public event Action<BotInfo> NeedSpawnBot = delegate { };

        [Inject]
        private readonly IBotModel _botModel;

        public void InitBots()
        {
            _botModel.InitBots();
        }

        public void OnBotDestroyed(BotType botType)
        {
            _botModel.OnBotDestroyed(botType);
        }

        public void OnBotSpawned(BotType botType)
        {
            _botModel.OnBotSpawned(botType);
        }

        public void NeedSpawn(BotInfo botType)
        {
            NeedSpawnBot(botType);
        }
    }
}
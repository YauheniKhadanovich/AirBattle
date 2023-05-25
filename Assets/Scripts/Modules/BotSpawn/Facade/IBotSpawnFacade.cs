using System;
using Modules.BotSpawn.Data;

namespace Modules.BotSpawn.Facade
{
    public interface IBotSpawnFacade
    {
        event Action<BotInfo> NeedSpawnBot; 
        
        void InitBots();
        void OnBotDestroyed(BotType botType);
        void OnBotSpawned(BotType botType);
        void NeedSpawn(BotInfo botType);
    }
}
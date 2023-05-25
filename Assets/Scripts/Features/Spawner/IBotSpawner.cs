using Modules.BotSpawn.Data;

namespace Features.Spawner
{
    public interface IBotSpawner
    {
        void Spawn(BotInfo botInfo);
    }
}
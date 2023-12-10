using System;
using System.Collections.Generic;

namespace Modules.GameController.Repositories
{
    [Serializable]
    public class LevelTo
    {
        public int LevelId;
        public int PointsToFinish;
        public List<BotConfig> Bots;
    }
}
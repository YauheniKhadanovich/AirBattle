using System;
using System.Collections.Generic;

namespace Modules.GameController.Data
{
    [Serializable]
    public class LevelTo
    {
        public int LevelId;
        public int PointsToFinish;
        public List<BotConfig> Bots;
    }
}
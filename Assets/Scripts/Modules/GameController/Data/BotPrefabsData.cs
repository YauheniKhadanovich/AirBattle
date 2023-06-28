using System;
using Features.Bots.Impl;
using UnityEngine;

namespace Modules.GameController.Data
{
    [Serializable]
    public class BotPrefabsData
    {
        [SerializeField]
        private BotType _botType;
        [SerializeField]
        private Bot _botPrefab;

        public BotType BotType => _botType;

        public Bot BotPrefab => _botPrefab;
    }
}
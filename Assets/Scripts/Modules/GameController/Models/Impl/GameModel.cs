using System;
using System.Collections.Generic;
using System.Linq;
using Modules.GameController.Data;
using Modules.GameController.Data.Impl;
using UnityEngine;

namespace Modules.GameController.Models.Impl
{
    public class GameModel : IGameModel
    {
        private readonly BotsScriptableObject _botsScriptableObject;

        public event Action<float> EarthUpdated = delegate { };
        public event Action<int> PointsUpdated = delegate { };
        public event Action GameStarted = delegate { };
        public event Action DestroyBotsRequested = delegate { };
        public event Action GameFailed = delegate { };

        private int _points;
        private float _earthHealth;
        private bool _gameInProgress;

        public Dictionary<string, BotInfo> Bots { get; } = new();

        private int Points
        {
            get => _points;
            set
            {
                _points = Mathf.Clamp(value, 0, int.MaxValue);
                PointsUpdated.Invoke(_points);
            }
        }
        
        private float EarthHealth
        {
            get => _earthHealth;
            set
            {
                _earthHealth = Mathf.Clamp(value, 0, 100);
                EarthUpdated.Invoke(_earthHealth);
            }
        }
        
        public GameModel(BotsScriptableObject botsScriptableObject)
        {
            _botsScriptableObject = botsScriptableObject;
        }
        
        public void StartGame(bool isRestart)
        {
            if (isRestart)
            {
                DestroyBotsRequested.Invoke();
            }
            else
            {
                Bots.Clear();
                foreach (var bot in _botsScriptableObject.Bots)
                {
                    Bots.Add(bot.BotConfig.BotId, new BotInfo(bot));
                }
                InitBots();
            }

            _gameInProgress = true;
            EarthHealth = 100;
            GameStarted.Invoke();
        }

        public void DestroyBot(bool wasDestroyedByPlayer)
        {
            if (wasDestroyedByPlayer)
            {
                Points++;
            }
        }
        
        public void DamageEarth(float value)
        {
            EarthHealth -= value;
            if (EarthHealth == 0)
            {
                DestroyBotsRequested.Invoke();
                FailGame();
            }
        }

        private void InitBots()
        {
            Bots.Values.ToList().ForEach(item=>item.SetBotEnable(true));
        }
        
        public void FailGame()
        {
            if (!_gameInProgress)
            {
                return;
            }
            _gameInProgress = false;
            GameFailed.Invoke();
        }
    }
}
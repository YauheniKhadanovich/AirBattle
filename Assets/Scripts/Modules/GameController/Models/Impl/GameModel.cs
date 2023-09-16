using System.Collections.Generic;
using System.Linq;
using Modules.GameController.Data;
using Modules.GameController.Data.Impl;
using Modules.GameController.Facade;
using Zenject;

namespace Modules.GameController.Models.Impl
{
    public class GameModel : IGameModel
    {
        [Inject] 
        private readonly IGameControllerFacade _gameControllerFacade;
        private readonly BotsScriptableObject _botsScriptableObject;

        private int _points;
        
        public Dictionary<string, BotInfo> Bots { get; } = new();

        private int Points
        {
            get => _points;
            set
            {
                _points = value;
                _gameControllerFacade.OnPointUpdated(_points);
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
                _gameControllerFacade.DestroyBotsImmediately();
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

            _gameControllerFacade.OnGameStarted();
        }

        public void OnBotDestroyed(bool wasDestroyedByPlayer)
        {
            if (wasDestroyedByPlayer)
            {
                Points++;
            }
        }
        
        private void InitBots()
        {
            Bots.Values.ToList().ForEach(item=>item.SetBotEnable(true));
        }
    }
}
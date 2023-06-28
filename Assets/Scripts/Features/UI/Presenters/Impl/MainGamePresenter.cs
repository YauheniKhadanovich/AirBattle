using System;
using Features.UI.Views;
using Modules.BotSpawn.Data;
using Modules.BotSpawn.Facade;
using Zenject;

namespace Features.UI.Presenters.Impl
{
    public class MainGamePresenter : IMainGamePresenter, IInitializable, IDisposable
    {
        [Inject] 
        private IMainGameView _mainGameView;
        [Inject] 
        private IBotSpawnFacade _botSpawnFacade;
        
        public void Initialize()
        {
            _mainGameView.GoClicked += OnGoClicked;
            _mainGameView.RestartClicked += OnRestartClicked;
            _botSpawnFacade.GameFailed += OnGameFailed;
        }

        public void Dispose()
        {
            _mainGameView.GoClicked -= OnGoClicked;
            _mainGameView.RestartClicked -= OnRestartClicked;
            _botSpawnFacade.GameFailed -= OnGameFailed;
        }

        private void OnGoClicked()
        {
            _botSpawnFacade.StartGame(true);
        }
        
        private void OnRestartClicked()
        {
            _botSpawnFacade.StartGame(false);
        }

        private void OnGameFailed()
        {
            _mainGameView.ShowRestartButton();
        }
    }
}
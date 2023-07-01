using System;
using Features.UI.Views;
using Modules.GameController.Facade;
using Zenject;

namespace Features.UI.Presenters.Impl
{
    public class MainGamePresenter : IMainGamePresenter, IInitializable, IDisposable
    {
        [Inject] 
        private IMainGameView _mainGameView;
        [Inject] 
        private IGameControllerFacade _gameControllerFacade;
        
        public void Initialize()
        {
            _mainGameView.GoClicked += OnGoClicked;
            _mainGameView.RestartClicked += OnRestartClicked;
            _gameControllerFacade.GameFailed += OnGameFailed;
            _gameControllerFacade.PointUpdated += OnPointUpdated;
        }

        public void Dispose()
        {
            _mainGameView.GoClicked -= OnGoClicked;
            _mainGameView.RestartClicked -= OnRestartClicked;
            _gameControllerFacade.GameFailed -= OnGameFailed;
            _gameControllerFacade.PointUpdated -= OnPointUpdated;
        }

        private void OnGoClicked()
        {
            _gameControllerFacade.StartGame(true);
        }
        
        private void OnRestartClicked()
        {
            _gameControllerFacade.StartGame(false);
        }

        private void OnGameFailed()
        {
            _mainGameView.ShowRestartButton();
        }

        private void OnPointUpdated(int pointsCount)
        {
            _mainGameView.SetPointsCount(pointsCount);
        }
    }
}